using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [Header("Set in Inspector")]
    public Camera playerCamera;
	public Camera battleCamera;
    //public float walkSpeed = 3f;
    
    private bool isMoving = false;
    private Direction moveDirection;
    //private Vector2 playerWidth;
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
        //playerWidth = transform.localScale; //transform.GetComponent<BoxCollider2D>().size;
        //print(playerWidth);
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
            #region oldmovement
            /*input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) { input.y = 0; }
            else { input.x = 0; }

            if (input != Vector2.zero)
            {
                StartCoroutine(Move(transform));
                //playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
            }*/
            #endregion
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
                    //logic for starting battle scene
                    enterBattle();
                    // is there more logic or is that it?
                    // i think that's it. just need to add to 
                    //  move back after defeating an enemy
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
                        //isMoving = false;
                        CancelInvoke("movePlayer");
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

    // Move function from https://github.com/StormBurpee/Unity_Pokemon/blob/master/Assets/Scripts/Player/PlayerMovement.cs
    /*public IEnumerator Move(Transform entity)
    {
        isMoving = true;
        startPos = entity.position;
        t = 0;
        float addY = 0, addX = 0;
        if (input.x != 0)
        {
            if (System.Math.Sign(input.x) == 1)
                addX = 0.1f;
            else if (System.Math.Sign(input.x) == -1)
                addX = -0.1f;
        }
        else if(input.y != 0)
        {
            if (System.Math.Sign(input.y) == 1)
                addY = 0.1f;
            else if (System.Math.Sign(input.y) == -1)
                addY = -0.1f;
        }
         
       // print(entity.position);
        endPos = new Vector2(startPos.x + addX, startPos.y + addY);
        characterEndPos = new Vector2(startPos.x + System.Math.Sign(input.x), startPos.y + System.Math.Sign(input.y));
        if (!Physics2D.OverlapBox(endPos, playerWidth, 0))
        {
            print("not there");
            while (t < 0.25f)
            {
                t += Time.deltaTime * walkSpeed;
                entity.position = Vector2.Lerp(startPos, characterEndPos, t);
                //print(entity.position);
                yield return null;
            }
        }
        else
        {
           // print(Physics2D.OverlapBoxAll(endPos, playerWidth, 0)[0].transform.position);
            print("is there");
        }
        isMoving = false;
        yield return 0;
    }*/

    public void enterBattle()
	{
		playerCamera.gameObject.SetActive(false);
		battleCamera.gameObject.SetActive(true);
	}
}

enum Direction
{
    North,
    South,
    East,
    West
}
