using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour {

	public GameObject winScreen;
	public Camera caveCamera;
	public Camera winCamera;
    public GameObject menuButton;
	public static WinZone S;

	void Awake()
	{
		S = this;
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			other.GetComponent<PlayerMovement>().enterBossBattle();
		}
	}

	public static void winGame()
	{
		S.caveCamera.gameObject.SetActive(false);
		S.menuButton.SetActive(false);
		S.winCamera.gameObject.SetActive(true);
		S.winScreen.gameObject.SetActive(true);
	}
}
