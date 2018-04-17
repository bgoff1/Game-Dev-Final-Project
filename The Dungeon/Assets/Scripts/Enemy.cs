using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character {

    private GameObject loseScreen;
    public bool playerDead;
	public GameObject display;

    public void setUp(GameObject enemy, GameObject displayP, GameObject losescreen)
    {
        display = displayP;
        setUpVariables();
        setUpUI();
        loseScreen = losescreen;
        display.SetActive(true);
        charName = display.GetComponentInChildren<Text>().text = enemy.GetComponent<SpriteRenderer>().sprite.name.ToUpper();
        updateHealthText();
        playerDead = false;
    }

    override public void setUpVariables()
    {
        attackDamage = 25;
        maxHealth = 50;
        level = 1;
        attackGrowth = 10;
        healthGrowth = 10;
    }

    override public void setUpUI()
    {
        #region Slider
        gameText = GameObject.Find("GameText").GetComponent<Text>();
        Slider[] sliders = display.GetComponentsInChildren<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.name == "HP Bar")
                health = s;
        }
        health.maxValue = maxHealth;
        health.value = health.maxValue;
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

    public override void death(Character c)
    {
        playerDead = true;
        gameText.text = "You limp out of the dungeon, weak from battle.";
        loseScreen.SetActive(true);
        Button[] allButtons = GameObject.Find("Player HUD").GetComponentsInChildren<Button>();
        foreach (Button b in allButtons)
        {
            b.interactable = false;
        }
    }

	public void upgradeToBoss()
	{
		attackDamage *= 2;
		health.maxValue *= 2;
		health.value = health.maxValue;
		level *= 10;
		updateHealthText();
	}
}
