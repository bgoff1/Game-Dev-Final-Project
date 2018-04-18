using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Combat : MonoBehaviour {

	#region Variables
    #region Player And Enemy
    static private Player player;
	static private GameObject gEnemy;
    static private Enemy enemy;
	#endregion
    #region Buttons
	static private Button topLeft;
	static private Button topRight;
	static private Button botLeft;
	static private Button botRight;
	#endregion
	#region UI Texts
    public Text gameText;
    public Text potionCount;
	#endregion
	#region Cameras
	static public Camera caveCamera;
	static public Camera battleCamera;
	#endregion
	static private Sprite[] enemies;
	static private GameObject s;
    static public GameObject battlePanel;

	static private int runAwayChance = 30;
	static private int strongAttackCD = 0;
	static private int shieldCD = 0;
    static private Sprite boss;
	#region Display
	static public GameObject enemyDisplay;
	static public GameObject loseScreen;
	#endregion
    #endregion

	void Awake()
    {
		s = this.gameObject;
        gEnemy = GameObject.Find("Enemy");
        if (player == null)
        {
            player = gameObject.AddComponent<Player>();
        }

		setupUI();
        setupButtons();
        setUpMonsters();
		fightStart();
        //fightOptions();
    }

    private void setupUI()
	{
		enemyDisplay = transform.parent.Find("Enemy Display").gameObject;
		loseScreen = transform.parent.Find("Lose Screen").gameObject;
        battlePanel = transform.parent.Find("EndBattleScreen").gameObject;
        Camera[] cams = Resources.FindObjectsOfTypeAll<Camera>();
		foreach (Camera c in cams)
		{
			if (c.name == "BattleCamera")
				battleCamera = c;
			else if (c.name == "CaveCamera")
				caveCamera = c;
		}
	}

    private void setupButtons()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            switch (b.name)
            {
                case "TopLeft":
                    topLeft = b;
                    break;
                case "TopRight":
                    topRight = b;
                    break;
                case "BotLeft":
                    botLeft = b;
                    break;
                case "BotRight":
                    botRight = b;
                    break;
            }
        }
		battlePanel.SetActive(true);
		battlePanel.GetComponentInChildren<Button>().onClick.AddListener(BackToCave);
		battlePanel.SetActive(false);
    }

	private void setUpMonsters()
	{
		// logic found from unity's documentation
		// found here: https://docs.unity3d.com/ScriptReference/Resources.LoadAll.html
		if (enemies == null)
		{
			Object[] monsters = Resources.LoadAll("Images/Monsters", typeof(Sprite));
			enemies = new Sprite[monsters.Length];
			for (int i = 0; i < monsters.Length; i++)
			{
                if (monsters[i].name != "Skeletal Dragon")
                {
				    enemies[i] = (Sprite)monsters[i];
                }
                else
                {
                    boss = (Sprite)monsters[i];
                }
			}
		}
	}

	private void fightStart()
	{
		gameText.text = " ";
		fightScreenOne();
	}

	static private void fightScreenOne()
	{
		removeListenersOnButtons();
		botLeft.transform.parent.gameObject.SetActive(false);
		topLeft.GetComponentInChildren<Text>().text = "FIGHT";
		topLeft.onClick.AddListener(fightScreenTwo);
		topRight.GetComponentInChildren<Text>().text = "RUN AWAY";
		topRight.onClick.AddListener(runAway);
	}

    static private void fightScreenTwo()
    {
		removeListenersOnButtons();
		botLeft.transform.parent.gameObject.gameObject.SetActive(true);
        topLeft.GetComponentInChildren<Text>().text = "ATTACK";
        topLeft.onClick.AddListener(attack);
        topRight.GetComponentInChildren<Text>().text = "DRINK POTION";
        topRight.onClick.AddListener(player.drinkPotion);
        botLeft.GetComponentInChildren<Text>().text = "STRONG\nATTACK";
        botLeft.onClick.AddListener(strongAttack);
		botRight.GetComponentInChildren<Text>().text = "SHIELD";
        botRight.onClick.AddListener(callShield);
    }

	static private void removeListenersOnButtons()
	{
		topLeft.onClick.RemoveAllListeners();
		topRight.onClick.RemoveAllListeners();
		botLeft.onClick.RemoveAllListeners();
		botRight.onClick.RemoveAllListeners();
	}

	static public void performFirstAction(Button button)
	{
		if (button == topLeft)
		{
			fightScreenTwo();
		}
		else if (button == topRight)
		{
			runAway();
		}
	}

	static public void performButtonAction(Button button)
	{
		if (button == topLeft)
		{
			attack();
		}
		else if (button == topRight)
		{
			drinkPotion();
		}
		else if (button == botLeft)
		{
			if (strongAttackCD <= 0)
			{
				strongAttack();
			}
			else {
				player.strongAttackFail(strongAttackCD);
			}
		}
		else if (button == botRight)
		{
			if(shieldCD <= 0)
			{
				callShield();
			}
			else {
				player.shieldFail(shieldCD);
			}
		}
		fightScreenOne();
	}

	static private void drinkPotion()
	{
		if (enemy != null)
		{
			player.drinkPotion();
			strongAttackCD--;
			shieldCD--;	
		}
	}

	static private void callShield(){
		if(enemy != null)
		{
			shieldCD = 2;
			strongAttackCD--;
			player.shield();
		}
	}

	static private void attack()
    {
        if (enemy != null)
        {
            player.attack(enemy);
			shieldCD --;
			strongAttackCD--;
			// if enemy was killed
            if (player.enemySlain && enemy.playerDead == false)
            {
				if (enemy.tag == "Boss")
				{
					WinZone.winGame();
				} else 
					showBattlePanel();
            }
        }
    }

	static private void strongAttack()
	{
		if (enemy != null)
		{
			player.strongAttack(enemy);
			shieldCD--;
			strongAttackCD = 3;
			// if enemy was killed
			if (player.enemySlain && enemy.playerDead == false)
			{
				if (enemy.tag == "Boss") {
					WinZone.winGame();
				} else
					showBattlePanel();
			}
		}
	}

	static private void runAway()
	{
		if (enemy != null)
		{
			if (Random.Range(0, 100) < runAwayChance) 
			{
				Destroy(enemy);
				showBattlePanel();
				if (battlePanel.GetComponentInChildren<Text>().name == "Win")
					battlePanel.GetComponentInChildren<Text>().text = "YOU RAN AWAY FROM BATTLE!";
			}
			else
			{
				player.runAwayFail(enemy);
			}
			
		}
	}

	static public void showBattlePanel()
	{
		battlePanel.SetActive(true);
		ButtonsUsingKeyboard.isFighting = false;
	}

    static public void BackToCave()
    {
        battlePanel.SetActive(false);
        enemy = null;
        player.enemySlain = false;
        // change view back to cave
        battleCamera.GetComponent<AudioListener>().enabled = false;
        battleCamera.gameObject.SetActive(false);
        caveCamera.gameObject.SetActive(true);
        caveCamera.GetComponent<AudioListener>().enabled = true;
        // hide UI
        player.display.SetActive(false);
        enemyDisplay.SetActive(false);
        s.SetActive(false);
    }

	static public void spawnEnemy()
    {
        gEnemy.GetComponent<SpriteRenderer>().sprite = enemies[Random.Range(0, enemies.Length - 1)];
        if (enemy == null)
        {
            enemy = s.AddComponent<Enemy>();
            enemy.setUp(gEnemy, enemyDisplay, loseScreen);
			ButtonsUsingKeyboard.isFighting = true;
        }
    }

	static public void spawnBoss()
	{
		gEnemy.GetComponent<SpriteRenderer>().sprite = boss;
		if (enemy == null)
		{
			enemy = s.AddComponent<Enemy>();
			enemy.setUp(gEnemy, enemyDisplay, loseScreen);
			enemy.upgradeToBoss();
			enemy.tag = "Boss";
			ButtonsUsingKeyboard.isFighting = true;
		}
	}
    
}