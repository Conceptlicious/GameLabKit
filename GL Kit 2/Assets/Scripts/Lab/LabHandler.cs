using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LabHandler : MonoBehaviour
{
    public static LabHandler instance;

    public enum State
    {
        IDLE,
        INTRODUCE,
        BRIEF,
        MEET,
        SHOW_LAB,
        FINISHED_TOUR,
        GO_TO_PORTAL,
        UNVIEL_PORTAL,
        PORTAL_FIRE,
        SHOW_ROBOT,
        BLUS_MINIGAME,
        TURN_PORTAL_ON,
        ENTER_PORTAL,
        ENTERED_PORTAL,
        COMPUTER_SCREEN,
        FOOD_MINIGAME_START,
        FOOD_MINIGAME,
        MEMEORY_MINIGAME,
        VEILIGHEID_MINIGAME,
        VEILIGHEID_MINIGAME_2,
        LOKAAL_SHOPPEN,
        JEUGDZORG,
        COLLECTED,
        MOVE_TROPHY,
        AT_TROPHY,
        PLACED_DIALOGUE,
        PLACED_TROPHIES,
        EXIT_GAME
    }

    private MinigameHandler minigameHandler => MinigameHandler.instance;
    private DialogueHandler dialogueHandler => DialogueHandler.instance;
    private Player player => Player.instance;

    [Header("Thropy case")]
    [SerializeField] GameObject[] trophies;
    private float trophyDelay;
    private int currentTrophy;
    [SerializeField] Transform trophyCaseLocation;
    [SerializeField] GameObject exitGameButton;

    [Header("Portal animation")]
    [SerializeField] bool portalActive;
    [SerializeField] GameObject portalCanvas;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Animator[] portalAnimation;

    [Header("Current state of the Intro")]
    [SerializeField] State currentState;

    [Header("Camera data")]
    [SerializeField] GameObject[] allCameras;
    [SerializeField] int currentCamera;
    [SerializeField] Transform[] cameraPositions;
    [SerializeField] int currentPosition;
    [SerializeField] Transform portalLocation;
    [SerializeField] Transform portalLocationTwo;

    [Header("Brief data")]
    [SerializeField] GameObject briefObject;
    [SerializeField] TMP_Text briefContent;

    [Header("Setting the name")]
    [SerializeField] GameObject enteringName;
    [SerializeField] TMP_InputField inputField;

    [Header("Fire parent")]
    [SerializeField] GameObject fireParent;

    [Header("Portal Effect")]
    [SerializeField] GameObject portalEffect;

    [Header("Map of the game")]
    [SerializeField] GameObject computerScreen;

    [Header("Random objects")]
    public GameObject curtainObject;
    public GameObject roetObject;

    public State CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    void Start()
    {
        instance = this;
        currentCamera = 0;
        dialogueHandler.onDialogueFinished += OnDialogueFinished;
        videoPlayer.loopPointReached += EndReached;
    }

    public void StartGame(bool skip = false)
    {
        if (!skip)
        {
            CurrentState = State.INTRODUCE;
            enteringName.SetActive(true);
            Debug.LogError("Player wants to follow all the minigames");
        } else
        {
            Transform camera = allCameras[currentCamera].transform;
            camera.position = portalLocation.position;// new Vector3(-1.59f, 1.57f, 5.88f);
            camera.localRotation = new Quaternion(0, 0, 0, 0);

            curtainObject.SetActive(false);

            Debug.LogError("Player wants to skip all minigames");
            FinishedMinigames();
        }
    }

    public void FinishedMinigames()
    {
        portalEffect.SetActive(false);
        CurrentState = State.COLLECTED;
        StartCoroutine(NextDialogue("Prof. Henk", "Collected", 0));
    }

    private void Update()
    {
        if (minigameHandler.gamePaused) return;

        Transform camera = allCameras[currentCamera].transform;
        float step = 4 * Time.deltaTime;
        //Handles moving the camera down the stairs
        if (CurrentState.Equals(State.SHOW_LAB))
        {
            Transform realPos = cameraPositions[currentPosition];
            camera.position = Vector3.MoveTowards(camera.position, realPos.position, step);

            float cameraY = camera.eulerAngles.y;
            //Handles rotating the camera to the bottom of the stairs
            if (currentPosition == 1 && Vector3.Distance(camera.position, realPos.position) < 3f && cameraY > 90)
            {
                RotateCamera(camera, 90);
            }
            //Handles rotating the camera to the portal
            else if (currentPosition == 2 && Vector3.Distance(camera.position, realPos.position) < 3f && cameraY > 0)
            {
                RotateCamera(camera, 0);
            }
            //Handles switching the state of the camera if it is close enough
            if (Vector3.Distance(camera.position, realPos.position) < 0.001f)
            {
                currentPosition++;
                //Handles setting the end of the tour
                if (currentPosition >= cameraPositions.Length)
                {
                    CurrentState = State.FINISHED_TOUR;
                    StartCoroutine(NextDialogue("Prof. Henk", "Intro2"));
                }
            }
        }
        //Handles moving the camera infront of the portal
        else if (CurrentState.Equals(State.GO_TO_PORTAL))
        {
            camera.position = Vector3.MoveTowards(camera.position, portalLocation.position, step);
            if (Vector3.Distance(camera.position, portalLocation.position) < 0.001f)
            {
                CurrentState = State.UNVIEL_PORTAL;
                portalAnimation[0].SetBool("WillMove_L", true);
                portalAnimation[1].SetBool("WillMove_R", true);
                StartCoroutine(NextDialogue("Prof. Henk", "Show portal", 1.25f));
            }
        }
        //Handles the player entering the portal
        else if(CurrentState.Equals(State.ENTER_PORTAL))
        {
            camera.position = Vector3.MoveTowards(camera.position, portalLocationTwo.position, step);
            if (Vector3.Distance(camera.position, portalLocationTwo.position) < 0.001f)
                CurrentState = State.ENTERED_PORTAL;
        }
        //Handles moving the camera infront of the trophy case
        else if (CurrentState.Equals(State.MOVE_TROPHY))
        {
            camera.position = Vector3.MoveTowards(camera.position, trophyCaseLocation.position, step);
            if (Vector3.Distance(camera.position, trophyCaseLocation.position) < 0.001f)
            {
                CurrentState = State.AT_TROPHY;
                trophyDelay = 0.25f;
            }
        }
        //Handles placing the trophies in the trophy case
        else if (CurrentState.Equals(State.AT_TROPHY))
        {
            if (trophyDelay > 0 && currentTrophy < trophies.Length)
            {
                trophyDelay -= Time.deltaTime;
                if (trophyDelay <= 0)
                {
                    trophies[currentTrophy++].SetActive(true);
                    trophyDelay = 1f;
                }
            }
            else
            {
                CurrentState = State.PLACED_DIALOGUE;
                StartCoroutine(NextDialogue("Prof. Henk", "Kast", 0));
            }
        }
        //Handles the interaction of the trophies
        else if(CurrentState.Equals(State.PLACED_TROPHIES))
        {

        }
    }


    /// <summary>
    /// Handles rotating the camera
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="minRotation"></param>
    private void RotateCamera(Transform camera, float minRotation)
    {
        Vector3 rotateValue = new Vector3(0, 120, 0) * Time.deltaTime;

        Vector3 outcome = camera.eulerAngles - rotateValue;

        if (outcome.y < minRotation) outcome = new Vector3(outcome.x, minRotation, outcome.z);

        camera.eulerAngles = outcome;
    }
    /// <summary>
    /// Handles starting the intro
    /// </summary>
    public void StartIntro()
    {
        briefObject.SetActive(false);
        dialogueHandler.SetDialogue("Prof. Henk", "Intro");
        CurrentState = State.MEET;
    }

    private void OnDialogueFinished()
    {
        switch(CurrentState)
        {
            case State.MEET:
                CurrentState = State.SHOW_LAB;
                break;
            case State.FINISHED_TOUR:
                CurrentState = State.GO_TO_PORTAL;
                break;
            case State.INTRODUCE:
                break;
            case State.UNVIEL_PORTAL:
                fireParent.SetActive(true);
                CurrentState = State.PORTAL_FIRE;
                StartCoroutine(NextDialogue("Prof. Henk", "Portal fire", 1.45f));
                break;
            /*case State.PORTAL_FIRE:
                //TODO: Show the robot next to a puddle of water
                CurrentState = State.SHOW_ROBOT;
                StartCoroutine(NextDialogue("Prof. Henk", "Robot", 2f));
                break;*/
            case State.PORTAL_FIRE:
                fireParent.SetActive(false);
                CurrentState = State.BLUS_MINIGAME;
                minigameHandler.StartMinigame("blussen");
                break;
            case State.BLUS_MINIGAME:
                Debug.Log("POG animatie dat de speler de portal in gaat");
                CurrentState = State.ENTER_PORTAL;
                StartCoroutine(EnteringPortal());
                break;
            case State.FOOD_MINIGAME_START:
                TogglePortal();
                break;
            case State.COLLECTED:
                CurrentState = State.MOVE_TROPHY;
                break;
            case State.PLACED_DIALOGUE:
                exitGameButton.SetActive(true);
                CurrentState = State.PLACED_TROPHIES;
                break;
            case State.TURN_PORTAL_ON:
                portalEffect.SetActive(true);
                StartCoroutine(NextDialogue("Prof. Henk", "Turned on portal", 1f));
                CurrentState = State.BLUS_MINIGAME;
                break;
            case State.EXIT_GAME:
                SceneManager.LoadScene(0);
                break;
        }
    }

    /// <summary>
    /// Handles setting the username of the player
    /// </summary>
    public void SetUsername()
    {
        enteringName.SetActive(false);
        player.PlayerName = inputField.text;
        CurrentState = State.BRIEF;
        briefContent.text = briefContent.text.Replace("[NAAM]", player.PlayerName);
        briefObject.SetActive(true);
        //StartCoroutine(NextDialogue("Prof. Henk", "Intro3", 0));
    }

    /// <summary>
    /// Handles showing a new dialogue
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="name"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator NextDialogue(string fileName, string name, float waitTime = 0.1f)
    {
        yield return new WaitForSeconds(waitTime);
        dialogueHandler.SetDialogue(fileName, name);
    }

    /// <summary>
    /// Handles entering the portal
    /// </summary>
    /// <returns></returns>
    IEnumerator EnteringPortal()
    {
        yield return new WaitForSeconds(1f);
        CurrentState = State.COMPUTER_SCREEN;
        computerScreen.SetActive(true);
    }

    /// <summary>
    /// Handles toggling the portal canvas
    /// </summary>
    public void TogglePortal()
    {
        portalActive = !portalActive;
        portalCanvas.SetActive(portalActive);
    }

    /// <summary>
    /// Handles reaching the end of the portal animation
    /// </summary>
    /// <param name="source"></param>
    private void EndReached(VideoPlayer source)
    {
        TogglePortal();
        switch (CurrentState)
        {
            case State.FOOD_MINIGAME_START:
                CurrentState = State.FOOD_MINIGAME;
                minigameHandler.StartMinigame("food");
                StartCoroutine(NextDialogue("Food truck", "Start", 0));
                FoodHandler foodHandler = (FoodHandler)FindObjectOfType(typeof(FoodHandler));
                if ((bool)(foodHandler?.HasLoaded)) foodHandler?.ResetMinigame();
                break;
            case State.MEMEORY_MINIGAME:
                minigameHandler.StartMinigame("marketing");
                dialogueHandler.SetDialogue("Memory", "Start");

                MemoryHandler memoryHandler = (MemoryHandler)FindObjectOfType(typeof(MemoryHandler));
                if ((bool)(memoryHandler?.HasLoaded)) memoryHandler?.ResetMinigame();
                break;
            case State.VEILIGHEID_MINIGAME:
                minigameHandler.StartMinigame("veiligheid");
                dialogueHandler.SetDialogue("Veiligheid", "Start");

                SearchHandler searchHandler = (SearchHandler)FindObjectOfType(typeof(SearchHandler));
                if ((bool)(searchHandler?.HasLoaded)) searchHandler?.ResetMinigame();
                break;
            case State.LOKAAL_SHOPPEN:
                minigameHandler.StartMinigame("lokaal");
                dialogueHandler.SetDialogue("Lokaal", "Start");

                CityHandler cityHandler = (CityHandler)FindObjectOfType(typeof(CityHandler));
                if((bool)(cityHandler?.HasLoaded)) cityHandler?.ResetMinigame();
                break;
            case State.JEUGDZORG:
                minigameHandler.StartMinigame("jeugdzorg");
                dialogueHandler.SetDialogue("Pleegouders", "Start");

                PleegouderHandler pleegouder = (PleegouderHandler)FindObjectOfType(typeof(PleegouderHandler));
                if ((bool)(pleegouder?.HasLoaded)) pleegouder?.ResetMinigame();
                break;
        }
    }
}
