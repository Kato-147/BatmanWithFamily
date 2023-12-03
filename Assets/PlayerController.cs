using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    MovementController movementController;

    public SpriteRenderer sprite;
    public Animator    animator;
    //locViet
    public GameObject startNode;
    public Vector2 startPos;
    public GameManager gameManager;

    
    void Awake()
    {
        //LocViet
        gameManager =GameObject.Find("GameManager").GetComponent<GameManager>();
        startPos = new Vector2(0.77f, 2.97f);



        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        movementController = GetComponent<MovementController>();
        movementController.lastMovingDirection = "left";

        //locViet
        startNode = movementController.currentNode;

    }
    //loc Viet
    public void Setup()
    {
        movementController.currentNode = startNode;
        movementController.lastMovingDirection = "left";
        transform.position = startPos;
        animator.SetBool("moving", false);
    }

    // Update is called once per frame
    void Update()
    {
        //Loc Viet
        if (!gameManager.gameIsRunning)
        {
            return;
        }

        animator.SetBool("moving", true);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementController.SetDirection("left");
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementController.SetDirection("right");
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementController.SetDirection("up");
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementController.SetDirection("down");
        }

        bool flipX = false;
        bool flipY = false;
        if(movementController.lastMovingDirection == "left")
        {
            animator.SetInteger("direction", 0);
          
        }
        else if (movementController.lastMovingDirection == "right")
        {
            animator.SetInteger("direction", 0);
            flipX = true;
        }else if (movementController.lastMovingDirection == "up")
        {
            animator.SetInteger("direction", 1);

        }else if (movementController.lastMovingDirection == "down")
        {
            animator.SetInteger("direction", 1);
            flipY = true;
        }

        sprite.flipY = flipY;
        sprite.flipX = flipX;   
    }
}
