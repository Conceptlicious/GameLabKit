using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class ActivePatternManager : Manager<ActivePatternManager>
{
	private const string KNOT_NAME = "Part2";

	[SerializeField] private List<NodePattern> patterns;
	[SerializeField] private NodePattern activePattern;
	private int amount = 0;
	[HideInInspector] public bool isWon = false;

	protected override void Awake()
	{
		base.Awake();

		EventManager.Instance.AddListener<DialogueChoiceSelectedEvent>(OnDialogueChoiceSelected);
	}

	public void NextActivePattern(NodePattern newPattern)
	{
		if (!patterns.Contains(newPattern))
		{
			return;
		}

		activePattern = newPattern;
		amount++;

		if (amount != 1)
		{
			return;
		}

		DialogueManager.Instance.CurrentDialogue.CurrentKnot = KNOT_NAME;
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}

	public bool IsActivePattern(NodePattern pattern)
	{
		return pattern == activePattern;
	}

	private void OnDialogueChoiceSelected(DialogueChoiceSelectedEvent eventData)
	{
		if(eventData.DialogueChoice.text != "Yes" || DialogueManager.Instance.CurrentRoomID != RoomType.ArtStyle)
		{
			return;
		}

		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.ArtStyle));
		EventManager.Instance.RaiseEvent(new NextRoomEvent());
	}
}