using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour {

    [Header("Set in Inspector")]
    public Camera caveCamera;
	public Camera battleCamera;
    public GameObject playerHUD;
    public GameObject playerDisplay;
    public GameObject enemyDisplay;
    public GameObject preBattleScreen;

	public GameObject fightButton;
	public GameObject runButton;
	private bool canMove = true;
	public GameObject adventureButton;
    private bool isMoving = false;
    private Direction moveDirection;
    private Vector3 endPos;
	private Vector3 midPos;
	private Vector3 charPosition;

	private bool alreadyRanAway = false;
	private int runAwayChance = 25;

    // how far the player goes on each movement
    private const float MOVE_DISTANCE = 0.5f;
    // how fast the player is moved from one block to the next
        // i.e. how fast the movePlayer function is called
    private const float MOVE_SPEED = 0.1f;
    // how much the player is moved in each call of movePlayer
    private const float MOVE_INCREMENT = 0.25f;
    // how much time is waited before isMoving boolean is set
        // back to false after moving. Makes moving rapidly less
        // chaotic
	private const float WAIT_INTERVAL = 0.15f;//0f;
    // what percent chance to encounter an enemy
        // chance out of 100 per move
    private const int ENCOUNTER_CHANCE = 5;

	private BoxCollider2D playerCollider;
    void Start()
    {
        caveCamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
		playerCollider = GetComponent<BoxCollider2D>();
	}

	void FixedUpdate () {

		if (!isMoving && caveCamera.isActiveAndEnabled && canMove)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                moveDirection = Direction.North;
                startMovingPlayer();
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                moveDirection = Direction.South;
                startMovingPlayer();
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                moveDirection = Direction.East;
                startMovingPlayer();
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                moveDirection = Direction.West;
                startMovingPlayer();
            }
			else if (Input.GetKey(KeyCode.F))
			{
				enterBattle();
			}
            else if (Input.GetKey(KeyCode.E))
            {
                transform.position = new Vector3((float)-25.05, (float)-1.21, 0);
            }

			if((transform.position.x <= 7.5 && transform.position.x >= 7 && transform.position.y <= 10.5 && transform.position.y >= 10) || (transform.position.x <= -24.5 && transform.position.x >= -25.25 && transform.position.y <= -1.5 && transform.position.y >= -2.25))
			{
				playerCollider.enabled = true;
			}
			else
			{
				playerCollider.enabled =false;
			}

			
        }
    }

    private void startMovingPlayer()
    {
        isMoving = true;
        endPos = transform.position;
		midPos = transform.position;
        switch (moveDirection)
        {
			case Direction.North:
				endPos.y += MOVE_DISTANCE;
				midPos.y += (MOVE_DISTANCE / 2);
                break;
			case Direction.South:
				endPos.y -= MOVE_DISTANCE;
				midPos.y -= (MOVE_DISTANCE / 2);
                break;
			case Direction.East:
				endPos.x -= MOVE_DISTANCE;
				midPos.x -= (MOVE_DISTANCE / 2);
                break;
			case Direction.West:
				endPos.x += MOVE_DISTANCE;
				midPos.x += (MOVE_DISTANCE / 2);
                break;
        }
        InvokeRepeating("movePlayer", 0.0f, MOVE_SPEED);
    }

    private void movePlayer()
    {
		Collider2D collend, collmid;
		collend = Physics2D.OverlapBox(endPos, transform.localScale, 0f);
		collmid = Physics2D.OverlapBox(midPos, transform.localScale, 0f);
		charPosition = transform.position;
		if ((collend == null && collmid == null) || (collmid != null && collmid.name == "Player")
			|| (collend != null && collend.CompareTag("Ladder")))
        {
			characterMovement(endPos, MOVE_INCREMENT);
        }
		else if ((collmid == null && collend != null) || collmid.CompareTag("Ladder"))
		{
			//print(collmid.name);
			if (collend != null) { print(collend.name); }
			characterMovement(midPos, MOVE_INCREMENT);
		}
        else
        {
			if (Physics2D.OverlapBox(endPos, transform.localScale, 0f))
			{
				print("endPos");
				print(Physics2D.OverlapBox(endPos, transform.localScale, 0f));
			}
			else if (Physics2D.OverlapBox(midPos, transform.localScale, 0f))
			{
				print("midPos");
				print(Physics2D.OverlapBox(midPos, transform.localScale, 0f));
			}
            isMoving = false;
            CancelInvoke("movePlayer");
        }
    }

	private void characterMovement(Vector3 positionToMoveTo, float moveIncrement)
	{
		if (charPosition != positionToMoveTo)
		{
			// if theres an encounter
			if (Random.Range(0f, 100f) < ENCOUNTER_CHANCE)
			{
				// change the view to the battle view and enable
				// the HUD
				//enterBattle();
				preBattleScreen.SetActive(true);
				
				canMove = false;
			}
			// otherwise, set where the player is going to move to,
			// and start moving there
			else
			{
				switch (moveDirection)
				{
					case Direction.North:
						charPosition.y += moveIncrement;
						break;
					case Direction.South:
						charPosition.y -= moveIncrement;
						break;
					case Direction.East:
						charPosition.x -= moveIncrement;
						break;
					case Direction.West:
						charPosition.x += moveIncrement;
						break;
				}
				transform.position = charPosition;
				if (charPosition == positionToMoveTo)
				{
					CancelInvoke("movePlayer");
					// sets isMoving to false
					Invoke("canMoveAgain", WAIT_INTERVAL);
				}
			}
		}
		else {
			CancelInvoke("movePlayer");
			Invoke("canMoveAgain", WAIT_INTERVAL);
		}
	}

	public void stopMoving()
	{
		CancelInvoke("movePlayer");
		isMoving = false;
	}

    private void canMoveAgain()
    {
        isMoving = false;
    }


    public void fightButtonAction(){
		GameObject.Find("preBattleText").GetComponent<Text>().text = "YOU ENCOUNTERED AN ENEMY!";
		Vector3 pos = fightButton.transform.position;
		pos.x = 300;
		fightButton.transform.position = pos;
		runButton.SetActive(true);
		preBattleScreen.SetActive(false);
		enterBattle();
		canMove = true;
	}

	public bool runAway()
    {
        if (Random.Range(0, 100) < runAwayChance && !alreadyRanAway) 
        {
            runAwayChance -= 5;
            return true;
        }
        else
        {
            
            alreadyRanAway = true;
            return false;

        }
    }
	public void runButtonAction(){
		bool success = runAway();
		
		if(success)
		{
			GameObject.Find("preBattleText").GetComponent<Text>().text = "YOU RAN AWAY SUCCESSFULLY!";
			fightButton.SetActive(false);
			runButton.SetActive(false);
			adventureButton.SetActive(true);
		}
		else{
			GameObject.Find("preBattleText").GetComponent<Text>().text = "YOU CAN'T GET AWAY! YOU WILL HAVE TO FIGHT.";
			Vector3 pos = fightButton.transform.position;
			pos.x = 500;
			fightButton.transform.position = pos;
			
			adventureButton.SetActive(false);
			runButton.SetActive(false);
		}
	}

	public void backToAdventureButtonAction(){
		adventureButton.SetActive(false);
		fightButton.SetActive(true);
		runButton.SetActive(true);
		GameObject.Find("preBattleText").GetComponent<Text>().text = "YOU ENCOUNTERED AN ENEMY!";
		preBattleScreen.SetActive(false);
		canMove = true;
	}
    public void enterBattle()
	{
		caveCamera.gameObject.SetActive(false);
		battleCamera.gameObject.SetActive(true);
        playerHUD.SetActive(true);
        playerDisplay.SetActive(true);
        enemyDisplay.SetActive(true);
		Combat.spawnEnemy();
    }
}

enum Direction
{
    North,
    South,
    East,
    West
}
