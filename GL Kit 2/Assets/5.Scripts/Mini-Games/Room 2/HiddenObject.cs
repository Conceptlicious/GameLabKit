using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Mathias Bevers
//Overview: This script contains all the info of the hidden items. All of it is set in the inspector.
//This info is later read in the hiddenItemHandler;
//Usage: On every item in the "CollectedHiddenObjectBar".
//--------------------------------------------------

[RequireComponent(typeof(MonoBehaviour))]
public class HiddenObject : BetterMonoBehaviour
{
	private const string KNOT_NAME = "Part9";

	private Button button = null;
	private Sprite sprite = null;
	private bool isFound = false;

	private void Start()
	{
		SetVariables();
	}

	public void Found()
	{
		if (isFound)
		{
			return;
		}

		button.interactable = true;
	}

	private void Clicked()
	{
		if (!HiddenObjectHandler.Instance.MinigameIsWon)
		{
			return;
		}

		DialogueManager.Instance.CurrentDialogue.Reset(KNOT_NAME);
		HiddenObjectHandler.Instance.SelectObject(name, sprite);
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}

	private void SetVariables()
	{
		sprite = GetComponent<Image>().sprite;
		button = GetComponent<Button>();

		button.onClick.AddListener(() => Clicked());

		button.interactable = false;
	}
}
