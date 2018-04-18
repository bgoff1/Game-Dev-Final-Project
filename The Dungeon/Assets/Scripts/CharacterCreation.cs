using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CharacterCreation : MonoBehaviour
{

    #region variables
	public Button submitButton;
	public Button backButton;
    public GameObject input;
    public Sprite maleSprite;
    public Sprite femaleSprite;

    private Button maleButton;
    private Button femaleButton;
    #endregion

    void Start()
    {
        registerButtons();
    }

	void FixedUpdate()
	{
		if (submitButton.gameObject.activeSelf)
		{
			if (Input.GetKeyDown(KeyCode.Return))
				getName();
		}
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
		backButton.onClick.AddListener(goBack);
		submitButton.onClick.AddListener(getName);
        maleButton.onClick.AddListener(setMaleCharacter);
        femaleButton.onClick.AddListener(setFemaleCharacter);
    }

    private void setMaleCharacter()
    {
        GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = maleSprite;
        PlayerPrefs.SetString("gender", "male");
        getCharacterName();
    }

    private void setFemaleCharacter()
    {
        GameObject.Find("Character").GetComponent<SpriteRenderer>().sprite = femaleSprite;
        PlayerPrefs.SetString("gender", "female");
        getCharacterName();
        
    }

    private void getCharacterName()
    {
        gameObject.GetComponentInChildren<Text>().text = "ENTER CHARACTER NAME";
		maleButton.gameObject.SetActive(false);
		femaleButton.gameObject.SetActive(false);
        input.SetActive(true);
		backButton.gameObject.SetActive(true);
    }

    private void getName()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetString("playerName", input.GetComponent<InputField>().text.ToUpper());
        input.SetActive(false);
		string gameMode = PlayerPrefs.GetString("mode");
		if (gameMode == "")
			gameMode = "AdventureMode";
        SceneManager.LoadScene(gameMode);
    }

	private void goBack()
	{
		gameObject.GetComponentInChildren<Text>().text = "ARE YOU MALE\n OR FEMALE?";
		maleButton.gameObject.SetActive(true);
		femaleButton.gameObject.SetActive(true);
		input.SetActive(false);
		backButton.gameObject.SetActive(false);
	}
}