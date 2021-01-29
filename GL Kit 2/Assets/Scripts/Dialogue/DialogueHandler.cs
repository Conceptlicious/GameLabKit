using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TODO: Convert to Gamelab Oost singleton system
/// TODO: Convert to GameLab Oost Bettermonobehavour system
/// </summary>
public class DialogueHandler : MonoBehaviour
{
    public static DialogueHandler instance;
    private ReadDialogueFiles dialogueFiles => ReadDialogueFiles.instance;
    private MinigameHandler minigameHandler => MinigameHandler.instance;
    private Player player => Player.instance;

    /// <summary>
    /// The box were the user clicks on to show the next dialogue
    /// </summary>
    public GameObject dialogueBox;
    /// <summary>
    /// The indicator the player has to click on the box
    /// </summary>
    public GameObject clickIndicator;
    /// <summary>
    /// 
    /// </summary>
    public TMP_Text nameText;
    /// <summary>
    /// The text component within the dialogue box
    /// </summary>
    public TMP_Text dialogueText;
    /// <summary>
    /// The action buttons like Yes or No
    /// </summary>
    public GameObject actions;
    /// <summary>
    /// The text components that are on the action buttons
    /// </summary>
    public TMP_Text[] buttonOptions;
    /// <summary>
    /// The image of the professor above the dialogue box
    /// </summary>
    public GameObject professorImage;
    /// <summary>
    /// The image of the robot above the dialogue box
    /// </summary>
    public GameObject robotImage;
    public RectTransform robotRect;
    /// <summary>
    /// The image component that shows the robot sprite
    /// </summary>
    public Image robotSprite;
    /// <summary>
    /// An array of all the robot sprites
    /// </summary>
    public Sprite[] robotSprites;
    /// <summary>
    /// 
    /// </summary>
    public GameObject playerImage;
    /// <summary>
    /// 
    /// </summary>
    public GameObject charlieImage;
    /// <summary>
    /// 
    /// </summary>
    public GameObject karenImage;

    public delegate void OnDialogueFinished();

    public OnDialogueFinished onDialogueFinished;

    void DialogueFinished()
    {
        onDialogueFinished?.Invoke();
    }

    /// <summary>
    /// The current line the user in on within a dialogue
    /// </summary>
    private int currentLine = -1;
    public int CurrentLine
    {
        get => currentLine;
        set => currentLine = value;
    }

    /// <summary>
    /// The current dialogue the user is on
    /// </summary>
    private Dialogue currentDialogue;
    public Dialogue CurrentDialogue
    {
        get => currentDialogue;
        set => currentDialogue = value;
    }

    /// <summary>
    /// The current file name the user is using
    /// </summary>
    private string currentFileName;
    public string CurrentFileName
    {
        get => currentFileName;
        set => currentFileName = value;
    }

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Handles setting a dialogue based on file name and name of the dialogue
    /// </summary>
    /// <param name="fileName">The name of the file</param>
    /// <param name="name">The name of the dialogue</param>
    public void SetDialogue(string fileName, string name)
    {
        Dialogue dialogue = dialogueFiles.GetDialogue(fileName, name);
        
        if (dialogue == null)
        {
            Debug.LogError($"No dialogue was found for the following parameters: fileName: {fileName}, name: {name}");
            return;
        }

        Reset();
        CurrentFileName = fileName;
        CurrentDialogue = dialogue;
        NextDialogue();
    }

