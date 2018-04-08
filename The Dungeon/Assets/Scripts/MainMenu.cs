using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public Button startButton;
	public Button adventureButton;
	public Button endlessButton;
	public GameObject mainMenuPanel;
	public GameObject difficultyPanel;
    
	void Start () {
		mainMenuPanel.SetActive(true);
		startButton.onClick.AddListener(showDifficulties);
        adventureButton.onClick.AddListener(startAdventure);
		endlessButton.onClick.AddListener(startEndless);
		difficultyPanel.SetActive(false);
	}

	private void showDifficulties()
	{
		mainMenuPanel.SetActive(false);
		difficultyPanel.SetActive(true);
	}

	#region  startFunctions
	private void startAdventure()
	{
        PlayerPrefs.SetString("mode", "AdventureMode");
        SceneManager.LoadScene("CharacterChoice");

    }

	private void startEndless()
	{
        PlayerPrefs.SetString("mode", "EndlessMode");
        SceneManager.LoadScene("CharacterChoice");
    }
	#endregion
}
