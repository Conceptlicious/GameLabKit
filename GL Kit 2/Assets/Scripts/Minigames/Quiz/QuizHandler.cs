using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class QuizHandler : Minigame
{
    [Header("Player data")]
    [SerializeField] int answeredQuestions;
    [SerializeField] int correctAnswers;
    [SerializeField] int wrongAnswers;
    [SerializeField] bool receiveReward;
    [SerializeField] bool answeredQuestion;
    [SerializeField] TMP_Text scoreText;

    public bool AnsweredQuestion
    {
        get => answeredQuestion;
        set => answeredQuestion = value;
    }

    public bool ReceiveReward
    {
        get => receiveReward;
        set => receiveReward = value;
    }

    public int AnsweredQuestions
    {
        get => answeredQuestions;
        set => answeredQuestions = value;
    }

    public int CorrectAnswers
    {
        get => correctAnswers;
        set => correctAnswers = value;
    }

    public int WrongAnswers
    {
        get => wrongAnswers;
        set => wrongAnswers = value;
    }

    [Header("Possible questions")]
    public List<QuizAnswer> allQuestions = new List<QuizAnswer>();


    [Header("Questions related")]
    [SerializeField] QuizAnswer randomQuestion;

    public QuizAnswer RandomQuestion
    {
        get => randomQuestion;
        set => randomQuestion = value;
    }

    [SerializeField] List<string> randomQuestions;

    public List<string> RandomQuestions
    {
        get => randomQuestions;
        set => randomQuestions = value;
    }

    [Header("UI Elements")]
    [SerializeField] TMP_Text[] answersText;
    [SerializeField] GameObject questionTextObject;
    [SerializeField] GameObject answerPanel;

    public GameObject AnswerPanel
    {
        get => answerPanel;
    }

    public GameObject QuestionTextObject
    {
        get => questionTextObject;
    }

    [Header("Feedback")]
    [SerializeField] GameObject feedbackPanel;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] float feedbackTimer;

    public float FeedbackTimer
    {
        get => feedbackTimer;
        set => feedbackTimer = value;
    }

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();
        if (feedbackTimer > 0)
        {
            feedbackTimer -= Time.deltaTime;
            if (feedbackTimer <= 0)
            {
                feedbackPanel.SetActive(false);
                SetRandomQuestion();
                feedbackText.text = string.Empty;
                feedbackTimer = 0;
                answerPanel.SetActive(true);
            }
        }
    }

    public override void StartMinigame()
    {
        SetRandomQuestion();
    }

    /// <summary>
    /// Handles selecting a random question and a list of 3 fake answers and 1 correct one
    /// </summary>
    public virtual void SetRandomQuestion()
    {
        receiveReward = false;
        randomQuestion = allQuestions[Random.Range(0, allQuestions.Count)];
        randomQuestions = Utility.Shuffle(GetRandomQuestions(randomQuestion));
        SetupUI();
    }

    /// <summary>
    /// Handles setting up the question ui
    /// This includes the image and the text on the buttons
    /// </summary>
    public virtual void SetupUI()
    {
        //Debug.LogError("randomQuestions size: " + randomQuestions.Count);
        string[] indicators = { "A", "B", "C", "D" };
        for (int index = 0; index < answersText.Length; index++)
            answersText[index].text = $"{indicators[index]}: {randomQuestions[index]}";

        AnsweredQuestion = false;
    }

    /// <summary>
    /// Handles grabbing a list of 4 questions including the right one
    /// </summary>
    /// <param name="currentQuestion"></param>
    /// <returns></returns>
    List<string> GetRandomQuestions(QuizAnswer currentQuestion)
    {
        List<string> questions = new List<string>();
        do
        {
            QuizAnswer question = allQuestions[Random.Range(0, allQuestions.Count)];
            if (question.Equals(currentQuestion)) continue;
            questions.Add(question.antwoord);
        } while (questions.Count < 3);
        questions.Add(currentQuestion.antwoord);
        return questions;
    }

    /// <summary>
    /// Handles checking if the answer is correct or wrong
    /// </summary>
    /// <param name="index"></param>
    public virtual void HandleAnswer(int index)
    {
        if (!hasStarted || minigameHandler.gamePaused || AnsweredQuestion) return;

        AnsweredQuestion = true;
        string answeredPick = randomQuestions[index];
        if (answeredPick.Equals(randomQuestion.antwoord))
        {
            correctAnswers++;
            if(GetType().Equals(typeof(BordenHandler)))
                SetFeedback("Goed!", true);
            allQuestions.Remove(randomQuestion);
            receiveReward = true;
        }
        else
        {
            wrongAnswers++;
            if (GetType().Equals(typeof(BordenHandler)))
                SetFeedback("Fout!");
            receiveReward = false;
        }
        answeredQuestions++;
    }

    /// <summary>
    /// Handles setting the feedback panel
    /// </summary>
    /// <param name="text"></param>
    /// <param name="correct"></param>
    void SetFeedback(string text, bool correct = false)
    {
        answerPanel.SetActive(false);
        feedbackPanel.SetActive(true);
        feedbackText.color = correct ? Color.green : Color.red;
        feedbackText.text = $"{text}"; //\n\nAantal goed: {correctAnswers}\nAantal fout: {wrongAnswers}
        if(scoreText != null)
        {
            scoreText.text = $"<color=\"green\">Goed: {correctAnswers}</color>\n<color=\"red\">Fout: {wrongAnswers}</color>";
        }
        feedbackTimer = 1.5f;
    }
}
