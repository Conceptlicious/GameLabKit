using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class WhiteRoomDialogueManager : Manager<WhiteRoomDialogueManager>
{
	private const string KNOT_BASE_STRING = "FromRoom";

	private int previousRoomID = 0;

	protected override void Awake()
	{
		base.Awake();

		EventManager.Instance.AddListener<FinishedRoomTransition>(OnFinishedRoomTransition);
		EventManager.Instance.AddListener<DialogueKnotCompletedEvent>(OnDialogueKnotCompleted);
	}

	private IEnumerator Start()
	{
		yield return null;
		DialogueManager.Instance.SetCurrentDialogue(RoomType.FrontDoor);
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}

	private void OnFinishedRoomTransition(FinishedRoomTransition eventData)
	{
		if (RoomManager.Instance.GetCurrentRoomID().z != 7)
		{
			return;
		}

		previousRoomID = RoomManager.Instance.GetCurrentRoomID().y;

		if (previousRoomID == 0)
		{
			DialogueManager.Instance.SetCurrentDialogue(RoomType.WhiteRoom);
			MenuManager.Instance.OpenMenu<DialogueMenu>();
		}
		else
		{
			DialogueManager.Instance.SetCurrentDialogue(RoomType.WhiteRoom, $"{KNOT_BASE_STRING}{previousRoomID}");
			MenuManager.Instance.OpenMenu<DialogueMenu>();
		}
	}

	private void OnDialogueKnotCompleted(DialogueKnotCompletedEvent eventData)
	{
		if (eventData.CompletedRoomID != RoomType.WhiteRoom && eventData.CompletedRoomID != RoomType.FrontDoor || eventData.Knot != "Intro" && eventData.Knot != "FromRoom1")
		{
			return;
		}

		EventManager.Instance.RaiseEvent(new NextRoomEvent());
	}
}