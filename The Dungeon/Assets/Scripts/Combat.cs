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
        fight();
    }

	private void setupUI()
	{
		enemyDisplay = transform.parent.Find("Enemy Display").gameObject;
		loseScreen = transform.parent.Find("Lose Screen").gameObject;

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
				enemies[i] = (Sprite)monsters[i];
			}
		}
	}

    private void fight()
    {
        gameText.text = " ";
        topLeft.GetComponentInChildren<Text>().text = "ATTACK";
        topLeft.onClick.AddListener(attack);
        topRight.GetComponentInChildren<Text>().text = "DRINK POTION";
        topRight.onClick.AddListener(player.drinkPotion);
    }

	static public void performButtonAction(Button button)
	{
		if (button == topLeft)
		{
			attack();
		}
		else if (button == topRight)
		{
			player.drinkPotion();
		}
		else if (button == botLeft)
		{
			print("Bottom Left button pressed.");
		}
		else if (button == botRight)
		{
			print("Bottom Right button pressed.");
		}
	}

	static private void attack()
    {
        if (enemy != null)
        {
            player.attack(enemy);
			// if enemy was killed
            if (player.enemySlain)
            {
                enemy = null;
				player.enemySlain = false;
				// change view back to cave
				battleCamera.gameObject.SetActive(false);
				caveCamera.gameObject.SetActive(true);
				// hide ui
				player.display.SetActive(false);
				enemyDisplay.SetActive(false);
				s.SetActive(false);
            }
        }
    }

	static public void spawnEnemy()
    {
        gEnemy.GetComponent<SpriteRenderer>().sprite = enemies[Random.Range(0, enemies.Length)];
        if (enemy == null)
        {
            enemy = s.AddComponent<Enemy>();
            enemy.setUp(gEnemy, enemyDisplay, loseScreen);
        }
    }
    
}