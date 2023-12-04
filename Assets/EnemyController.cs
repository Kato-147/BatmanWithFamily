using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public enum GhostNodeStateEnum
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes
    }

    public GhostNodeStateEnum ghostNodeState;

    public enum GhostType
    {
        red,
        blue,
        pink,
        orange
    }

    public GhostType ghostType;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public MovementController movementController;

    public GameObject startingNode;

    public bool readyToLeaveHome = false;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if (ghostType == GhostType.red)
        {
            ghostNodeState = GhostNodeStateEnum.startNode;
            startingNode = ghostNodeStart;
        }
        else if (ghostType == GhostType.pink)
        {
            ghostNodeState = GhostNodeStateEnum.centerNode;
            startingNode = ghostNodeCenter;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodeState = GhostNodeStateEnum.leftNode;
            startingNode = ghostNodeLeft;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodeState = GhostNodeStateEnum.rightNode;
            startingNode = ghostNodeRight;
        }
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReachedCenterOfNode(NodeController nodeController)
    {
        if(ghostNodeState == GhostNodeStateEnum.movingInNodes)
        {
            //Determine next game node to go to
            if(ghostType == GhostType.red)
            {
                DetermineRedGhostDirection();
            }
        }
        else if (ghostNodeState == GhostNodeStateEnum.respawning)
        {
            //Determine quickest direction to home
        }
        else
        {
            // If we are ready to leave our home
            if(readyToLeaveHome)
            {
                //If we are in the left home node, move to the center
                if(ghostNodeState == GhostNodeStateEnum.leftNode)
                {
                    ghostNodeState = GhostNodeStateEnum.centerNode;
                    movementController.SetDirection("right");
                }
                //If we are in the right home node, move to the center
                else if (ghostNodeState == GhostNodeStateEnum.rightNode)
                {
                    ghostNodeState = GhostNodeStateEnum.centerNode;
                    movementController.SetDirection("left");
                }
                //If we are in the center node, move to the start node
                else if (ghostNodeState == GhostNodeStateEnum.centerNode)
                {
                    ghostNodeState = GhostNodeStateEnum.startNode;
                    movementController.SetDirection("up");
                }
                //If we are in the start node, start moving around in the game
                else if(ghostNodeState == GhostNodeStateEnum.startNode)
                {
                    ghostNodeState = GhostNodeStateEnum.movingInNodes;
                    movementController.SetDirection("left");
                }
            }
        }
    }

    void DetermineRedGhostDirection()
    {
        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }

    void DeterminePinkGhostDirection()
    {

    }

    void DetermineBlueGhostDirection()
    {

    }

    void DetermineOrangeGhostDirection()
    {

    }

    string GetClosestDirection(Vector2 target)
    {
        float shortestDistance = 0;
        string lastMovingDirection = movementController.lastMovingDirection;
        string newDirection = "";

        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>(); 

        //If we can move up and we aren't reversing
        if(nodeController.canMoveUp && lastMovingDirection != "down")
        {
            //Get the node above us 
            GameObject nodeUP = nodeController.nodeUp;
            //Get the distance between our top mode, and pacman
            float distance = Vector2.Distance(nodeUP.transform.position, target);

            //If this is the shortest distance so far, set our direction
            if(distance <shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "up";    
            }
        }


        if (nodeController.canMoveDown && lastMovingDirection != "up")
        {
            //Get the node above us 
            GameObject nodeDown = nodeController.nodeDown;
            //Get the distance between our top mode, and pacman
            float distance = Vector2.Distance(nodeDown.transform.position, target);

            //If this is the shortest distance so far, set our direction
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "down";
            }
        }

        if (nodeController.canMoveLeft && lastMovingDirection != "right")
        {
            //Get the node above us 
            GameObject nodeLeft = nodeController.nodeLeft;
            //Get the distance between our top mode, and pacman
            float distance = Vector2.Distance(nodeLeft.transform.position, target);

            //If this is the shortest distance so far, set our direction
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "left";
            }
        }

        if (nodeController.canMoveRight && lastMovingDirection != "left")
        {
            //Get the node above us 
            GameObject nodeRight = nodeController.nodeRight;
            //Get the distance between our top mode, and pacman
            float distance = Vector2.Distance(nodeRight.transform.position, target);

            //If this is the shortest distance so far, set our direction
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "right";
            }
        }

        return newDirection;
    }
}
