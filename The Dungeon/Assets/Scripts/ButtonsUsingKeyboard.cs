using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum CursorSpot {
	TopLeft,
	TopRight,
	BotLeft,
	BotRight
}

public class ButtonsUsingKeyboard : MonoBehaviour {


	private Button topLeft;
	private Button topRight;
	private Button botLeft;
	private Button botRight;
	private CursorSpot cs;
	private Sprite cursor;

	void Start() 
	{
		topLeft = transform.Find("TopButtons").Find("TopLeft").gameObject.GetComponent<Button>();
		topRight = transform.Find("TopButtons").transform.Find("TopRight").gameObject.GetComponent<Button>();
		botLeft = transform.Find("BottomButtons").transform.Find("BotLeft").gameObject.GetComponent<Button>();
		botRight = transform.Find("BottomButtons").transform.Find("BotRight").gameObject.GetComponent<Button>();
		cursor = (Sprite)Resources.Load("Images/redarrow", typeof(Sprite));
		cs = CursorSpot.TopLeft;
		topLeft.transform.Find("Image").gameObject.GetComponent<Image>().sprite = cursor;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			switch (cs) {
				case CursorSpot.BotLeft:
					toggleCursor();
					cs = CursorSpot.TopLeft;
					toggleCursor();
					break;
				case CursorSpot.BotRight:
					toggleCursor();
					cs = CursorSpot.TopRight;
					toggleCursor();
					break;
				default:
					break;
			}
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			switch (cs) {
				case CursorSpot.TopLeft:
					toggleCursor();
					cs = CursorSpot.BotLeft;
					toggleCursor();
					break;
				case CursorSpot.TopRight:
					toggleCursor();
					cs = CursorSpot.BotRight;
					toggleCursor();
					break;
				default:
					break;
			}
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			switch (cs) {
				case CursorSpot.BotRight:
					toggleCursor();
					cs = CursorSpot.BotLeft;
					toggleCursor();
					break;
				case CursorSpot.TopRight:
					toggleCursor();
					cs = CursorSpot.TopLeft;
					toggleCursor();
					break;
				default:
					break;
			}
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			switch (cs)
			{
				case CursorSpot.BotLeft:
					toggleCursor();
					cs = CursorSpot.BotRight;
					toggleCursor();
					break;
				case CursorSpot.TopLeft:
					toggleCursor();
					cs = CursorSpot.TopRight;
					toggleCursor();
					break;
				default:
					break;
			}
		}
		else if (Input.GetKeyDown(KeyCode.Return))
		{
			Combat.performButtonAction(getButtonFromCursorSpot());
		}
	}

	private Button getButtonFromCursorSpot()
	{
		switch (cs)
		{
			case CursorSpot.BotLeft:
				return botLeft;
			case CursorSpot.BotRight:
				return botRight;
			case CursorSpot.TopLeft:
				return topLeft;
			case CursorSpot.TopRight:
				return topRight;
		}
		return null;
	}

	private void toggleCursor() 
	{
		Image currentImage = getButtonFromCursorSpot().transform.Find("Image").gameObject.GetComponent<Image>();
		if (currentImage.sprite == null)
		{
			currentImage.sprite = cursor;
		}
		else {
			currentImage.sprite = null;
		}
	}
}
