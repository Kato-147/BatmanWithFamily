using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject leftWarpNode;
    public GameObject rightWarpNode;

    public AudioSource siren;
    public AudioSource munch1;
    public AudioSource munch2;
    public int currentMunch = 0;

    public int score;
    public TextMeshProUGUI scoreText;

<<<<<<< Updated upstream
    // Start is called before the first frame update
    void Awake()
    {
=======
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

    public bool gameIsRunning;

    public enum GhostMode
    {
        chase, scatter
    }
    public GhostMode currentGhostMode;

    // Start is called before the first frame update
    void Awake()
    {
        gameIsRunning = true;
        pinkGhost.GetComponent<EnemyController>().readToLeaveHome = true;
        currentGhostMode = GhostMode.chase;
        ghostNodeStart.GetComponent<NodeController>().isGhostStartingNode = true;
        pacman = GameObject.Find("Player");
>>>>>>> Stashed changes
        score = 0;
        currentMunch = 0;
        siren.Play();
    }

    // Update is called once per frame
    void Update()
    {

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

        AddToScore(10);

        //Add to our score
        //Check if there are any pellet left 

        //Check how many pellet were eaten

        //Is this are power pellet
    }

}
