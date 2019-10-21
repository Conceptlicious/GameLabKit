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
	private const int HIDDENOBJECTS_AMOUNT = 7;

	public bool MinigameIsWon { get; private set; } = false;
	[SerializeField] private GameObject nextMinigameButton = null;
	[SerializeField] private Image buttonsImage;
	[HideInInspector] public Sprite lastSelectedObjectSprite;
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
		HiddenObject currentHiddenObject = foundObject.GetComponent<HiddenObject>();

		if (!foundObjects.Contains(foundObject))
		{
			currentHiddenObject.Found();
			foundObjects.Add(foundObject);
			foundObjectName = foundObject.name;

			ProgressDialogue();

			if (foundObjects.Count >= HIDDENOBJECTS_AMOUNT)
			{
				nextMinigameButton.SetActive(true);
				MinigameIsWon = true;
			}
		}
		else
		{
			if (MinigameIsWon)
			{
				return;
			}
		}
	}

	public void NextRoom()
	{
		if (lastSelectedObjectSprite == null)
		{
			return;
		} 
		buttonsImage.gameObject.SetActive(true);
		//EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
	}

	public void YesButton()
	{
		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Goals));
		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		buttonsImage.gameObject.SetActive(false);
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}

	public void NoButton()
	{
		buttonsImage.gameObject.SetActive(false);
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
	}
}