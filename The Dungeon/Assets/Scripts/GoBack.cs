using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoBack : MonoBehaviour {

	void Start () {
		gameObject.GetComponent<Button>().onClick.AddListener(goBack);		
	}
	
	private void goBack()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
