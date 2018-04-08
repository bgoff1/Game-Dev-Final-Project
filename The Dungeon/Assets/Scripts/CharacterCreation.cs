using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{

    #region variables
    public GameObject characterHUD;
    public GameObject characterDisplay;
    public GameObject input;
    public GameObject caveBackground;
    public Sprite maleSprite;
    public Sprite femaleSprite;

    private Button maleButton;
    private Button femaleButton;
    #endregion

    void Start()
    {
        registerButtons();
    }

    private void registerButtons()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            if (b.name == "Male")
            {
                maleButton = b;
            }
            else if (b.name == "Female")
            {
                femaleButton = b;
            }
        }
        maleButton.onClick.AddListener(setMaleCharacter);
        femaleButton.onClick.AddListener(setFemaleCharacter);
    }

    private void setMaleCharacter()
    {
        GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = maleSprite;
      //  GameObject.Find("Player").GetComponent<SpriteRenderer>().sprite = maleSprite;
        getCharacterName();
    }

    private void setFemaleCharacter()
    {
        GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = femaleSprite;
       // GameObject.Find("Player").GetComponent<SpriteRenderer>().sprite = femaleSprite;
        getCharacterName();
    }

    private void getCharacterName()
    {
        gameObject.GetComponentInChildren<Text>().text = "ENTER CHARACTER NAME";
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in buttons) { b.gameObject.SetActive(false); }
        input.SetActive(true);
        input.GetComponentInChildren<Button>().onClick.AddListener(getName);
    }

    private void getName()
    {
        gameObject.SetActive(false);
        caveBackground.SetActive(true);
        //characterDisplay.SetActive(true);
        characterDisplay.GetComponentInChildren<Text>().text = input.GetComponent<InputField>().text.ToUpper();
        // characterHUD.SetActive(true);
        input.SetActive(false);
    }
}