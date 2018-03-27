using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    [Header("Set in Inspector")]
    public Camera playerCamera;
	public Camera battleCamera;
    /*public Sprite northSprite;
    public Sprite eastSprite;
    public Sprite southSprite;
    public Sprite westSprite;*/
    public float walkSpeed = 3f;

    private bool isAllowedToMove;
    private bool isMoving = false;
    private Direction currentDir;
    private float t;
    private Vector2 playerWidth;
    private Vector2 input;
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 characterEndPos;

    void Start()
    {
        isAllowedToMove = true;
        playerWidth = transform.localScale; //transform.GetComponent<BoxCollider2D>().size;
        //print(playerWidth);
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
    }

	void FixedUpdate () {

        if (!isMoving && isAllowedToMove)
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) { input.y = 0; }
            else { input.x = 0; }

            if (input != Vector2.zero)
            {
                
                StartCoroutine(Move(transform));
                //playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
            }
        }
	}

    // Move function from https://github.com/StormBurpee/Unity_Pokemon/blob/master/Assets/Scripts/Player/PlayerMovement.cs
    public IEnumerator Move(Transform entity)
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
    }
	
	public void EnterBattle()
	{
		playerCamera.gameObject.SetActive(false);
		battleCamera.gameObject.SetActive(true);
	}
}

enum Direction
{
    North,
    East,
    South,
    West
}
