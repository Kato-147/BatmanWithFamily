using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameManager gameManager;


    public GameObject currentNode;
    public float speed = 4f;

    public string direction = "";
    public string lastMovingDirection = "";

    public bool canWarp = true;

    public bool isGhost = false;


    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        NodeController currentNodeController = currentNode.GetComponent<NodeController>();

        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

        bool reverseDirection = false;
        if (
            (direction == "left" && lastMovingDirection == "right")
            || (direction == "right" && lastMovingDirection == "left")
            || (direction == "up" && lastMovingDirection == "down")
            || (direction == "down" && lastMovingDirection == "up")
            )
        {
            reverseDirection = true;
        }

        if (transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y)
        {
            if (isGhost)
            {
                GetComponent<EnemyController>().ReachedCenterOfNode(currentNodeController);
            }
            if (currentNodeController.isWarpLeftNode && canWarp)
            {
                currentNode = gameManager.rightWarpNode;
                direction = "left";
                lastMovingDirection = "left";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            else if (currentNodeController.isWarpRightNode && canWarp)
            {
                currentNode = gameManager.leftWarpNode;
                direction = "right";
                lastMovingDirection = "right";
                transform.position = currentNode.transform.position;
                canWarp = false;
            }
            else
            {
                // if we
                if (currentNodeController.isGhostStartingNode && direction == "down"
                    && (!isGhost || GetComponent<EnemyController>().ghostNodeState != EnemyController.GhostNodeStatesEnum.respawning)) 
                {
                    direction = lastMovingDirection;
                }
                GameObject newNode = currentNodeController.GetNodeFromDirection(direction);
                if (newNode != null)
                {
                    currentNode = newNode;
                    lastMovingDirection = direction;
                }

                else
                {
                    direction = lastMovingDirection;
                    newNode = currentNodeController.GetNodeFromDirection(direction);
                    if (newNode != null)
                    {
                        currentNode = newNode;
                    }
                }
            }

        }
        else
        {
            canWarp = true;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void SetDirection(string newDirection)
    {
        direction = newDirection;
    }
}
