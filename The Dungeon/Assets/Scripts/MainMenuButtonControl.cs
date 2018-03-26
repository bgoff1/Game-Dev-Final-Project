using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtonControl : MonoBehaviour {
	public Button startButton;
	public Button sandboxButton;
	public Button easyButton;
	public Button mediumButton;
	public Button hardButton;
	public Button nightmareButton;
	public Button infoButton;
	public Button backButton;
	public GameObject mainMenuPanel;
	public GameObject difficultyPanel;
	public GameObject infoPanel;

	private Button[] startsDisabled;
	void Start () {
		mainMenuPanel.SetActive(true);
		startButton.onClick.AddListener(showDifficulties);
		sandboxButton.onClick.AddListener(startSandbox);
		nightmareButton.onClick.AddListener(startNightmare);
		infoButton.onClick.AddListener(showInfo);
		backButton.onClick.AddListener(showDifficulties);
		difficultyPanel.SetActive(false);
		infoPanel.SetActive(false);
	}

	private void showDifficulties()
	{
		mainMenuPanel.SetActive(false);
		infoPanel.SetActive(false);
		difficultyPanel.SetActive(true);
	}

	#region  startFunctions
	private void startSandbox()
	{
		SceneManager.LoadScene("SandboxGame");
	}

	private void startNightmare()
	{
		SceneManager.LoadScene("NightmareGame");
	}
	#endregion

	private void showInfo()
	{
		difficultyPanel.SetActive(false);
		infoPanel.SetActive(true);
	}
}
