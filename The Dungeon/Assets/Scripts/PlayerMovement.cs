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

    void Start()
    {
        isAllowedToMove = true;
        playerWidth = transform.localScale; //transform.GetComponent<BoxCollider2D>().size;
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
                playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
                StartCoroutine(Move(transform));
            }
        }
	}

    // Move function from https://github.com/StormBurpee/Unity_Pokemon/blob/master/Assets/Scripts/Player/PlayerMovement.cs
    public IEnumerator Move(Transform entity)
    {
        isMoving = true;
        startPos = entity.position;
        t = 0;
        print(entity.position);
        endPos = new Vector2(startPos.x + System.Math.Sign(input.x), startPos.y + System.Math.Sign(input.y));

        if (!Physics2D.OverlapBox(endPos, playerWidth, 0))
        {
            print("not there");
            while (t < 1f)
            {
                t += Time.deltaTime * walkSpeed;
                entity.position = Vector2.Lerp(startPos, endPos, t);
                print(entity.position);
                yield return null;
            }
        }
        else
        {
            print(Physics2D.OverlapBoxAll(endPos, playerWidth, 0)[0].transform.position);
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
