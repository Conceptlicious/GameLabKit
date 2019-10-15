using GameLab;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueManager : Manager<DialogueManager>
{
	public const string IntroKnotName = "Intro";
	public const string OutroKnotName = "Outro";

	public RoomStory CurrentDialogue { get; private set; } = null;

	[SerializeField] private List<RoomStory> roomStories = new List<RoomStory>();

	protected override void Awake()
	{
		base.Awake();

		List<RoomStory> originalRoomStories = new List<RoomStory>(roomStories);
		roomStories.Clear();

		foreach(RoomStory originalRoomStory in originalRoomStories)
		{
			roomStories.Add(RuntimeScriptableObject.CreateInstanceFromAsset(originalRoomStory));
		}
	}

	private void Update()
	{
		for(int i = 0; i < 9; ++i)
		{
			if(Input.GetKeyDown((KeyCode)(i + 49)))
			{
				SetCurrentDialogue((RoomType)i);
				CurrentDialogue.Reset(IntroKnotName);
				MenuManager.Instance.CloseMenu<DialogueMenu>();
				MenuManager.Instance.OpenMenu<DialogueMenu>();
			}
		}
	}

	private void OnEnable()
	{
		EventManager.Instance.AddListener<RequestToPlayKnotEvent>(OnRequestToPlayKnot);
		EventManager.Instance.AddListener<RequestNextDialogueLineEvent>(OnRequestNextDialogueLine, 100);
	}

	private void OnDisable()
	{
		EventManager.InstanceIfInitialized?.RemoveListener<RequestToPlayKnotEvent>(OnRequestToPlayKnot);
		EventManager.InstanceIfInitialized?.RemoveListener<RequestNextDialogueLineEvent>(OnRequestNextDialogueLine);
	}

	private void OnRequestToPlayKnot(RequestToPlayKnotEvent playKnotEvent) => SetCurrentDialogue(playKnotEvent.RoomID, playKnotEvent.KnotToPlay);
	private void OnRequestNextDialogueLine(RequestNextDialogueLineEvent nextDialogueLineEvent)
	{
		nextDialogueLineEvent.NextDialogueLine = CurrentDialogue.GetNextLine();
		nextDialogueLineEvent.DialogueCompleted = nextDialogueLineEvent.NextDialogueLine == null;

		nextDialogueLineEvent.Consume();
	}

	/// <summary>
	/// Sets the new current dialogue.
	/// </summary>
	/// <param name="roomID">The ID of the room for which to choose a dialogue</param>
	/// <param name="knotToStartFrom">The dialogue knot from which the dialogue should start</param>
	/// <param name="resetDialogue">Whether to reset the dialogue to the beginning or not</param>
	/// <remarks>
	/// If the requested new dialogue and the current dialogue are the same, this method does nothing.
	/// </remarks>
	public void SetCurrentDialogue(RoomType roomID, string knotToStartFrom = IntroKnotName, bool resetDialogue = true)
	{
		RoomStory roomStoryFromID = roomStories.GetStoryByID(roomID);

		if(roomStoryFromID == null)
		{
			return;
		}

		if(CurrentDialogue == roomStoryFromID)
		{
			return;
		}

		CurrentDialogue = roomStoryFromID;

		if(resetDialogue)
		{
			CurrentDialogue.Reset();
		}

		CurrentDialogue.SetCurrentKnot(knotToStartFrom);
	}
}
