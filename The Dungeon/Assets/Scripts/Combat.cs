using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Combat : MonoBehaviour {

    #region Variables

    private Sprite dwarfSprite;
    private Sprite elfSprite;
    private Sprite ruffianSprite;
    private Sprite peasantSprite;
    private Sprite khalifateSprite;
    private Sprite charSprite;

    private GameObject character;
	#region GameVariables
	private int monsterLevels = 0;
	private int monstersDefeated = 0;
    private int totalGoldCollected= 0;

	private int itemInShop = 0;

	private Sprite[] enemies;
    private GameObject enemy;
    private string enemyName;
    private string[] bosses = { "Giant", "Ogre", "Vampire" };
    private int maxEnemyHealth = 50;
    private int enemyAttackDamage = 25;
    private int maxEnemyExperience = 100;
    private bool enemyIsBoss = false;
        
    private int experienceNeeded;
    private float totalXP;
        
	//options: 1 - weapon, 2-armor, 3-cloak, 4-dagger,
	        // 5-Spellbook, 6-bow, 7-shield, 8-druidic amulet
    private int[] possibleRogueOptions = {1, 2, 3, 4, 8};
    private int[] possibleWarriorOptions = {1, 2, 3, 7, 8};
    private int[] possibleWizardOptions = {1, 2, 3, 5, 8};
    private int[] possibleArcherOptions = {1, 2, 3, 6, 8};
    private int[] possibleDruidOptions = {1, 2, 3, 8, 9};
	#endregion
	#region PlayerVariables
	private int maxHealthPotions =5;
    private int maxHealth=100;
    private int currentHealth=100;
    private int attackDamage = 50;
    private int numHealthPotions = 3;
    private int playerGold = 50;
    private int escapeRopes = 0;
    private int critChance = 5; //percent
    private int runAwayChance = 50; //percent
    private bool alreadyRanAway = false;
    private bool magicWeaponOwned = false; //+10% damage
    private bool magicArmorOwned = false; //-10% damage taken
    private bool magicCloakOwned = false; //+10% dodge chance
    private bool magicDaggerOwned = false; //rogue only - 20% chance for extra 50% attack
    private bool magicSpellbookOwned = false; //wizard only - 10% chance for random spell to be cast
    private bool magicBowOwned = false; //archer only - 10% chance for headshot (2x damage)
    private bool magicShieldOwned = false; //warrior only - +10% block, reflects 20% of all incoming damage
    private bool magicPendantOwned = false; //+5% crit chance
    private bool magicStaffOwned = false; //druid only - gets to choose 2 aspects
	#endregion
	#region DifficultyVariables
	private int healthPotionHealAmount = 30;
    private int healthPotionDropChance = 35; //Percentage
    private int attackMedium = 7;
    private int healthMedium = 20;
    private int potionGainStandard = 10;
    private int enemyDifficultyStandard = 10;
    private int experienceNeededMultiplier = 300;
	#endregion
	#region LevelingVariables
	private int level = 1;
	private int currentExperience = 0;

	private int playerClass = 0;
	private int dodgeBlockChance = 0;
	#endregion
	#region SpellVariables
	private int maxSpellSlots = 0;
	private int currentSpellSlots = 0;
	#endregion
    #region Buttons
    private Button topLeft;
    private Button topRight;
    private Button midLeft;
    private Button midRight;
    private Button bottomLeft;
    private Button bottomRight;
    #endregion
    #region Sliders
    private Slider playerHealth;
    private Text playerHealthText;
    private Slider playerExperience;
    private Slider enemyHealth;
    private Text enemyHealthText;
    #endregion
    public Text gameText;
    public Text potionCount;
    public GameObject enemyDisplay;
    public GameObject loseScreen;
    public GameObject[] buttons;
	#endregion

    // Awake is called when the GameObject it is attached to
        // becomes active
	void Awake()
    {
        charSprite = GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite;
        enemy = GameObject.Find("Enemy");
        character = GameObject.Find("Character");

        // Do fight logic here:
        setupDifficulty();
        setSprites();
        setupRace();
        setupMonsters();
        setButtons();
        setSliders();
        setText();
        
        fight();
    }

    private void setupDifficulty() {
		switch (SceneManager.GetActiveScene().name)
		{
			case "SandboxGame":
                attackMedium+=8;
                healthMedium+=10;
                potionGainStandard+=10;
                enemyDifficultyStandard-=5;
                healthPotionHealAmount=50;
                healthPotionDropChance=45;
                experienceNeededMultiplier=250;
                break;
            case "EasyGame":
                attackMedium+=3;
                healthMedium+=5;
                potionGainStandard+=5;
                enemyDifficultyStandard-=3;
                healthPotionHealAmount=40;
                healthPotionDropChance=40;
                experienceNeededMultiplier=275;
                break;
            case "HardGame":
                attackMedium -= 1;
                healthMedium-=5;
                potionGainStandard-=3;
                enemyDifficultyStandard+=3;
                healthPotionHealAmount=25;
                healthPotionDropChance=30;
                experienceNeededMultiplier=325;
                break;
            case "NightmareGame":
                attackMedium -= 2;
                healthMedium -=7;
                potionGainStandard -=5;
                enemyDifficultyStandard+=5;
                healthPotionHealAmount=20;
                healthPotionDropChance=25;
                experienceNeededMultiplier=350;
                break;
		}
	}

    private void setSprites()
    {
        dwarfSprite = (Sprite)Resources.Load("Images/Upgradable PC/Dwarves/Dwarvish Fighter", typeof(Sprite));
        elfSprite = (Sprite)Resources.Load("Images/Upgradable PC/Elves/Elvish Fighter", typeof(Sprite));
        peasantSprite = (Sprite)Resources.Load("Images/Upgradable PC/Humans/Peasant", typeof(Sprite));
        ruffianSprite = (Sprite)Resources.Load("Images/Upgradable PC/Humans/Ruffian", typeof(Sprite));
        khalifateSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Arif", typeof(Sprite));
    }

    private void setButtons()
    {
        Button[] Buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in Buttons)
        {
            switch (b.name)
            {
                case "TopLeft":
                    topLeft = b;
                    break;
                case "TopRight":
                    topRight = b;
                    break;
                case "MidLeft":
                    midLeft = b;
                    break;
                case "MidRight":
                    midRight = b;
                    break;
                case "BottomLeft":
                    bottomLeft = b;
                    break;
                case "BottomRight":
                    bottomRight = b;
                    break;
            }
        }
    }

    private void setSliders()
    {
        GameObject playerDisplay = GameObject.Find("Player Display");
        Slider[] sliders = playerDisplay.GetComponentsInChildren<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.name == "HP Bar")
                playerHealth = s;
            else if (s.name == "XP Bar")
                playerExperience = s;
        }
        playerHealth.maxValue = maxHealth;
        playerHealth.value = playerHealth.maxValue;
        int factor = (int)Mathf.Pow(2, level - 1);
        experienceNeeded = experienceNeededMultiplier * factor;
        playerExperience.maxValue = experienceNeeded;
        playerExperience.value = 0;
        enemyHealth = enemyDisplay.GetComponentInChildren<Slider>();
        enemyHealth.maxValue = maxEnemyHealth;
        enemyHealth.value =  enemyHealth.maxValue;
    }

    private void setText()
    {
        Text[] texts = GameObject.Find("Player Display").GetComponentsInChildren<Text>();
        playerHealthText = setHealthText(texts, playerHealthText);
        texts = enemyDisplay.GetComponentsInChildren<Text>();
        enemyHealthText = setHealthText(texts, enemyHealthText);
        playerHealthText.text = playerHealth.value.ToString() + "/" + playerHealth.maxValue.ToString();
    }

    private Text setHealthText(Text[] texts, Text textToSet)
    {
        foreach (Text t in texts)
        {
            if (t.name == "Text")
                textToSet = t;
        }
        return textToSet;
    }

    private void setupMonsters()
    {
        Object[] monsters = Resources.LoadAll("Images/Monsters", typeof(Sprite));
        enemies = new Sprite[monsters.Length];
        for (int i = 0; i < monsters.Length; i++)
        {
            enemies[i] = (Sprite)monsters[i];
        }
    }

    private void setupRace()
    {
        if (charSprite == dwarfSprite) {
            healthMedium += 5;
            maxHealth += 50;
        }
        else if(charSprite == elfSprite)
        {
            dodgeBlockChance += 5;
            runAwayChance += 25;
        }
        else if(charSprite == peasantSprite)
        {
            healthMedium += 2;
            maxHealth += 25;
            attackMedium += 1;
            attackDamage += 5;
        }
        else if(charSprite == ruffianSprite)
        {
            attackMedium += 2;
            attackDamage += 10;
            playerGold *= 2;
        }
        else if(charSprite == khalifateSprite)
        {
            attackMedium += 5;
            attackDamage += 20;
        }

    }

    private void updateSliders()
    {
        playerHealthText.text = playerHealth.value.ToString() + "/" + playerHealth.maxValue.ToString();
        enemyHealthText.text  = enemyHealth.value.ToString() + "/" + enemyHealth.maxValue.ToString();
    }

    private void attack()
    {
        
        int damageDealt = Random.Range(0, attackDamage);
		int damageTaken = Random.Range(0, enemyAttackDamage);

        if(Random.Range(0,100)<critChance)
        {
            damageDealt *= 2;
            gameText.text = "\nYou crit for double damage!";
        }
		enemyHealth.value -= damageDealt;
        if(Random.Range(0,100)<dodgeBlockChance)
        {
            damageTaken = 0;
            gameText.text = "\nYou dodge out of the way of the " + enemy.name + "'s attack!";
        }
		playerHealth.value -= damageTaken;
        updateSliders();

        if (playerHealth.value < 1) {
            gameText.text = "You have taken too much damage, you are too weak to go on";
            gameText.text = gameText.text.ToUpper();
            playerDeath();
        }
        if (enemyHealth.value < 1)
            enemyDefeated();
        Invoke("characterDisappear", 0.05f);
    }

    //public static double NextGaussianDouble(this Random r)
    //{
    //    double u, v, S;

    //    do
    //    {
    //        u = 2.0 * Random.value - 1.0;
    //        v = 2.0 * Random.value - 1.0;
    //        S = u * u + v * v;
    //    }
    //    while (S >= 1.0);

    //    double fac = Math.Sqrt(-2.0 * Math.Log(S) / S);
    //    return u * fac;
    //}

    private void characterDisappear()
    {
        enemy.SetActive(false);
        character.SetActive(false);
        Invoke("characterReappear", 0.05f);
    }

    private void characterReappear()
    {
        character.SetActive(true);
        enemy.SetActive(true);
    }

    private void drinkPotion()
    {
        if (numHealthPotions > 0) {
            playerHealth.value += healthPotionHealAmount;
            updateSliders();
            numHealthPotions--;
            updatePotionCount();
        } else {
            gameText.text  = "You have no health potions, defeat enemies for a chance to get one";
            gameText.text = gameText.text.ToUpper();
        }
    }

    private void runAway()
    {
        if (Random.Range(0, 100) > runAwayChance && !alreadyRanAway) //should there be a variable that dictates the % chance you have to actually escape??
        {
            gameText.text = "You run away from the " + enemyName + ".";
            gameText.text = gameText.text.ToUpper();

            
            nextEnemy();
        }
        else
        {
            gameText.text = "You can't get away!";
            gameText.text = gameText.text.ToUpper();
            alreadyRanAway = true;
        }
    }

    // TODO: 
        // Fix bug where if youre going to level up and will gain more experience than needed
        // it gives too much experience.
    private void enemyDefeated()
    {
        alreadyRanAway = false;
        topLeft.interactable = false;
        topRight.interactable = false;
        bottomRight.interactable = false;
        gameText.text = enemyName + " was defeated! You gain " + maxEnemyExperience + " experience!";
        totalXP = playerExperience.value + maxEnemyExperience;
        float timeDelay = 0.01f;
        InvokeRepeating("addExperience", 0f, timeDelay);
        if (!IsInvoking("addExperience"))
        {
            enemyDefeated2();
        }
        else
            Invoke("enemyDefeated2", timeDelay * maxEnemyExperience);
    }

    private void enemyDefeated2() // needs new name
    {
        if (playerExperience.value == playerExperience.maxValue)
        {
            levelUp();
        }
        if (Random.Range(0,100) < healthPotionDropChance) 
        {
            numHealthPotions++;
            gameText.text += "\nThe " + enemyName + " dropped a health potion!";
            string hpnum = "health potions";
            if (numHealthPotions == 1)
                hpnum = "health potion";
            gameText.text += "\nYou now have " + numHealthPotions + " " + hpnum + "!";
            updatePotionCount();
        }
        nextEnemy();
    }

    private void addExperience()
    {
        playerExperience.value++;
        if (playerExperience.value == totalXP || playerExperience.value == playerExperience.maxValue)
        {
            CancelInvoke("addExperience");
        }
    }

    private void levelUp()
    {
        level++;
        // determines if there is any excess/rollover xp
        if (playerExperience.value != totalXP)
        {
            totalXP = totalXP - playerExperience.value;
        }
        else
        { 
            totalXP = 0;
        }

        // resets xp bar
        playerExperience.value -= playerExperience.maxValue;
        // increments xp cap
        int factor = (int)Mathf.Pow(2, level - 1);
        experienceNeeded = experienceNeededMultiplier * factor;
        playerExperience.maxValue = experienceNeeded;
        // adds rollover xp
        InvokeRepeating("addExperience", 0f, 0.01f);

        updateStats();
        updateSliders();
        gameText.text += "\nYou leveled up to level " + level + "!";
        gameText.text += "\nYou gained " + attackMedium + " attack damage - you now have " + attackDamage + "!";
        gameText.text += "\nYou gained " + healthMedium + " health - you now have " + maxHealth + "!";
        gameText.text += "\nYour health potions heal for " + potionGainStandard + " more - they now heal for " + healthPotionHealAmount + "!";

        if(level == 3 || level == 5 || level == 8 || level == 10 || level == 15)
        {
            chooseClass();
        }
        
    }

    private void chooseClass()
    {
        if (level == 3)
        {
            if (charSprite == ruffianSprite || charSprite == peasantSprite)
            {
                Sprite cavalrymanSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Cavalryman", typeof(Sprite));
                //cavalryman
                Sprite darkAdeptSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Dark Adept", typeof(Sprite));
                //dark adept
                Sprite fencerSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Fencer", typeof(Sprite));
                //fencer
                Sprite heavyInfantrymanSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Heavy Infantryman", typeof(Sprite));
                //heavy infantryman
                Sprite horsemanSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Horseman", typeof(Sprite));
                //horseman
                Sprite mageSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Mage", typeof(Sprite));
                //mage
                Sprite bowmanSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Bowman", typeof(Sprite));
                //bowman
                Sprite spearmanSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Spearman", typeof(Sprite));
                //spearman
                Sprite footpadSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Footpad", typeof(Sprite));
                //footpad
                Sprite thugSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Thug", typeof(Sprite));
                //thug
                Sprite sergeantSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Sergeant", typeof(Sprite));
                //sergeant
                Sprite thiefSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Thief", typeof(Sprite));
                //thief
                Sprite poacherSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Poacher", typeof(Sprite));
                //poacher
            }
            else if (charSprite == dwarfSprite)
            {
                //fighter
                Sprite fighterSprite = (Sprite)Resources.Load("Images/Upgradable PC/Dwarves/Dwarvish Fighter", typeof(Sprite));
                //guardsman
                Sprite guardsmanSprite = (Sprite)Resources.Load("Images/Upgradable PC/Dwarves/Dwarvish Guardsman", typeof(Sprite));
                //scout
                Sprite scoutSprite = (Sprite)Resources.Load("Images/Upgradable PC/Dwarves/Dwarvish Scout", typeof(Sprite));
                //thunderer
                Sprite thundererSprite = (Sprite)Resources.Load("Images/Upgradable PC/Dwarves/Dwarvish Thunderer", typeof(Sprite));
                //ulfserker
                Sprite ulfserkerSprite = (Sprite)Resources.Load("Images/Upgradable PC/Dwarves/Dwarvish Ulfserker", typeof(Sprite));
            }
            else if (charSprite == elfSprite)
            {
                //archer
                Sprite archerSprite = (Sprite)Resources.Load("Images/Upgradable PC/Elves/Elvish Archer", typeof(Sprite));
                //fighter
                Sprite fighterSprite = (Sprite)Resources.Load("Images/Upgradable PC/Elves/Elvish Fighter", typeof(Sprite));
                //scout
                Sprite scoutSprite = (Sprite)Resources.Load("Images/Upgradable PC/Elves/Elvish Scout", typeof(Sprite));
                //shaman
                Sprite shamanSprite = (Sprite)Resources.Load("Images/Upgradable PC/Elves/Elvish Scout", typeof(Sprite));
            }
            else if (charSprite == khalifateSprite)
            {
                //arif
                Sprite arifSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Arif", typeof(Sprite));
                //hakim
                Sprite hakimSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Hakim", typeof(Sprite));
                //jundi
                Sprite jundiSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Jundi", typeof(Sprite));
                //khaiyal
                Sprite khaiyalSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Khaiyal", typeof(Sprite));
                //naffat
                Sprite naffatSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Naffat", typeof(Sprite));
                //rami
                Sprite ramiSprite = (Sprite)Resources.Load("Images/Upgradable PC/Khalifate/Rami", typeof(Sprite));
            }
        }
        else if(level ==5)
        {
            if (charSprite == dwarfSprite)
            {
                //runesmith
                Sprite runesmithSprite = (Sprite)Resources.Load("Images/Upgradable PC/Dwarf/Dwarvish Runesmith", typeof(Sprite));
            }
            else if (charSprite == elfSprite)
            {
                //Lord
                Sprite lordSprite = (Sprite)Resources.Load("Images/Upgradable PC/Elves/Elvish Lord", typeof(Sprite));
            }
        }
        else if(level == 8)
        {
            if (charSprite == elfSprite)
            {
                //lady
                Sprite ladySprite = (Sprite)Resources.Load("Images/Upgradable PC/Elves/Elvish Lady", typeof(Sprite));
            }
        }
        else if(level == 10)
        {
            if (charSprite == ruffianSprite || charSprite == peasantSprite)
            {
                //royal warrior
                Sprite royalSprite = (Sprite)Resources.Load("Images/Upgradable PC/Humans/Royal Warrior", typeof(Sprite));
            }
            else if(charSprite == dwarfSprite)
            {
                //arcanister
                Sprite arcanisterSprite = (Sprite)Resources.Load("Images/Upgradable PC/Dwarves/Dwarvish Arcanister", typeof(Sprite));
            }
        }
        else if(level == 12)
        {
            // Does something needs to be here?
        }
        else if(level == 15)
        {
            if (charSprite == ruffianSprite || charSprite == peasantSprite)
            {
                //elder mage
                Sprite elderMageSprite = (Sprite)Resources.Load("Images/Upgradable PC/Humans/Elder Mage", typeof(Sprite));
            }
        }

    }

    private void updateStats()
    {
        attackDamage += attackMedium;
        maxHealth += healthMedium;
        healthPotionHealAmount += potionGainStandard;
        playerHealth.maxValue = maxHealth;
        playerHealth.value = playerHealth.maxValue;

        enemyAttackDamage += enemyDifficultyStandard;
        maxEnemyHealth += enemyDifficultyStandard;
        enemyHealth.maxValue = maxEnemyHealth;
    }

    private void playerDeath()
    {
        gameText.text = "You limp out of the dungeon, weak from battle.";
        loseScreen.SetActive(true);
        Button[] allButtons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in allButtons)
        {
            b.interactable = false;
        }
    }

    private void reducePlayerHealth()
    {
        playerHealth.value -= 5;
        if (playerHealth.value < 1)
        {
            playerDeath();
        }
        playerHealthText.text = playerHealth.value.ToString() + "/" + playerHealth.maxValue.ToString();
    }

    private void updatePotionCount()
    {
        potionCount.text = "POTIONS: " + numHealthPotions;
    }

    private void nextEnemy()
    {
        buttons[0].SetActive(false);
        bottomRight.gameObject.SetActive(true);
        bottomLeft.gameObject.SetActive(true);
        bottomLeft.GetComponentInChildren<Text>().text = "CONTINUE";
        bottomLeft.onClick.AddListener(spawnNewEnemy);
        bottomRight.GetComponentInChildren<Text>().text = "VISIT SHOP\nGold = " + playerGold;
        bottomRight.onClick.AddListener(visitShop);
    }

    private void spawnNewEnemy()
    {
        enemy.GetComponent<SpriteRenderer>().sprite = enemies[Random.Range(0, enemies.Length)];
        enemyDisplay.GetComponentInChildren<Text>().text = enemy.GetComponent<SpriteRenderer>().sprite.name.ToUpper();
        enemyHealth.value = enemyHealth.maxValue;
        enemyName = enemyDisplay.GetComponentInChildren<Text>().text;
        updateSliders(); 
        buttons[0].SetActive(true);
        bottomRight.gameObject.SetActive(true);
        bottomLeft.gameObject.SetActive(false);
        topLeft.interactable = true;
        topRight.interactable = true;
        bottomRight.interactable = true;
    }

    private void fight()
    {
        gameText.text = "";
        updatePotionCount();
        buttons[1].SetActive(false);
        bottomLeft.gameObject.SetActive(false);
        topLeft.GetComponentInChildren<Text>().text = "ATTACK";
        topLeft.onClick.AddListener(attack);
        topRight.GetComponentInChildren<Text>().text = "DRINK POTION";
        topRight.onClick.AddListener(drinkPotion);
        bottomRight.GetComponentInChildren<Text>().text = "RUN AWAY";
        bottomRight.onClick.AddListener(runAway);
        Invoke("spawnEnemy", 0.5f);
    }

    private void spawnEnemy()
    {
        enemy.GetComponent<SpriteRenderer>().sprite = enemies[Random.Range(0, enemies.Length)];
        enemyDisplay.SetActive(true);
        enemyDisplay.GetComponentInChildren<Text>().text = enemy.GetComponent<SpriteRenderer>().sprite.name.ToUpper();
        updateSliders();
        enemyName = enemyDisplay.GetComponentInChildren<Text>().text;
    }

    private void visitShop()
    {
        SceneManager.LoadScene("Shop");
    }
}