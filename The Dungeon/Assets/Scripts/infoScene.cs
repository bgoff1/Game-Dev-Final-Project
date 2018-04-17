using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class infoScene : MonoBehaviour {

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(menuButton);
    }

    public void menuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
