using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpLadderFloor1Collider : MonoBehaviour {

	public Vector3 teleportLocation;

	private Collider2D coll;

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
			coll = other;
			other.gameObject.SendMessage("stopMoving");
			Invoke("teleport", 0.1f);
		}
	}

	void teleport()
	{
		coll.gameObject.transform.position = teleportLocation;
	}
}
