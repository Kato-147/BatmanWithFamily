using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    public enum GhostNodeStatesEnum
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes
    }

    public GhostNodeStatesEnum ghostNodeState;
    public GhostNodeStatesEnum startGhostNodeState;
    public GhostNodeStatesEnum respawnState;

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

    public bool readToLeaveHome = false;
    //loc viet
    public GameManager gameManager;
    

    // Vy viet
    public bool testRespawm = false;

    public bool isFrightened = false;

    public GameObject[] scatterNodes;
    public int scatterNodeIndex;

    public bool leftHomeBefore = false;

    // Start is called before the first frame update
    void Awake()
    {
       
        //locViet
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        movementController = GetComponent<MovementController>();
        if (ghostType == GhostType.red)
        {
            //Loc Sua
            startGhostNodeState = GhostNodeStatesEnum.startNode;
            respawnState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeStart;
          
        }
        else if (ghostType == GhostType.pink)
        {
            //Loc Sua
            startGhostNodeState = GhostNodeStatesEnum.centerNode;
            respawnState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
        }
        else if (ghostType == GhostType.blue)
        {
            //Loc Sua
            startGhostNodeState = GhostNodeStatesEnum.leftNode;
            respawnState = GhostNodeStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
        }
        else if (ghostType == GhostType.orange)
        {
            //Loc Sua
            startGhostNodeState = GhostNodeStatesEnum.rightNode;
            respawnState = GhostNodeStatesEnum.rightNode;
            startingNode = ghostNodeRight;
        }
        movementController.currentNode = startingNode;
        //loc viet
        transform.position = startingNode.transform.position;
        
    }
    //loc Viet
    public void Setup()
    {
        ghostNodeState = startGhostNodeState;
        //resset lại hồn ma trở về vị trí ban đầu
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;
        //đặt chỉ số phân tán chúng
        scatterNodeIndex = 0;
        //set isFrightened:sợ hãi
        isFrightened = false;
        //set readyToLeaveHome to be false if they are blue and pink
        if (ghostType == GhostType.red)
        {
            readToLeaveHome = true;
            leftHomeBefore = true;
        }
        else if (ghostType == GhostType.pink)
        {
            readToLeaveHome = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //locViet
        if (!gameManager.gameIsRunning)
        {
            return;
        }

        if (testRespawm == true)
        {
            readToLeaveHome = false;
            ghostNodeState = GhostNodeStatesEnum.respawning;
            testRespawm = false;
        }
        if (movementController.currentNode.GetComponent<NodeController>().isSideNode)
        {
            movementController.SetSpeed(1);
        }
        else
        {
            movementController.SetSpeed(3);
        }
    }

    public void ReachedCenterOfNode(NodeController nodeController)
    {
        if (ghostNodeState == GhostNodeStatesEnum.movingInNodes)
        {
            leftHomeBefore = true;
            // Scatter mode
            if(gameManager.currentGhostMode == GameManager.GhostMode.scatter)
            {

                DetermineGhostScatterModeDirection();

            }
            // Fright mode
            else if (isFrightened)
            {
                string direction= GetRandomDirection();
                movementController.SetDirection(direction);
            }
            // chase mode
            else
            {
                //Determine next game node to go to
                if (ghostType == GhostType.red)
                {
                    DetermineRedGhostDirection();
                }
                else if(ghostType == GhostType.pink){
                    DeterminePinkGhostDirection();
                }
                else if(ghostType == GhostType.blue)
                {
                    DetermineBlueGhostDirection();
                }
                else if (ghostType == GhostType.orange)
                {
                    DetermineOrangeGhostDirection();
                }
            }

        }
        else if (ghostNodeState == GhostNodeStatesEnum.respawning)
        {
            string direction = "";

            // we have reached our start node, move to the center node
            if (transform.position.x == ghostNodeStart.transform.position.x && transform.position.y == ghostNodeStart.transform.position.y)
            {
                direction = "down";
            }

            // we have reached our start node,either finish respawn , or move to the left/right node
            else if (transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
                if(respawnState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeState = respawnState;
                }
                else if(respawnState == GhostNodeStatesEnum.leftNode)
                {
                    direction = "left";
                }
                else if(respawnState == GhostNodeStatesEnum.rightNode)
                {
                    direction = "right";
                }
            }
            // if our respawn sate is either the left or right node, and we got to that node , leave home again
            else if(
                (transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y)
                || (transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y)
                )
            {
                ghostNodeState = respawnState;
            }
            // we are in the gamebroad still, locate our start node
            else
            {
                //Determine quickest direction to home
                direction = GetClosestDirection(ghostNodeStart.transform.position);
            }


          
            movementController.SetDirection(direction);

        }
        else
        {
            // If we are ready to leave our home
            if (readToLeaveHome)
            {
                //If we are in the left home node, move to the center
                if (ghostNodeState == GhostNodeStatesEnum.leftNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("right");
                }
                // If we are tn the right home node, move to the center
                else if (ghostNodeState == GhostNodeStatesEnum.rightNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("left");
                }
                //If we are in the center node, move to the start node
                else if (ghostNodeState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.startNode;
                    movementController.SetDirection("up");
                }
                // If we are in the start node, start moving around in the game
                else if (ghostNodeState == GhostNodeStatesEnum.startNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.movingInNodes;
                    movementController.SetDirection("left");
                }
            }
        }
    }

    string GetRandomDirection()
    {
        List<string> possibleDirections=new List<string>();
        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        if (nodeController.canMoveDown && movementController.lastMovingDirection != "up")
        {
            possibleDirections.Add("down");
        }
        if (nodeController.canMoveDown && movementController.lastMovingDirection != "down")
        {
            possibleDirections.Add("up");
        }
        if (nodeController.canMoveDown && movementController.lastMovingDirection != "left")
        {
            possibleDirections.Add("right");
        }
        if (nodeController.canMoveDown && movementController.lastMovingDirection != "right")
        {
            possibleDirections.Add("left");
        }

        string direction = "";
        int randomDirectionIndex= Random.Range(0, possibleDirections.Count-1);
        direction = possibleDirections[randomDirectionIndex];
        return direction;
    }

    void DetermineGhostScatterModeDirection()
    {
        if (transform.position.x == scatterNodes[scatterNodeIndex].transform.position.x && transform.position.y == scatterNodes[scatterNodeIndex].transform.position.y)
        {
            scatterNodeIndex++;

            if (scatterNodeIndex == scatterNodes.Length - 1)
            {
                scatterNodeIndex = 0;
            }
        }

        string direction = GetClosestDirection(scatterNodes[scatterNodeIndex].transform.position);
        movementController.SetDirection(direction);
    }
    //loc Viet
    void DetermineRedGhostDirection()
    {
        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }
    void DeterminePinkGhostDirection() {
        string pacmansDirection = gameManager.pacman.GetComponent<MovementController>().lastMovingDirection;
        float distanceBetweenNodes = 0.35f;

        Vector2 target = gameManager.pacman.transform.position;
        if(pacmansDirection == "left")
        {
            target.x -= distanceBetweenNodes * 2;
        }
        else if(pacmansDirection == "right")
        {
            target.x += distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "up")
        {
            target.y += distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "down")
        {
            target.y -= distanceBetweenNodes * 2;
        }

        string direction = GetClosestDirection(target);
        movementController.SetDirection(direction);

    }
    void DetermineBlueGhostDirection() {
        string pacmansDirection = gameManager.pacman.GetComponent<MovementController>().lastMovingDirection;
        float distanceBetweenNodes = 0.35f;

        Vector2 target = gameManager.pacman.transform.position;
        if (pacmansDirection == "left")
        {
            target.x -= distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "right")
        {
            target.x += distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "up")
        {
            target.y += distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "down")
        {
            target.y -= distanceBetweenNodes * 2;
        }

        GameObject redGhost = gameManager.redGhost;
        float xDistace = target.x - redGhost.transform.position.x;
        float yDistace = target.y - redGhost.transform.position.y;

        Vector2 blueTager = new Vector2(target.x + xDistace, target.y + yDistace);
        string direction = GetClosestDirection(blueTager);
        movementController.SetDirection(direction);
    }
    void DetermineOrangeGhostDirection() 
    {
        float distance = Vector2.Distance(gameManager.pacman.transform.position, transform.position);
        float distanceBetweenNodes = 0.35f;
        if(distance < 0)
        {
            distance *= -1;
        }

        if(distance <= distanceBetweenNodes * 8)
        {
            DetermineRedGhostDirection();     
        }
        else
        {
            DetermineGhostScatterModeDirection();
        }
    }
    string GetClosestDirection(Vector2 target)
    {
        float shortesDistance = 0;
        string lastMovingDirection = movementController.lastMovingDirection;
        string newDirection = "";
        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();
        //if we can move up and we aren't reversing
        if (nodeController.canMoveUp && lastMovingDirection != "down")
        {
            GameObject nodeUp = nodeController.nodeUp;
            //get the distance between our top node,and pacman
            float distance = Vector2.Distance(nodeUp.transform.position, target);
            //if this is the shortest distance so far,set our direction
            if(distance < shortesDistance || shortesDistance == 0)
            {
                shortesDistance = distance;
                newDirection = "up";
            }
        }

        if (nodeController.canMoveDown && lastMovingDirection != "up")
        {
            GameObject nodeDown = nodeController.nodeDown;
            //get the distance between our top node,and pacman
            float distance = Vector2.Distance(nodeDown.transform.position, target);
            //if this is the shortest distance so far,set our direction
            if (distance < shortesDistance || shortesDistance == 0)
            {
                shortesDistance = distance;
                newDirection = "down";
            }
        }

        if (nodeController.canMoveLeft && lastMovingDirection != "right")
        {
            GameObject nodeLeft = nodeController.nodeLeft;
            //get the distance between our top node,and pacman
            float distance = Vector2.Distance(nodeLeft.transform.position, target);
            //if this is the shortest distance so far,set our direction
            if (distance < shortesDistance || shortesDistance == 0)
            {
                shortesDistance = distance;
                newDirection = "left";
            }
        }
        if (nodeController.canMoveRight && lastMovingDirection != "left")
        {
            GameObject nodeRight = nodeController.nodeRight;
            //get the distance between our top node,and pacman
            float distance = Vector2.Distance(nodeRight.transform.position, target);
            //if this is the shortest distance so far,set our direction
            if (distance < shortesDistance || shortesDistance == 0)
            {
                shortesDistance = distance;
                newDirection = "right";
            }
        }
        return newDirection;
    }
    
}

