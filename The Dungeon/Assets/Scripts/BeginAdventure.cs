using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginAdventure : MonoBehaviour {
    public Sprite femaleSprite;
    public Sprite maleSprite;
    public GameObject PlayerDisplay;
	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetString("gender") != null)
        {
            if(PlayerPrefs.GetString("gender") == "male")
            {
                GameObject.Find("Player").GetComponent<SpriteRenderer>().sprite = maleSprite;
                GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = maleSprite;
            }
            else
            {
                GameObject.Find("Player").GetComponent<SpriteRenderer>().sprite = femaleSprite;
                GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = femaleSprite;
            }
        }
        if(PlayerPrefs.GetString("playerName") != null)
        {
            PlayerDisplay.GetComponentInChildren<Text>().text = PlayerPrefs.GetString("playerName");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
