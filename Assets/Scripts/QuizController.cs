using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizController : MonoBehaviour
{
    private QuestionCollection questionCollection;
    private QuizQuestion currentQuestion;
    private UIController uiController;

    public static bool backToGame;

    public static float delayBetweenQuestions = 2.0f;

    private void Awake()
    {
        questionCollection = FindObjectOfType<QuestionCollection>();
        uiController = FindObjectOfType<UIController>();
    }

    private void Start()
    {
        PresentQuestion();
    }

    private void PresentQuestion()
    {
        currentQuestion = questionCollection.GetUnaskedQuestion();
        uiController.SetupUIForQuestion(currentQuestion);
    }

    public void SubmitAnswer(int answerNumber)
    {
        bool isCorrect = answerNumber == currentQuestion.CorrectAnswer;
        uiController.HandleSubmittedAnswer(isCorrect);
        InvokeRepeating("SwitchToGame", 1, 1);
        
    }

    private void SwitchToGame()
    {
        delayBetweenQuestions--;

        if (delayBetweenQuestions <= 0)
        {
            backToGame = true;
            SceneManager.LoadScene(0);
        }

    }
}