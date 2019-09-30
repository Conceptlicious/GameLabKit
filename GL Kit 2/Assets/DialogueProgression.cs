﻿using UnityEngine;
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

		StartStory();
	}

	private void OnEnable()
	{
		EventManager.Instance.AddListener<NextRoomEvent>(OnNextRoom);
	}

	private void OnDisable()
	{
		EventManager.InstanceIfInitialized?.RemoveListener<NextRoomEvent>(OnNextRoom);
	}

	void StartStory ()
	{
		story = new Story (roomStoryFiles[0].RoomInkFile.text);
		story.ChoosePathString("FromRoom1");
		RefreshView();
	}

	private void OnNextRoom()
	{
		Vector3Int currentRoom = RoomManager.Instance.GetRoomIDs();

		story = new Story(roomStoryFiles.First(storyFile => storyFile.RoomID == currentRoom.z).RoomInkFile.text);

		if (currentRoom.z == RoomManager.Instance.WhiteRoomID)
		{
			string knotName = $"FromRoom{RoomManager.Instance.GetRoomIDs().y}";
			story.ChoosePathString(knotName);
		}

		RefreshView();
	}

	void RefreshView ()
	{
		while (story.canContinue)
		{
			string text = story.Continue();

			text = text.Trim();

			storyText = text;
		}

		if(story.currentChoices.Count > 0)
		{
			for (int i = 0; i < story.currentChoices.Count; i++)
			{
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim());

				button.onClick.AddListener (delegate{OnClickChoiceButton(choice);});
			}
		}

		else
		{
			Button choice = CreateChoiceView(" ");
			choice.onClick.AddListener(delegate{RemoveChildren();});
		}
	}

	void OnClickChoiceButton (Choice choice)
	{
		Debug.Log(storyText.Length);
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	Button CreateChoiceView (string text)
	{
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);

		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = storyText;

		return choice;
	}

	void RemoveChildren ()
	{
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i)
		{
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}
}