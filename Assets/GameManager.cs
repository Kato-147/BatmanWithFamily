using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //loc Viet
    public GameObject pacman;
    public bool gameIsRunning;
    public List<NodeController> nodeControllers = new List<NodeController>();
    public EnemyController redGhostController;
    public EnemyController pinkGhostController;
    public EnemyController blueGhostController;
    public EnemyController orangeGhostController;
    public bool newGame;
    public bool clearedLevel;
    public AudioSource startGameAudio;
    public int lives;
    public int currentLevel;
   


    public GameObject leftWarpNode;
    public GameObject rightWarpNode;


    public AudioSource siren;
    public AudioSource munch1;
    public AudioSource munch2;
    public int currentMunch = 0;

    public int score;
    public Text scoreText;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public GameObject redGhost;
    public GameObject blueGhost;
    public GameObject pinkGhost;
    public GameObject orangeGhost;

    public int totalPellets;
    public int pelletsLeft;
    public int pellectedCollectedOnThisLife;

    public bool hadDeathOnThisLevel=false;
    public enum GhostMode
    {
        chase, scatter
    }
    public GhostMode currentGhostMode;

    // Start is called before the first frame update
    void Awake()
    {
        //Loc Viet
        newGame = true;
        clearedLevel = false;
        redGhostController = redGhost.GetComponent<EnemyController>();
        pinkGhostController = pinkGhost.GetComponent<EnemyController>();
        blueGhostController = blueGhost.GetComponent<EnemyController>();
        orangeGhostController = orangeGhost.GetComponent<EnemyController>();


        ghostNodeStart.GetComponent<NodeController>().isGhostStartingNode = true;
        
        pacman = GameObject.Find("Player");

        //LocViet
        StartCoroutine(Setup());


    }
    //locViet
    public IEnumerator Setup()
    {
        //if pacman clear a level ,a background will appear covering the level and the game will pause for 0.1 seconds
        //nếu pacman vượt qua một cấp độ, nền sẽ xuất hiện bao trùm cấp độ đó và trò chơi sẽ tạm dừng trong 0,1 giây
        if (clearedLevel)
        {
            //Activate background:Kích hoạt nền
            yield return new WaitForSeconds(0.1f);
        }
        pellectedCollectedOnThisLife = 0;
        currentGhostMode = GhostMode.scatter;
        gameIsRunning = false;
        currentMunch = 0;
        siren.Play();
        float waitTimer = 1f;
        //Pellets will respawn when pacman clears the level or starts a new game
        //sẽ hồi sinh khi pacman vượt qua cấp độ hoặc bắt đầu trò chơi mới
        if (clearedLevel || newGame)
        {
            waitTimer = 4f;
            for (int i = 0; i < nodeControllers.Count; i++)
            {
                nodeControllers[i].RespawnPellet();
            }
        }
        if (newGame)
        {
            startGameAudio.Play();
            score = 0;
            scoreText.text = "Score:" + score.ToString();
            lives = 3;
            currentLevel = 1;
        }

        pacman.GetComponent<PlayerController>().Setup();
        redGhostController.Setup();
        pinkGhostController.Setup();
        blueGhostController.Setup();
        orangeGhostController.Setup();
        newGame = false;
        clearedLevel = false;
        yield return new WaitForSeconds(waitTimer);
        StartGame();
    }
    void StartGame()
    {
        gameIsRunning = true;
        siren.Play();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void GotPelletFromNodeController(NodeController nodeController)
    {
        nodeControllers.Add(nodeController);
        totalPellets++;
        pelletsLeft++;
    }

    public void AddToScore(int amount)
    {
        score += amount;
        scoreText.text = "Score : " + score.ToString();
    }

    public void CollectedPellet(NodeController nodeController)
    {
        if (currentMunch == 0)
        {
            munch1.Play();
            currentMunch = 1;
        }
        else if (currentMunch == 1)
        {
            munch2.Play();
            currentMunch = 0;
        }

        pelletsLeft--;
        pellectedCollectedOnThisLife++;

        int requiredBluePellets = 0;
        int requiredOrangePellets = 0;
        if (hadDeathOnThisLevel)
        {
            requiredBluePellets = 12;
            requiredOrangePellets = 32;
        }
        else 
        {
            requiredBluePellets = 30;
            requiredOrangePellets = 60;
        }
        if (pellectedCollectedOnThisLife >= requiredBluePellets && !blueGhost.GetComponent<EnemyController>().leftHomeBefore)
        {
            blueGhost.GetComponent<EnemyController>().readToLeaveHome = true;
        }
        if (pellectedCollectedOnThisLife >= requiredOrangePellets && !orangeGhost.GetComponent<EnemyController>().leftHomeBefore)
        {
            orangeGhost.GetComponent<EnemyController>().readToLeaveHome = true;
        }

        AddToScore(10);

        //Add to our score
        //Check if there are any pellet left 

        //Check how many pellet were eaten

        //Is this are power pellet
    }

}
