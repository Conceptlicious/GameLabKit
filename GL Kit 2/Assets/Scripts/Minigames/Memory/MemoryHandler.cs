using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MemoryHandler : Minigame
{
    public static MemoryHandler instance;

    //[Header("All sprites that are used for the randomness")]
    //[SerializeField] Sprite[] allSprites;

    [Header("The current level the player is on")]
    [SerializeField] MemoryLevel[] allLevels;
    [SerializeField] int levelIndex = 0;
    [SerializeField] MemoryLevel currentLevel;

    [Header("Memory objects")]
    [SerializeField] GameObject buttonParent;
    [SerializeField] GameObject buttonPrefab;

    [Header("Game data")]
    [SerializeField] float levelTimer;
    [SerializeField] int failedAttempts;
    [SerializeField] int maxSize;
    [SerializeField] List<Sprite> filledBoard;
    [SerializeField] bool levelTransition;

    [Header("Lockers")]
    [SerializeField] MemoryButton[] memoryButtons;
    [Header("Images within the locker")]
    [SerializeField] SpriteRenderer[] lockerRenderer;

    [Header("Message of the assignment")]
    [SerializeField] TMP_Text opdrachtText;

    [Header("Menu buttons")]
    [SerializeField] GameObject[] infoObjects;

    public bool LevelTransition
    {
        get => levelTransition;
        private set => levelTransition = value;
    }

    [SerializeField] List<MemoryButton> flipped = new List<MemoryButton>(2);

    void Start()
    {
        instance = this;
        base.Start();
    }

    private void Update()
    {
        base.Update();
        if(levelTimer > 0)
        {
            levelTimer -= Time.deltaTime;
            if(levelTimer < 0)
            {
                LevelTransition = true;
                StartCoroutine(NextLevel());
                levelTimer = 0;
            }
        }
    }

    /// <summary>
    /// Handles starting the minigame
    /// </summary>
    public override void StartMinigame()
    {
        currentLevel = allLevels[levelIndex];
        maxSize = currentLevel.width * currentLevel.height;
        filledBoard = new List<Sprite>(maxSize);
    }

    /// <summary>
    /// Handles spawning the board
    /// </summary>
    void SpawnBoard(bool setup)
    {
        for(int index = 0; index < filledBoard.Count; index++)
        {
            MemoryButton memoryButton = memoryButtons[index];
            Sprite sprite = filledBoard[index];

            lockerRenderer[index].sprite = sprite;
            memoryButton.SetRealSprite(sprite);
            memoryButton.TempFlip();
            if(setup) memoryButton.onButtonClicked += ButtonClicked;
        }
    }

    /// <summary>
    /// Handles filling the board with all the required spirtes and random onces
    /// </summary>
    void FillBoard()
    {
        filledBoard.Clear();
        //Fill the list with the mandatory sprites
        foreach (Sprite sprite in currentLevel.requiredObject)
            filledBoard.Add(sprite);
        //Fill the board with unique sprites
        do
        {
            Sprite sprite = currentLevel.randomSprites[Random.Range(0, currentLevel.randomSprites.Length)];
            if (filledBoard.Contains(sprite)) continue;

            filledBoard.Add(sprite);
        } while (filledBoard.Count < maxSize);
        //Shuffle the list
        filledBoard = Utility.Shuffle(filledBoard);

        opdrachtText.text = $"{currentLevel.opdracht}";
    }

    /// <summary>
    /// Handles the button click event
    /// </summary>
    /// <param name="memoryButton"></param>
    void ButtonClicked(MemoryButton memoryButton)
    {
        flipped.Add(memoryButton);
        //Debug.LogError($"flipped.Count: {flipped.Count}, currentLevel.requiredObject.Length: {currentLevel.requiredObject.Length}");
        if(flipped.Count == currentLevel.requiredObject.Length)
        {
            //Debug.LogError($"Flipped {currentLevel.requiredObject.Length} objects do the check...");
            if(MatchingSprites())
            {
                LevelTransition = true;
                StartCoroutine(NextLevel());
            } else StartCoroutine(Reflip());
        }
    }

    /// <summary>
    /// Handles switching to the next level
    /// </summary>
    /// <returns></returns>
    IEnumerator NextLevel()
    {
        //Wait for 0.5 a second
        yield return new WaitForSeconds(0.5f);

        foreach (Transform child in buttonParent.transform)
            Destroy(child.gameObject);

        flipped.Clear();

        levelIndex++;

        if(levelIndex >= allLevels.Length)
        {
            levelTimer = 0;
            opdrachtText.text = "";
            Debug.LogError("Finished the minigame");
            dialogueHandler.SetDialogue("Memory", "Klaar");
            yield break;
        }

        SwitchLevel(false);

        LevelTransition = false;

        failedAttempts = 0;
    }

    /// <summary>
    /// Handles reflipping the buttons the players clicked
    /// </summary>
    /// <returns></returns>
    IEnumerator Reflip()
    {
        failedAttempts++;
        //Wait for 0.5 a second
        yield return new WaitForSeconds(0.5f);


        if (failedAttempts == 3)
        {
            LevelTransition = true;
            StartCoroutine(NextLevel());
        }
        else
        {
            //Makes it so it reflips the flipped buttons
            foreach (MemoryButton button in flipped)
                button.Flip(false);
            //Clear the list of flipped buttons
            flipped.Clear();
        }
    }

    /// <summary>
    /// Handles checking if the player has the correct spirtes
    /// </summary>
    /// <returns></returns>
    private bool MatchingSprites()
    {
        bool matches = true;
        foreach (MemoryButton button in flipped)
        {
            Sprite sprite = button.RealSprite;
            bool matchingSprite = false;
            foreach (Sprite requiredSprite in currentLevel.requiredObject)
            {
                if (requiredSprite.Equals(sprite))
                {
                    matchingSprite = true;
                    break;
                }
            }
            if (!matchingSprite)
            {
                matches = false;
                break;
            }
        }
        return matches;
    }

    /// <summary>
    /// Handles switching levels
    /// </summary>
    private void SwitchLevel(bool setup)
    {
        currentLevel = allLevels[levelIndex];
        maxSize = currentLevel.width * currentLevel.height;
        filledBoard = new List<Sprite>(maxSize);
        FillBoard();
        SpawnBoard(setup);
        levelTimer = 10;
    }

    /// <summary>
    /// Handles the clicking of the start button on the instruction panel
    /// </summary>
    public void ClickedStart()
    {
        base.ClickedStart();
        SwitchLevel(true);
        foreach (GameObject o in infoObjects)
            o.SetActive(true);
    }

    /// <summary>
    /// Handles finishing the minigame
    /// </summary>
    public override void FinishMinigame()
    {
        player.AddTrophy(trophy);
        minigameHandler.ReturnToLab(true);
    }

    /// <summary>
    /// Handles the closing of a dialogue
    /// </summary>
    public override void OnDialogueFinished()
    {
        if(!hasStarted)
        {
            switch(labHandler.CurrentState)
            {
                case LabHandler.State.MEMEORY_MINIGAME:
                    instructionScreen[0].SetActive(true);
                    break;
            }
        } else
        {
            switch (labHandler.CurrentState)
            {
                case LabHandler.State.MEMEORY_MINIGAME:
                    DisplayTrophy();
                    break;
            }
        }
    }

    public override void ResetMinigame()
    {
        hasStarted = false;
        opdrachtText.text = "";
        levelTimer = 0;
        levelIndex = 0;
        LevelTransition = false;
        failedAttempts = 0;
        foreach (GameObject o in infoObjects)
            o.SetActive(false);
        flipped.Clear();
        foreach (MemoryButton button in memoryButtons)
            button.Reset();
    }
}
