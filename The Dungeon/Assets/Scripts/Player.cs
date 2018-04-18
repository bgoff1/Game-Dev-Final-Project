using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    private bool addingXP = false;
    private int critChance = 5; //percent
    private int experienceNeededMultiplier = 3;
    private int healthPotionHealAmount = 30;
    private int healthPotionDropChance = 35; //Percentage
    private int maxHealthPotions = 5;
    private int potionHealGain = 10;
    private Character recentlySlainEnemy;
    private float totalXP;
    private int enemyExperienceReward = 1;
	private Slider experience;

    private const float TIME_DELAY = 0.01f;
    public AudioClip potionSound;
    public AudioClip swordSound;
    private AudioSource source;
    public bool enemySlain = false;
	public GameObject display;

    public void Awake()
    {
        updatePotionCount();
        setUpVariables();
        setUpUI();
        source = GetComponent<AudioSource>();
    }

    public override void setUpUI()
    {
        #region Slider
        findGameText();
		display = transform.root.Find("Player Display").gameObject;
        Slider[] sliders = display.GetComponentsInChildren<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.name == "HP Bar")
                health = s;
            else if (s.name == "XP Bar")
                experience = s;
        }
        health.maxValue = maxHealth;
        health.value = health.maxValue;
        //int factor = (int)Mathf.Pow(2, level - 1);
        //int experienceNeeded = experienceNeededMultiplier * factor;
        experience.maxValue = experienceNeededMultiplier;
        experience.value = 0;
        #endregion
        #region Text
        Text[] texts = display.GetComponentsInChildren<Text>();
        foreach (Text t in texts)
        {
            if (t.name == "Text")
                healthText = t;
        }
        updateHealthText();
        #endregion
    }

    private void findGameText()
    {
        if (gameText == null)
        {
            gameText = GameObject.Find("GameText").GetComponent<Text>();
        }
    }

    override public void updateStats()
    {
        base.updateStats();
        healthPotionHealAmount += potionHealGain;
    }

    public void shieldFail(int CD)
    {
        gameText.text = "You cannot use " + "Shield" + " for " + CD + " more turns.";
    }

    public void shield(){
        gameText.text = "You used a shield. Neither you nor the enemy took damage this turn";
    }

    public override void attack(Character c)
    {
        source.PlayOneShot(swordSound);
		gameText.text = "";
        float startEnemyHP = c.health.value;
        base.attack(c);
        // if the enemy is still alive and you critically hit
        if (c.health.value >= 1 && Random.Range(0, 100) < critChance)
        {
            // deal double damage
            float damageTakenByEnemy = startEnemyHP - c.health.value;
            c.health.value -= damageTakenByEnemy;
            c.updateHealthText();
            // kill the enemy if its health is 0
            if (c.health.value < 1)
            {
                death(c);
            }
        }
        // if your health is 0 then you die
        if (health.value < 1)
        {
            c.death(this);
        }
    }

	public void runAwayFail(Character c)
	{
		int damageTaken = Random.Range(5, (c.attackDamage * 2) / 3);
		health.value -= damageTaken;

		gameText.text = "You failed to escape and took " + damageTaken + " damage.";

		updateHealthText();
		if (health.value < 1)
			c.death(this);
	}

	public void strongAttack(Character c)
	{
        source.PlayOneShot(swordSound);
		gameText.text = "";
		float startEnemyHP = c.health.value;
		int damageDealt = Random.Range(attackDamage, attackDamage * Mathf.FloorToInt(1.5f));
		int damageTaken = Random.Range(0, c.attackDamage);

		health.value -= damageTaken;
		c.health.value -= damageDealt;

		updateHealthText();
		c.updateHealthText();
		if (c.health.value < 1)
		{
			death(c);
		}
		if (health.value < 1)
			c.death(this);
		Invoke("characterDisappear", 0.05f);
	}

	public void strongAttackFail(int CD)
	{
		gameText.text = "You cannot use " + "Strong Attack" + " for " + CD + " more turns.";
	}

    override public void death(Character c)
    {
        findGameText();
        recentlySlainEnemy = c;
        enemySlain = true;
        gameText.text = c.charName + " was defeated! You gain " + enemyExperienceReward + 
            " experience!";
        totalXP = experience.value + enemyExperienceReward;
        
        //InvokeRepeating("addExperience", 0f, TIME_DELAY);
		Invoke("addExperience", TIME_DELAY);
        /*if (!IsInvoking("addExperience"))
        {
            enemyDefeated();
        }
        else
        {*/
		enemyDefeated();
            //Invoke("enemyDefeated", TIME_DELAY * enemyExperienceReward);
        //}
    }

    private void enemyDefeated()
    {
        //if (experience.value == experience.maxValue)
            //levelUp();
        if (Random.Range(0,100) < healthPotionDropChance && 
            numHealthPotions < maxHealthPotions)
        {
            numHealthPotions++;
            gameText.text += "\nThe " + recentlySlainEnemy.charName + 
                " dropped a health potion!";
            string hpnum = "health potions";
            if (numHealthPotions == 1)
                hpnum = "health potion";
            gameText.text += "\nYou now have " + numHealthPotions + " " + hpnum + "!";
            updatePotionCount();
        }
        Enemy enemy = gameObject.GetComponentInParent<Enemy>();
        Destroy(enemy);
    }

    private void levelUp()
    {
        level++;
        // determines if there is any excess/rollover xp
        if (experience.value != totalXP)
        {
            totalXP = totalXP - experience.value;
        }
        else
        {
            totalXP = 0;
        }

        // resets xp bar
        experience.value -= experience.maxValue;
        // increments xp cap
        int factor = (int)Mathf.Pow(2, level - 1);
        int experienceNeeded = experienceNeededMultiplier * factor;
        experience.maxValue = experienceNeeded;
        // adds rollover xp
        if (totalXP != 0)
            InvokeRepeating("addExperience", 0f, 0.01f);

        updateStats();
        updateHealthText();
        gameText.text += "\nYou leveled up to level " + level + "!";
        gameText.text += "\nYou gained " + attackGrowth + 
            " attack damage - you now have " + attackDamage + "!";
        gameText.text += "\nYou gained " + healthGrowth + 
            " health - you now have " + maxHealth + "!";
        gameText.text += "\nYour health potions heal for " + 
            potionHealGain + " more - they now heal for " + healthPotionHealAmount + 
            "!";

    }

    private void addExperience()
    {
        experience.value++;
		if (experience.value == experience.maxValue)
		{
			levelUp();
			if (experience.value == totalXP)
			{
				addingXP = false;
			}
			else {
				addingXP = true;
			}
		}
		if (addingXP)
		{
			addExperience();
		}
    }

    private void updatePotionCount()
    {
        GameObject.Find("PotionCount").GetComponent<Text>().text = "POTIONS: " + 
            numHealthPotions;
    }

    public void drinkPotion()
    {
        if (numHealthPotions > 0)
        {
            source.PlayOneShot(potionSound);
            health.value += healthPotionHealAmount;
            updateHealthText();
            numHealthPotions--;
            updatePotionCount();
        }
        else
        {
            gameText.text = "You have no health potions, defeat enemies for a " +
                "chance to get one";
            gameText.text = gameText.text.ToUpper();
        }
    }
}
