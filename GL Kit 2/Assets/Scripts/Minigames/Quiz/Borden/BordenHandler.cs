using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BordenHandler : QuizHandler
{
    [Header("UI Elements v2")]
    [SerializeField] Image signImage;
    [SerializeField] GameObject bordCanvas;

    private bool finished;

    private void Start()
    {
        ResetMinigame();
        base.Start();
        instructionScreen[0].SetActive(true);
    }

    /// <summary>
    /// Handles setting up the question ui
    /// This includes the image and the text on the buttons
    /// </summary>
    public override void SetupUI()
    {
        base.SetupUI();
        signImage.sprite = RandomQuestion.sprite;
    }

    public void HandleAnswer(int index)
    {
        base.HandleAnswer(index);

        if (CorrectAnswers >= 5)
        {
            Debug.Log("Answered enough questiosn....");
            finished = true;
            bordCanvas.SetActive(false);
            dialogueHandler.SetDialogue("Veiligheid", "Finished borden");
        }
    }

    public override void FinishMinigame()
    {
        player.AddTrophy(trophy);
        minigameHandler.ReturnToLab(true);
    }

    public void ClickedStart()
    {
        if (HasLoaded) ResetMinigame();
        base.ClickedStart();
        bordCanvas.SetActive(true);
    }

    public override void OnDialogueFinished()
    {
        if(finished)
        {
            switch (labHandler.CurrentState)
            {
                case LabHandler.State.VEILIGHEID_MINIGAME_2:
                    DisplayTrophy();
                    break;
            }
        }
    }

    public override void ResetMinigame()
    {
        Debug.Log("Starting borden minigame...");
        hasStarted = false;
        finished = false;
        AnsweredQuestions = 0;
        CorrectAnswers = 0;
        WrongAnswers = 0;
    }
}
