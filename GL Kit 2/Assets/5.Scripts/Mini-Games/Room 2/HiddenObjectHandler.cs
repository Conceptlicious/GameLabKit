using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Mathias
//Overview: This script is the core of room 2, It keeps track of which objects are found,
//and which message should be send to the TextUpdater.
//Usage: Once on the CollectedHiddenObjectBar.
//--------------------------------------------------
public class HiddenObjectHandler : Singleton<HiddenObjectHandler>
{
	private const string GOAL_VARIABLE = "goal";
	private const int HIDDENOBJECTS_AMOUNT = 7;

	public bool MinigameIsWon { get; private set; } = false;
	public Sprite SelectedObjectSprite { get; private set; }
	[SerializeField] private GameObject nextMinigameButton = null;
	private List<GameObject> foundObjects = new List<GameObject>();
	private int currentKnotID = 2;	
	private string currentKnotPath = string.Empty;
	private string foundObjectName = string.Empty;

	private void Start()
	{
		SetVariables();
	}

	public void ObjectFound(GameObject foundObject)
	{
		foundObjectName = foundObject.name;
		HiddenObject currentHiddenObject = foundObject.GetComponent<HiddenObject>();

		if (!foundObjects.Contains(foundObject))
		{
			currentHiddenObject.Found();
			foundObjects.Add(foundObject);			

			ProgressDialogue();

			if (foundObjects.Count >= HIDDENOBJECTS_AMOUNT)
			{				
				MinigameIsWon = true;
			}
		}
		else
		{
			DialogueManager.Instance.CurrentDialogue.CurrentKnot = foundObjectName;
			MenuManager.Instance.OpenMenu<DialogueMenu>();
		}
	}

	public void SelectObject(string name, Sprite sprite)
	{
		DialogueManager.Instance.CurrentDialogue.SetStringVariable(GOAL_VARIABLE, $"\"{name}\"");
		SelectedObjectSprite = sprite;
	}

	private void ProgressDialogue()
	{
		if (foundObjects.Count <= HIDDENOBJECTS_AMOUNT)
		{
			currentKnotPath = $"Part{currentKnotID}";

			DialogueManager.Instance.CurrentDialogue.CurrentKnot = currentKnotPath;
			MenuManager.Instance.OpenMenu<DialogueMenu>();
			++currentKnotID;
		}
	}

	private void OnDialogueKnotCompleted(DialogueKnotCompletedEvent eventData)
	{
		if (eventData.Knot != currentKnotPath || eventData.CompletedRoomID != RoomType.Goals)
		{
			return;
		}
		
		//DialogueManager.Instance.CurrentDialogue.Reset();
		DialogueManager.Instance.CurrentDialogue.CurrentKnot = foundObjectName;
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}

	private void OnDialogueChoiceSelected(DialogueChoiceSelectedEvent eventData)
	{
		if(eventData.DialogueChoice.text != "Yes" || DialogueManager.Instance.CurrentRoomID != RoomType.Goals)
		{
			return;
		}

		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Goals));
		EventManager.Instance.RaiseEvent(new NextRoomEvent());		
	}

	private void OnFinishedRoomTransition(FinishedRoomTransition eventData)
	{
		int currentRoomID = RoomManager.Instance.GetCurrentRoomID().z;

		if (currentRoomID == 2)
		{
			DialogueManager.Instance.SetCurrentDialogue(RoomType.Goals);
			MenuManager.Instance.OpenMenu<DialogueMenu>();
		}
	}

	private void SetVariables()
	{
		nextMinigameButton.SetActive(false);
		EventManager.Instance.AddListener<FinishedRoomTransition>(OnFinishedRoomTransition);
		EventManager.Instance.AddListener<DialogueKnotCompletedEvent>(OnDialogueKnotCompleted);
		EventManager.Instance.AddListener<DialogueChoiceSelectedEvent>(OnDialogueChoiceSelected);
	}
}