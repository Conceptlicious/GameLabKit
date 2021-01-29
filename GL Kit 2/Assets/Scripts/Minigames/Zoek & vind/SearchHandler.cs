using System.Collections.Generic;
using UnityEngine;

public class SearchHandler : Minigame
{
    private SearchUI searchUI => SearchUI.instance;

    public static SearchHandler instance;

    private bool finished;

    [Header("Found objects")]
    [SerializeField] List<SearchItem> foundObjects = new List<SearchItem>();
    [Header("UI Elements")]
    [SerializeField] GameObject gameCanvas;
    [Header("Borden minigame")]
    [SerializeField] GameObject bordenMinigame;
    [SerializeField] GameObject bordenCanvas;
    [SerializeField] GameObject bordenInstructions;
    [Header("Main camera")]
    [SerializeField] GameObject[] mainCameras;

    [Header("Resetting")]
    [SerializeField] Transform pictureParent;

    private State currentState = State.STARTING;

    public enum State
    {
        STARTING,
        ENDED
    }

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        ResetMinigame();
        base.Start();
    }

    public void Update()
    {
        base.Update();

        //Handles clicking the game objects
        if (Input.GetMouseButtonDown(0) && hasStarted)
        {
            GameObject clickedObject = GetClickedGameObject();
            if (clickedObject != null && !clickedObject.name.Equals("Plane"))
            {
                SearchInteraction searchInteraction = clickedObject.GetComponent<SearchInteraction>();
                searchInteraction?.Click();
            }
        }
    }

    public override void StartMinigame()
    {

    }

    public override void FinishMinigame()
    {

    }

    public void ClickedStart()
    {
        base.ClickedStart();
        dialogueHandler.SetDialogue("Veiligheid", "Start 2");
    }

    public override void OnDialogueFinished()
    {
        if(!hasStarted)
        {
            switch(labHandler.CurrentState)
            {
                case LabHandler.State.VEILIGHEID_MINIGAME:
                    mainCameras[0].SetActive(false);
                    mainCameras[1].SetActive(true);
                    instructionScreen[0].SetActive(true);
                    break;
            }
        } else
        {
            if (finished && currentState.Equals(State.STARTING))
            {
                foreach(Transform child in pictureParent.transform)
                    Destroy(child.gameObject);

                bordenInstructions.SetActive(true);
                bordenCanvas.SetActive(false);

                labHandler.CurrentState = LabHandler.State.VEILIGHEID_MINIGAME_2;
                mainCameras[1].SetActive(false);
                bordenMinigame.SetActive(true);
                gameObject.SetActive(false);

                currentState = State.ENDED;
            } else
            {
                switch(currentState)
                {
                    case State.STARTING:
                        gameCanvas.SetActive(true);

                        foreach (Transform child in pictureParent.transform)
                            Destroy(child.gameObject);

                        if (HasLoaded) searchUI?.Reset();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Object found event
    /// </summary>
    /// <param name="searchItem"></param>
    public delegate void OnObjectFound(SearchItem searchItem);
    public OnObjectFound onObjectFound;

    /// <summary>
    /// Handles the clicking of an object and adding them to the list of found objects
    /// </summary>
    /// <param name="searchItem"></param>
    public void ObjectFound(SearchItem searchItem)
    {
        if (!hasStarted || minigameHandler.gamePaused) return;

        if(foundObjects.Contains(searchItem))
        {
            Debug.Log("Already found this object.");
            return;
        }
        foundObjects.Add(searchItem);
        onObjectFound?.Invoke(searchItem);



        if(foundObjects.Count >= 7)
        {
            finished = true;
            Debug.Log("Finished the minigame....");
            dialogueHandler.SetDialogue("Veiligheid", "Finished zoek");
            gameCanvas.SetActive(false);

        }
    }

    public override void ResetMinigame()
    {
        hasStarted = false;
        finished = false;
        currentState = State.STARTING;

        mainCameras[1].SetActive(false);
        mainCameras[0].SetActive(true);

        foundObjects.Clear();

        gameCanvas.SetActive(false);
        if (HasLoaded) searchUI?.Reset();
    }
}
