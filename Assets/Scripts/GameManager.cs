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
    public static bool miniGame = false;
    public static float score;
    private AudioSource audioSource;
    private int timeRemaining = 60;
    private int miniGameCooldown = 10;
    public static bool timedGame = false;
    private UIController uiControllerScript;
    private QuizController quizControllerScript;
    public static bool gameInProgress;
    private static GameObject lastObject;
    private static GameManager instance;
    public static float speed = 30f;

    


    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        BackToGame();
        GameInProgress();
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

    private void TimeCountdown()
    {
        miniGameCooldown--;
        timeRemaining--;
        if(miniGameCooldown <= 0)
        {
           StartMiniGame();
            CancelInvoke("TimeCountdown");
        }

        if(timeRemaining <= 0)
        {
            CancelInvoke("TimeCountdown");
            
        }

    }

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

    private void EndGame()
    {
        
        if(gameOver || timeRemaining == 0)
        {
            gameOver = true;
            playerAnimator.SetBool("BeginGame_b", false);
            playerAnimator.SetFloat("Speed_f", 0f);
            audioSource.Stop();
            CancelInvoke();
            timeRemainingText.text = "Game\nOver";
        }
        
    }

    public void SetTimed(bool timed)
    {
        timedGame = timed;
    }

    public static void ChangeScore(float change)
    {
        score += change;
    }

    private void StartMiniGame()
    {
        gameInProgress = true;
        SceneManager.LoadScene(1);
        QuizController.backToGame = false;
        

        
    }

    private void GameInProgress()
    {
        if(gameInProgress)
        {
            timeRemainingText.gameObject.SetActive(true);
            toggleGroup.SetActive(false);
            startButton.SetActive(false);
        }
    }
    public void BackToGame()
    {
        if (QuizController.backToGame)
        {
            playerAnimator.SetBool("BeginGame_b", true);
            playerAnimator.SetFloat("Speed_f", 1.0f);
            miniGameCooldown = 10;
            InvokeRepeating("TimeCountdown", 1, 1);
            Debug.Log("working");
        };
    }
}

