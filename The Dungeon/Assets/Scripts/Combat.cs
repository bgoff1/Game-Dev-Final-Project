using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Combat : MonoBehaviour {

    #region Variables
    static private Player player;
    private GameObject gEnemy;
    private Enemy enemy;
	#region GameVariables
	private int monsterLevels = 0;
	private int monstersDefeated = 0;
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
    #endregion
    #region Sliders
    /*private Slider playerHealth;
    private Text playerHealthText;
    private Slider playerExperience;
    private Slider enemyHealth;
    private Text enemyHealthText;*/

    #endregion
    public Text gameText;
    public Text potionCount;
    private Sprite[] enemies;
    public GameObject enemyDisplay;
    public GameObject loseScreen;
    public GameObject[] buttons;
	#endregion
    
	void Awake()
    {
        gEnemy = GameObject.Find("Enemy");
        if (player == null)
        {
            player = gameObject.AddComponent<Player>();
        }

        // Do fight logic here:
        setupButtons();
        setUpMonsters();
        fight();
    }

    private void setupButtons()
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
                case "BotLeft":
                    midLeft = b;
                    break;
                case "BotRight":
                    midRight = b;
                    break;
            }
        }
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

    private void fight()
    {
        gameText.text = "";
        topLeft.GetComponentInChildren<Text>().text = "ATTACK";
        topLeft.onClick.AddListener(attack);
        topRight.GetComponentInChildren<Text>().text = "DRINK POTION";
        topRight.onClick.AddListener(player.drinkPotion);
        Invoke("spawnEnemy", 0.5f);
    }

    private void attack()
    {
        if (enemy != null)
        {
            player.attack(enemy);
            // could change logic to move back to cave screen 
            if (player.enemySlain)
            {
                enemy = null;
                spawnEnemy();
                player.enemySlain = false;
            }
        }
        else
        {
            spawnEnemy();
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

    private void spawnEnemy()
    {
        gEnemy.GetComponent<SpriteRenderer>().sprite = enemies[Random.Range(0, enemies.Length)];
        if (enemy == null)
        {
            enemy = gameObject.AddComponent<Enemy>();
            enemy.setUp(gEnemy, enemyDisplay, loseScreen);
        }
    }
    
}