﻿using System.Collections;
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

	static public bool isFighting;
	private Button topLeft;
	private Button topRight;
	private Button botLeft;
	private Button botRight;
	private Image tlS;
	private Image trS;
	private Image blS;
	private Image brS;
	private int buttonCount;
	private CursorSpot cs;
	private Sprite cursor;

	public GameObject winScreen;
	public GameObject loseScreen;

	void Awake() 
	{
		topLeft = transform.Find("TopButtons").Find("TopLeft").gameObject.GetComponent<Button>();
		topRight = transform.Find("TopButtons").transform.Find("TopRight").gameObject.GetComponent<Button>();
		botLeft = transform.Find("BottomButtons").transform.Find("BotLeft").gameObject.GetComponent<Button>();
		botRight = transform.Find("BottomButtons").transform.Find("BotRight").gameObject.GetComponent<Button>();
		cursor = (Sprite)Resources.Load("Images/redarrow", typeof(Sprite));
		tlS = getImage(topLeft);
		trS = getImage(topRight);
		blS = getImage(botLeft);
		brS = getImage(botRight);
		trS.gameObject.SetActive(false);
		blS.gameObject.SetActive(false);
		brS.gameObject.SetActive(false);
		cs = CursorSpot.TopLeft;
		isFighting = false;
		topLeft.transform.Find("Image").gameObject.GetComponent<Image>().sprite = cursor;
	}

	private Image getImage(Button b)
	{
		return b.transform.Find("Image").GetComponent<Image>();
	}

	private int getActiveButtonCount()
	{
		int result = 0;
		if (topLeft.gameObject.activeSelf)
			result++;
		if (topRight.gameObject.activeSelf)
			result++;
		if (botLeft.gameObject.activeSelf)
			result++;
		if (botRight.gameObject.activeSelf)
			result++;
		if (!botLeft.transform.parent.gameObject.activeSelf)
		{
			result -= 2;
		}
		return result;
	}

	private void resetCursorPosition()
	{
		if (cs != CursorSpot.TopLeft)
		{
			toggleCursor();
			cs = CursorSpot.TopLeft;
			toggleCursor();
		}
	}

	private void Update()
	{
		bool win = true;
		if (isFighting)
		{
			buttonCount = getActiveButtonCount();
			if (winScreen.activeSelf)
				win = false;
			if (loseScreen.activeSelf)
				win = false;
		}
		if (isFighting && buttonCount > 2 && win)
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
			else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
			{
				Combat.performButtonAction(getButtonFromCursorSpot());
				resetCursorPosition();
			}
		}
		else if (isFighting && buttonCount == 2 && win)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (cs == CursorSpot.TopRight)
				{
					toggleCursor();
					cs = CursorSpot.TopLeft;
					toggleCursor();
				}
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (cs == CursorSpot.TopLeft)
				{
					toggleCursor();
					cs = CursorSpot.TopRight;
					toggleCursor();
				}
			}
			else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
			{
				Combat.performFirstAction(getButtonFromCursorSpot());
			}
		}
		else if (win)
		{
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
			{
				Combat.BackToCave();
			}
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

	private Image getImageFromCursorSpot()
	{
		switch (cs)
		{
			case CursorSpot.BotLeft:
				return blS;
			case CursorSpot.BotRight:
				return brS;
			case CursorSpot.TopLeft:
				return tlS;
			case CursorSpot.TopRight:
				return trS;
		}
		return null;
	}

	private void toggleCursor() 
	{
		Image currentImage = getImageFromCursorSpot();
		if (currentImage.sprite == null)
		{
			currentImage.gameObject.SetActive(true);
			currentImage.sprite = cursor;
		}
		else {
			currentImage.gameObject.SetActive(false);
			currentImage.sprite = null;
		}
	}
}
