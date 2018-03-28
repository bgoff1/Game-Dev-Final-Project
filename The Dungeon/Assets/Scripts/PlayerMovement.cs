using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [Header("Set in Inspector")]
    public Camera playerCamera;
	public Camera battleCamera;
    public GameObject playerHUD;
    public GameObject playerDisplay;
    public GameObject enemyDisplay;
    
    private bool isMoving = false;
    private Direction moveDirection;
    private Vector3 endPos;

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
    private const float WAIT_INTERVAL = 0.15f;
    // what percent chance to encounter an enemy
        // chance out of 100 per move
    private const int ENCOUNTER_CHANCE = 5;

    void Start()
    {
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
    }

	void FixedUpdate () {

        if (!isMoving)
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
        }
    }

    private void startMovingPlayer()
    {
        isMoving = true;
        endPos = transform.position;
        switch (moveDirection)
        {
            case Direction.North:
                endPos.y += MOVE_DISTANCE;
                break;
            case Direction.South:
                endPos.y -= MOVE_DISTANCE;
                break;
            case Direction.East:
                endPos.x -= MOVE_DISTANCE;
                break;
            case Direction.West:
                endPos.x += MOVE_DISTANCE;
                break;
        }
        InvokeRepeating("movePlayer", 0.0f, MOVE_SPEED);
    }

    private void movePlayer()
    {
        Vector3 charPosition = transform.position;
        if (!Physics2D.OverlapBox(endPos, transform.localScale, 0f))
        {
            if (charPosition != endPos)
            {
                // if theres an encounter
                if (Random.Range(0f, 100f) < ENCOUNTER_CHANCE)
                {
                    // change the view to the battle view and enable
                        // the HUD
                    enterBattle();
                    
                }
                // otherwise, set where the player is going to move to,
                    // and start moving there
                else
                {
                    switch (moveDirection)
                    {
                        case Direction.North:
                            charPosition.y += MOVE_INCREMENT;
                            break;
                        case Direction.South:
                            charPosition.y -= MOVE_INCREMENT;
                            break;
                        case Direction.East:
                            charPosition.x -= MOVE_INCREMENT;
                            break;
                        case Direction.West:
                            charPosition.x += MOVE_INCREMENT;
                            break;
                    }
                    transform.position = charPosition;
                    if (charPosition == endPos)
                    {
                        CancelInvoke("movePlayer");
                        // sets isMoving to false
                        Invoke("canMoveAgain", WAIT_INTERVAL);
                    }
                }
            }
        }
        else
        {
            isMoving = false;
            print(Physics2D.OverlapBox(endPos, transform.localScale, 0f));
            CancelInvoke("movePlayer");
        }
    }

    private void canMoveAgain()
    {
        isMoving = false;
    }

    
    public void enterBattle()
	{
		playerCamera.gameObject.SetActive(false);
		battleCamera.gameObject.SetActive(true);
        playerHUD.SetActive(true);
        playerDisplay.SetActive(true);
        enemyDisplay.SetActive(true);
    }
}

enum Direction
{
    North,
    South,
    East,
    West
}
