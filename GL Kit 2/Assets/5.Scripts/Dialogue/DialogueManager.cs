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

	public event Action DialogueStarted;
	public event Action<string> DialogueContinued;
	public event Action DialogueEnded;

	[SerializeField] private List<RoomStory> roomStories = new List<RoomStory>();

	protected override void Awake()
	{
		base.Awake();

		roomStories.ForEach(story => story.Initialize());
	}

	private void Start()
	{
		PlayKnot(RoomType.FrontDoor, IntroKnotName);
		GetNextLine(RoomType.FrontDoor);
	}

	private void OnRequestToPlayKnot(RequestToPlayKnotEvent playKnotEvent) => PlayKnot(playKnotEvent.RoomID, playKnotEvent.KnotToPlay);

	public void PlayKnot(RoomType roomID, string knotName)
	{
		roomStories.GetStoryByID(roomID).PlayKnot(knotName);

		DialogueStarted?.Invoke();
	}

	public string GetNextLine(RoomType roomID)
	{
		string nextDialogueLine = roomStories.GetStoryByID(roomID).GetNextLine();

		if (nextDialogueLine == null)
		{
			DialogueEnded?.Invoke();
		}
		else
		{
			DialogueContinued?.Invoke(nextDialogueLine);
		}

		return nextDialogueLine;
	}
}

public static class RoomStoryExtensions
{
	public static RoomStory GetStoryByID(this List<RoomStory> roomStories, RoomType roomID)
	{
		return roomStories.First(roomStory => roomStory.RoomID == roomID);
	}
}
