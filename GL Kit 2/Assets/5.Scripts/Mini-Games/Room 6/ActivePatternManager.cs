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

	public void NextActivePattern(NodePattern newPattern)
	{
		if (!patterns.Contains(newPattern))
		{
			return;
		}
		activePattern = newPattern;
		amount++;
		if (amount == 1)
		{
			DialogueManager.Instance.CurrentDialogue.CurrentKnot = KNOT_NAME;
			MenuManager.Instance.OpenMenu<DialogueMenu>();
		}
	}

	public bool IsActivePattern(NodePattern pattern)
	{
		return pattern == activePattern;
	}
}
