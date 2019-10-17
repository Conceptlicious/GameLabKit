using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;
using System;

//--------------------------------------------------
//Produced by Mathias
//Overview: This script is the core of room 2, It keeps track of which objects are found,
//and which message should be send to the TextUpdater.
//Usage: Once on the CollectedHiddenObjectBar.
//--------------------------------------------------
public class HiddenObjectHandler : Singleton<HiddenObjectHandler>
{
	private const string OBJECT_ALREADY_FOUND_MESSAGE = "You have already found this object";
	private const string WARNING = "Warning";
	private const string WARNING_EXPLINATION = "There is no object selected";
	private const int HIDDENOBJECTS_AMOUNT = 7;

	public bool MinigameIsWon { get; private set; } = false;
	[SerializeField] private GameObject nextMinigameButton = null;
	[SerializeField] private Image buttonsImage;
	[HideInInspector] public Sprite lastSelectedObjectSprite;
	private List<GameObject> foundObjects = new List<GameObject>();
	private int currentDialoguePart = 2;

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
			ProgressDialogue();

			if (foundObjects.Count >= HIDDENOBJECTS_AMOUNT)
			{
				nextMinigameButton.SetActive(true);
				TextUpdater.Instance.CallUpdateTextCoroutine(foundObject.name, currentHiddenObject.Description, true);
				MinigameIsWon = true;
			}
		}
		else
		{
			if (MinigameIsWon)
			{
				TextUpdater.Instance.CallUpdateTextCoroutine(foundObject.name, currentHiddenObject.Description);
				return;
			}

			TextUpdater.Instance.CallUpdateTextCoroutine(foundObject.name, OBJECT_ALREADY_FOUND_MESSAGE);
		}
	}

	public void NextRoom()
	{
		if (lastSelectedObjectSprite == null)
		{
			TextUpdater.Instance.CallUpdateTextCoroutine(WARNING, WARNING_EXPLINATION);
			return;
		}
		Debug.Log("qUICK cHECK");
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

	private void ExplanationDialogue(string knotName)
	{
		DialogueManager.Instance.CurrentDialogue.SetCurrentKnot(knotName);
		//When done invoke "explanation done" event.
	}

	//This need to listen to "explanation done" event.
	private void ProgressDialogue()
	{
		if (foundObjects.Count <= 7)
		{
			DialogueManager.Instance.CurrentDialogue.SetCurrentKnot($"Part{currentDialoguePart}");
			MenuManager.Instance.OpenMenu<DialogueMenu>();
			++currentDialoguePart;
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
	}
}