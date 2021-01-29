using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityHandler : QuizHandler
{
    public static CityHandler instance;

    [Header("Voucher trophy")]
    [SerializeField] Trophy voucher;
    [Header("UI Elements 2.0")]
    [SerializeField] TMP_Text questionText;
    [Header("Canvas")]
    [SerializeField] GameObject gameCanvas;
    [Header("Shop panel")]
    [SerializeField] GameObject shopCanvas;
    [Header("Images for the buttons")]
    [SerializeField] Image[] answerImages;

    [SerializeField] Sprite correctSprite;
    [SerializeField] Sprite incorrectSprite;
    [SerializeField] Sprite[] placeHolderSprite;

    [SerializeField] GameObject informationButton;

    private float nextQuestionTimer;
    private int[] pickedIndexes = new int[2];

    public void Start()
    {
        ResetMinigame();
        base.Start();
        instance = this;
    }

    public void Update()
    {
        base.Update();
        if(nextQuestionTimer > 0)
        {
            nextQuestionTimer -= Time.deltaTime;
            if(nextQuestionTimer <= 0)
            {
                if (ReceiveReward) dialogueHandler.SetDialogue("Lokaal", "Goed" + Random.Range(1, 3));
                else dialogueHandler.SetDialogue("Lokaal", "Fout" + Random.Range(1, 4));
                nextQuestionTimer = 0;
            }
        }
    }

    public override void HandleAnswer(int index)
    {
        if (!hasStarted || minigameHandler.gamePaused || AnsweredQuestion) return;

        base.HandleAnswer(index);

        pickedIndexes[0] = index;
        placeHolderSprite[0] = answerImages[index].sprite;
        if (ReceiveReward)
        {
            answerImages[index].sprite = correctSprite;
            player.CoinAmount += 2;
        }
        else
        {
            int correctIndex = GetCorrectIndex();
            pickedIndexes[1] = correctIndex;
            placeHolderSprite[1] = answerImages[correctIndex].sprite;
            answerImages[correctIndex].sprite = correctSprite;

            answerImages[index].sprite = incorrectSprite;
            player.CoinAmount += 1;
        }
        nextQuestionTimer = 1f;
    }

    /// <summary>
    /// Handles setting up the random question
    /// This handles setting it up a little bit different then the sign minigame
    /// </summary>
    public override void SetRandomQuestion()
    {
        for (int index = 0; index < placeHolderSprite.Length; index++)
        {
            if (placeHolderSprite[index] == null) continue;

            answerImages[pickedIndexes[index]].sprite = placeHolderSprite[index];
        }
        for (int index = 0; index < pickedIndexes.Length; index++)
        {
            pickedIndexes[index] = -1;
            placeHolderSprite[index] = null;
        }
        ReceiveReward = false;
        RandomQuestion = allQuestions[Random.Range(0, allQuestions.Count)];
        RandomQuestions = new List<string>(RandomQuestion.randomAnswers);
        RandomQuestions.Add(RandomQuestion.antwoord);
        RandomQuestions = Utility.Shuffle(RandomQuestions);

        allQuestions.Remove(RandomQuestion);

        SetupUI();
        AnsweredQuestion = false;
    }

    /// <summary>
    /// Handles setting up the question ui
    /// This includes the image and the text on the buttons
    /// </summary>
    public override void SetupUI()
    {
        base.SetupUI();
        questionText.text = $"{RandomQuestion.name}?";
    }

    public void ClickedStart()
    {
        base.ClickedStart();
        gameCanvas.SetActive(true);
    }

    public override void FinishMinigame()
    {
        CurrentState = State.FINISHED_MINIGAME;
        player.AddTrophy(trophy);
        dialogueHandler.SetDialogue("Lokaal", "Controller");
    }

    public override void OnDialogueFinished()
    {
        if(!hasStarted)
        {
            switch(labHandler.CurrentState)
            {
                case LabHandler.State.LOKAAL_SHOPPEN:
                    CurrentState = State.ANSWERING_QUESTIONS;
                    instructionScreen[0].SetActive(true);
                    break;
            }
        } else
        {
            //Debug.LogError("CurrentState (city): " + CurrentState);
            switch(CurrentState)
            {
                case State.ANSWERING_QUESTIONS:
                    if (AnsweredQuestions < 6)
                    {
                        SetRandomQuestion();
                        AnswerPanel.SetActive(true);
                        QuestionTextObject.SetActive(true);
                    }
                    else
                    {
                        CurrentState = State.SHOPPING;
                        gameCanvas.SetActive(false);
                        shopCanvas.SetActive(true);
                        informationButton.SetActive(true);
                    }
                    break;
                case State.BOUGHT_VOUCHER:
                    player.RemoveTrophy(voucher);
                    DisplayTrophy();
                    break;
                case State.FINISHED_MINIGAME:
                    minigameHandler.ReturnToLab(true);
                    CurrentState = State.ENDED;
                    break;
            }
            
        }
    }

    private int GetCorrectIndex()
    {
        for(int index = 0; index < RandomQuestions.Count; index++)
            if(RandomQuestion.antwoord.Equals(RandomQuestions[index]))
                return index;
        return -1;
    }

    public override void ResetMinigame()
    {
        Debug.Log("Started city handler...");
        hasStarted = false;
        gameCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        player.CoinAmount = 0;
        CurrentState = State.NONE;
        placeHolderSprite = new Sprite[2];
        for (int index = 0; index < pickedIndexes.Length; index++)
        {
            pickedIndexes[index] = -1;
            placeHolderSprite[index] = null;
        }
    }

    public State currentState = State.NONE;

    public State CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    public enum State
    {
        NONE,
        ANSWERING_QUESTIONS,
        SHOPPING,
        BOUGHT_VOUCHER,
        FINISHED_MINIGAME,
        ENDED
    }

}
