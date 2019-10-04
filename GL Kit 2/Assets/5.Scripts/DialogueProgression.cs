using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ink.Runtime;
using GameLab;
using System;
using System.Linq;

public class DialogueProgression : Singleton<DialogueProgression>
{
	[Serializable]
	public struct RoomStoryFileReference
	{
		public int RoomID;
		public TextAsset RoomInkFile;
	}

	private int currentKnot;

	[SerializeField] private RoomStoryFileReference[] roomStoryFiles;

	public Story story;

	[SerializeField]
	private Canvas canvas;

	private string storyText;

	[SerializeField]
	private Button buttonPrefab;

	protected override void Awake()
	{
		base.Awake();

		EventManager.Instance.AddListener<ProgressDialogueEvent>(OnDialogueProgress);

		EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());

		StartStory();
	}

	private void OnEnable()
	{
		EventManager.Instance.AddListener<NextRoomEvent>(RemoveChildren);
		EventManager.Instance.AddListener<FinishedRoomTransition>(OnNextRoomReached);
	}

	private void OnDisable()
	{
		EventManager.InstanceIfInitialized.RemoveListener<NextRoomEvent>(RemoveChildren);
		EventManager.InstanceIfInitialized?.RemoveListener<FinishedRoomTransition>(OnNextRoomReached);
	}

	private void StartStory()
	{
		story = new Story(roomStoryFiles[0].RoomInkFile.text);
		RoomPartHandler();
	}

	private void OnNextRoomReached()
	{
		Vector3Int currentRoom = RoomManager.Instance.GetCurrentRoomID();
		currentKnot = 1;
		ProgressDialogueEvent.ResetKnotID();

		story = new Story(roomStoryFiles.First(storyFile => storyFile.RoomID == currentRoom.z).RoomInkFile.text);

		if (currentRoom.z == RoomManager.Instance.WhiteRoomID)
		{
			string knotName = $"FromRoom{RoomManager.Instance.GetCurrentRoomID().y + 1}";
			story.ChoosePathString(knotName);
		}
		else
		{
			RoomPartHandler();
		}

		RefreshView();
	}

	public void RoomPartHandler()
	{
		Vector3Int currentRoom = RoomManager.Instance.GetCurrentRoomID();

		if (currentRoom.z < 6)
		{
			string knotName = $"Part{currentKnot}";
			story.ChoosePathString(knotName);
		}

		RefreshView();
	}

	private void RefreshView()
	{
		if(story.canContinue)
		{
			string text = story.Continue();

			text = text.Trim();

			storyText = text;
		}

		if (story.currentChoices.Count > 0)
		{
			for (int i = 0; i < story.currentChoices.Count; i++)
			{
				Choice choice = story.currentChoices[i];
				Button button = CreateChoiceView();

				button.onClick.AddListener(() => OnClickChoiceButton(choice));
			}
		}
		else
		{
			Button choice = CreateChoiceView();
			choice.onClick.AddListener(RemoveChildren);
		}
	}

	private void OnClickChoiceButton(Choice choice)
	{
		story.ChooseChoiceIndex(choice.index);
		RefreshView();
	}

	private Button CreateChoiceView()
	{
		Button choice = Instantiate(buttonPrefab) as Button;
		choice.transform.SetParent(canvas.transform, false);

		Text choiceText = choice.GetComponentInChildren<Text>();
		choiceText.text = storyText;

		choice.onClick.AddListener(DebugChoiceInfo);

		return choice;
	}

	private void DebugChoiceInfo()
	{
		Debug.Log($"currentKnot: {currentKnot}");
		Debug.Log($"story.canContinue: {story.canContinue} and choices: {story.currentChoices.Count}");
		if(RoomManager.Instance.GetCurrentRoomID().z == 0 && currentKnot == 3 && !story.canContinue && story.currentChoices.Count == 0)
		{
			NextRoomEvent newInfo = new NextRoomEvent();
			EventManager.Instance.RaiseEvent(newInfo);
		}
	}

	private void OnDialogueProgress(ProgressDialogueEvent eventData)
	{
		currentKnot = eventData.KnotID;

		if (story != null)
		{
			RoomPartHandler();
		}
	}

	public void RemoveChildren()
	{
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i)
		{
			Destroy(canvas.transform.GetChild(i).gameObject);
		}
	}
}