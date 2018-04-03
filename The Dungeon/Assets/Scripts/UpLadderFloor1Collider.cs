using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpLadderFloor1Collider : MonoBehaviour {

	public Vector3 teleportLocation;

	void Start()
	{
		switch (gameObject.name)
		{
			case "UpLadderCave1":
				teleportLocation = GameObject.Find("DownLadderCave2").transform.position;
				break;
			case "DownLadderCave2":
				teleportLocation = GameObject.Find("UpLadderCave1").transform.position;
				break;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player") {
			print("hi " + other.name);
			other.gameObject.transform.position = teleportLocation;
			other.SendMessage("stopMoving");
		}
	}
}
