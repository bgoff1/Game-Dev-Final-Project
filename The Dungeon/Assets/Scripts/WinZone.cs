using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour {

	public GameObject winScreen;
	public Camera caveCamera;
	public Camera winCamera;


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			caveCamera.gameObject.SetActive(false);
			winCamera.gameObject.SetActive(true);
			winScreen.gameObject.SetActive(true);
		}
	}
}