    /// <summary>
    /// Handles showing the next dialogue
    /// </summary>
    public void NextDialogue()
    {
        if (minigameHandler.gamePaused) return;

        if (CurrentDialogue == null) Debug.LogError("Current dialogue null");
        if (CurrentDialogue.dialogueType.Equals(DialogueType.OPTION))
        {
            Debug.LogError("Je kunt niet naar een volgende dialogue gaan als het een option dialogue is");
            return;
        }
        
        if (currentLine + 1 >= CurrentDialogue.lines.Count)
        {
            Close();
            return;
        }

        string line = CurrentDialogue.lines[++CurrentLine];

        int amountOfMembers = int.Parse(line.Substring(0, 1));
        string indicator = line.Substring(1, 1);
        line = line.Replace(amountOfMembers +"" + indicator + "-", "").Replace("%u", player.PlayerName);
        DialogueType dialogueType = line.Contains("[") && line.Contains("]") ? DialogueType.OPTION : DialogueType.TEXT;

        professorImage.SetActive(amountOfMembers == 1 && (indicator.Equals("P") || indicator.Equals("B") || indicator.Equals("M") || indicator.Equals("N") || indicator.Equals("O") || indicator.Equals("I") || indicator.Equals("U")) || amountOfMembers > 1);

        bool showRobot = (amountOfMembers > 1 || (amountOfMembers == 1 && (indicator.Equals("Q") || indicator.Equals("W") || indicator.Equals("E") || indicator.Equals("R") || indicator.Equals("T") || indicator.Equals("Y"))));
        if (showRobot)
        {
            Sprite sprite = GetRobotSprite(indicator);
            robotSprite.sprite = sprite;
            robotRect.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
        }
        robotImage.SetActive(showRobot);

        playerImage.SetActive(indicator.Equals("S") && amountOfMembers >= 1);
        charlieImage.SetActive(indicator.Equals("C") && amountOfMembers >= 1);
        karenImage.SetActive(indicator.Equals("K") && amountOfMembers >= 1);

        actions.SetActive(dialogueType.Equals(DialogueType.OPTION));
        clickIndicator.SetActive(!dialogueType.Equals(DialogueType.OPTION));

        if (dialogueType.Equals(DialogueType.OPTION))
        {
            //Handles splitting the text and options from eaother
            string[] outcome = line.Split('-');
            line = outcome[0];

            //Handles removing the [] around the options and splits the 2 actions (most of the time yes and no)
            string[] options = outcome[1].Replace("[", "").Replace("]", "").Split(',');
            
            //Handles splitting the text and action (Text for the button and what dialogue the button calls)
            string[] optionOne = options[0].Split(';');
            string[] optionTwo = options[1].Split(';');

            //Sets the text within the buttons
            buttonOptions[0].text = optionOne[0];
            buttonOptions[1].text = optionTwo[0];
            
            CurrentDialogue.actions = new string[] { optionOne[1], optionTwo[1].Remove(optionTwo[1].Length - 1) };
        }

        CurrentDialogue.dialogueType = dialogueType;
        dialogueText.text = line;
        nameText.text = GetName(indicator);
        dialogueBox.SetActive(true);
    }

    /// <summary>
    /// Handles the options like Yes or No
    /// </summary>
    /// <param name="option"></param>
    public void HandleOption(int option)
    {
        string action = CurrentDialogue.actions[option];
        switch (action)
        {
            case "End":
            case "Close":
                Close();
                break;
            default:
                SetDialogue(CurrentFileName, action);
                break;
        }
    }

    /// <summary>
    /// Handles resetting the dialogue
    /// </summary>
    private void Reset()
    {
        dialogueText.text = String.Empty;
        currentLine = -1;
        CurrentDialogue = null;
        CurrentFileName = String.Empty;
    }

    /// <summary>
    /// Handles closing the dialogue box
    /// </summary>
    public void Close()
    {
        //Debug.LogError("Reached the end of the dialogue, do something");
        DialogueFinished();
        actions.SetActive(false);
        dialogueBox.SetActive(false);
        Reset();
    }

    /// <summary>
    /// Handles getting the actuall name of the speaker with the indicator
    /// </summary>
    /// <param name="indicator"></param>
    /// <returns></returns>
    private string GetName(string indicator)
    {
        switch(indicator)
        {
            case "P":
            case "M":
            case "O":
            case "I":
            case "U":
            case "N":
            case "B":
                return "Prof. Henk";
            case "Q":
                return "LUXe bot";
            case "W":
                return "B.O.B.";
            case "E":
                return "Ram-C";
            case "R":
                return "K.M";
            case "T":
                return "I.D.K.";
            case "Y":
                return "U.Go";
            case "C":
                return "Charlie";
            case "K":
                return "Karen";
        }
        return player.PlayerName;
    }

    /// <summary>
    /// Handles getting the sprites for the robot
    /// </summary>
    /// <param name="indicator"></param>
    /// <returns></returns>
    private Sprite GetRobotSprite(string indicator)
    {
        switch (indicator)
        {
            case "Q":
            case "O":
                return robotSprites[3];//"LUXe bot";
            case "W":
            case "I":
                return robotSprites[0];//"B.O.B.";

            case "E":
            case "B":
                return robotSprites[4];//"Ram-C";

            case "R":
            case "U":
                return robotSprites[2];//"K.M";

            case "T":
            case "N":
                return robotSprites[1];//"I.D.K.";

            case "Y":
            case "M":
                return robotSprites[5];//"U.Go";
        }
        return null;
    }
}
