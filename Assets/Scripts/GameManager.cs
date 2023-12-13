using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
/***************************************************
 * 
 * 
 * 
 * 
 * ************************************************/

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreboardText;
    [SerializeField] TextMeshProUGUI timeRemainingText;
    [SerializeField] GameObject toggleGroup;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject spawnManager;
    [SerializeField] Animator playerAnimator;
    [SerializeField] ParticleSystem dirtSplatter;
    public static bool gameOver = true;
    public static float score;
    private AudioSource audioSource;
    private int timeRemaining = 60;
    private int miniGameCooldown = 5;
    public static bool timedGame = false;
    private UIController uiControllerScript;
    private QuizController quizControllerScript;
    public static bool gameInProgress;
    private static GameObject lastObject;
    private static GameManager instance;
    public static float speed = 30f;

    [Header("Debug Tags")]
    public bool DebugToggle;


    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        BackToGame();
        DisplayUI();
        EndGame();
    }

    private void DisplayUI()
    {
        scoreboardText.text = "Score:" + Mathf.RoundToInt(score).ToString();

        if(timedGame && !gameOver)
        {
            if(timeRemaining > 0)
            {
                
                timeRemainingText.text = timeRemaining.ToString();
            }
           else
           {
               timeRemainingText.text = "Game\nOver";
           }
        }
    }

    // This is called when the player presses the start button
    public void StartGame()
    {
        audioSource.Play();
        
        
        gameInProgress = true;
        toggleGroup.SetActive(false);
        startButton.SetActive(false);
        
        if(timedGame)
        {            
            timeRemainingText.gameObject.SetActive(true);
            InvokeRepeating("TimeCountdown", 1,1);
        }

        gameOver = false;

        spawnManager.SetActive(true);

        playerAnimator.SetFloat("Speed_f", 1.0f);
        playerAnimator.SetBool("BeginGame_b", true);
        dirtSplatter.Play();
    }

    public void SetTimed(bool timed)
    {
        timedGame = timed;
    }

    // ???
    public static void ChangeScore(float change)
    {
        score += change;
    }

    // This is called from the TimeCountdown Invoke Repeating
    private void StartKahootGame()
    {
        // Stops all movement and spawning in main scene
        QuizController.backToGame = false;
        gameInProgress = false;
        speed = 0;
        playerAnimator.SetBool("BeginGame_b", false);
        playerAnimator.SetFloat("Speed_f", 0f);
        audioSource.Stop();
        dirtSplatter.Stop();
        CancelInvoke();

        // Opens Kahoos scene as second scene
        SceneManager.LoadScene("KahootScene", LoadSceneMode.Additive);        
    }

    public void BackToGame()
    {
        if (QuizController.backToGame)
        {
            // Toggles Back-to-Game off and restarts timer
            QuizController.backToGame = false;
            miniGameCooldown = 10;

            // Delete any existing Obstacles in scene
            Obstacle[] obstaclesObjects = FindObjectsOfType<Obstacle>();
            foreach (Obstacle obj in obstaclesObjects)
            {
                Destroy(obj.gameObject);
            }

            // Resets player transform and begins spawning
            speed = 30;
            playerAnimator.SetBool("BeginGame_b", true);
            playerAnimator.SetFloat("Speed_f", 1.0f);
            dirtSplatter.Play();
            audioSource.Play();
            gameInProgress = true;

            // Restart countdown
            InvokeRepeating("TimeCountdown", 1, 1);
        }
    }

    private void EndGame()
    {
        if (gameOver || timeRemaining == 0)
        {
            gameOver = true;
            playerAnimator.SetBool("BeginGame_b", false);
            playerAnimator.SetFloat("Speed_f", 0f);
            audioSource.Stop();
            timeRemainingText.text = "Game\nOver";
        }
    }

    // This is called from and Invoke Repeating
    private void TimeCountdown()
    {
        miniGameCooldown--;
        timeRemaining--;
        if (miniGameCooldown <= 0)
        {
            StartKahootGame();
            CancelInvoke("TimeCountdown");
        }

        if (timeRemaining <= 0)
        {
            CancelInvoke("TimeCountdown");

        }

    }
}

