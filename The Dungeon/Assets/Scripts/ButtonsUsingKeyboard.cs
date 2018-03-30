using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsUsingKeyboard : MonoBehaviour {

	public Image cursor;

	private Button topLeft;
	private Button topRight;
	private Button botLeft;
	private Button botRight;

	void Start() 
	{
		// set up cursor to be at topleft and move based on input.
			// move by setting sprite of one to nothing and setting sprite
			// where to go to the cursor
		topLeft = transform.Find("TopLeft").gameObject.GetComponent<Button>();
		topRight = transform.Find("TopRight").gameObject.GetComponent<Button>();
		botLeft = transform.Find("BotLeft").gameObject.GetComponent<Button>();
		botRight = transform.Find("BotRight").gameObject.GetComponent<Button>();
	}
}
