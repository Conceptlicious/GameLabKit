using System.Collections;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public Player player => Player.instance;
    public LabHandler labHandler => LabHandler.instance;
    public DialogueHandler dialogueHandler => DialogueHandler.instance;
    public MinigameHandler minigameHandler => MinigameHandler.instance;

    [Header("Trophy data")]
    public Trophy trophy;
    [SerializeField] float trophyDelay;
    [SerializeField] GameObject trophyShowoff;

    [Header("Instructions")]
    public GameObject[] instructionScreen;
    public bool hasStarted = false;

    [Header("Information")]
    private bool hasLoaded;

    public bool HasLoaded
    {
        get => hasLoaded;
        set => hasLoaded = value;
    }

    public GameObject informationPanel;

    public virtual void Start() 
    {
        hasLoaded = true;
        StartMinigame();
        dialogueHandler.onDialogueFinished += OnDialogueFinished;
    }

    public virtual void Update()
    {
        if (trophyDelay > 0)
        {
            trophyDelay -= Time.deltaTime;
            if (trophyDelay <= 0)
            {
                trophyShowoff.SetActive(false);
                FinishMinigame();
                trophyDelay = 0;
            }
        }
    }

    public abstract void StartMinigame();

    public abstract void FinishMinigame();

    public abstract void OnDialogueFinished();

    public abstract void ResetMinigame();

    public void DisplayTrophy()
    {
        trophyShowoff.SetActive(true);
        trophyDelay = 2.5f;
    }

    public void OpenInformation()
    {
        if (!hasStarted) return;
        minigameHandler.gamePaused = true;
        Time.timeScale = 0;
        informationPanel.SetActive(true);
    }

    public void CloseInformation()
    {
        Time.timeScale = 1;
        minigameHandler.gamePaused = false;
        informationPanel.SetActive(false);
    }

    public virtual void ClickedStart()
    {
        instructionScreen[0].SetActive(false);
        hasStarted = true;
    }

    /// <summary>
    /// Handles grabbing the game object the player clicked
    /// </summary>
    /// <returns></returns>
    public GameObject GetClickedGameObject()
    {
        // Builds a ray from camera point of view to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Casts the ray and get the first game object hit
        if (Physics.Raycast(ray, out hit)) return hit.transform.gameObject;
        return null;
    }

    /// <summary>
    /// Handles showing a new dialogue
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="name"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    public IEnumerator NextDialogue(string fileName, string name, float waitTime = 0.1f)
    {
        yield return new WaitForSeconds(waitTime);
        dialogueHandler.SetDialogue(fileName, name);
    }
}
