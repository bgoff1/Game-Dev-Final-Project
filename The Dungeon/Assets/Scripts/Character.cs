using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public int attackDamage;
    public int maxHealth;
    public int level;
    public int attackGrowth;
    public int healthGrowth;
    public GameObject display;
    public Slider health;
    public string charName;
    public Text gameText;
    public Text healthText;

    private Character enemy;

    virtual public void setUpVariables()
    {
        // default to player's stats
        attackDamage = 50;
        maxHealth = 100;
        level = 1;
        attackGrowth = 7;
        healthGrowth = 20;
    }

    public abstract void setUpUI();

    public void updateHealthText()
    {
        healthText.text = health.value.ToString() + "/" + health.maxValue.ToString();
    }

    virtual public void updateStats()
    {
        attackDamage += attackGrowth;
        maxHealth += healthGrowth;
        health.maxValue = maxHealth;
        health.value = health.maxValue;
    }

    virtual public void attack(Character c)
    {
        enemy = c;
        int damageDealt = Random.Range(0, attackDamage);
        int damageTaken = Random.Range(0, c.attackDamage);
        
        health.value -= damageTaken;
        c.health.value -= damageDealt;

        updateHealthText();
        if (c.health.value < 1)
        {
            death(c);
        }
        Invoke("characterDisappear", 0.05f);
    }

    private void characterDisappear()
    {
        enemy.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Invoke("characterReappear", 0.05f);
    }

    private void characterReappear()
    {
        enemy.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public abstract void death(Character c);
}
