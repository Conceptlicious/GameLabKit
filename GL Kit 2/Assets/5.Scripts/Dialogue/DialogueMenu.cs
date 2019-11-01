using GameLab;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMenu : Menu
{
	[SerializeField] private Button continueButton = null;
	[SerializeField] private TextMeshProUGUI dialogueText = null;
	[SerializeField] private ContentAnimation contentAnimation = null;

	protected override void OnOpened()
	{
		contentAnimation.OnAnimationIsPlaying += OnIsContentAnimationPlaying;

		continueButton.onClick.AddListener(RequestNextDialogueLine);
		EventManager.Instance.AddListener<DialogueChoiceSelectedEvent>(OnDialogueChoiceSelected);
		EventManager.Instance.AddListener<DialogueKnotCompletedEvent>(Close, 1000);

		RequestNextDialogueLine();
	}

	protected override void OnClosed()
	{
		if(contentAnimation != null)
		{
			contentAnimation.OnAnimationIsPlaying -= OnIsContentAnimationPlaying;
		}

		continueButton.onClick.RemoveListener(RequestNextDialogueLine);
		EventManager.InstanceIfInitialized?.RemoveListener<DialogueChoiceSelectedEvent>(OnDialogueChoiceSelected);
		EventManager.InstanceIfInitialized?.RemoveListener<DialogueKnotCompletedEvent>(Close);
	}

	private void OnDialogueChoiceSelected()
	{
		DestroyAllMenuListings();
		RequestNextDialogueLine();
	}

	private void RequestNextDialogueLine()
	{
		RequestNextDialogueLineEvent nextDialogueLineRequest = new RequestNextDialogueLineEvent();
		EventManager.Instance.RaiseEvent(nextDialogueLineRequest);

		if(!nextDialogueLineRequest.Consumed)
		{
			Debug.LogWarning("Nothing is listening or consuming the dialogue line request event");
			return;
		}

		if (nextDialogueLineRequest.KnotCompleted)
		{
			return;
		}

		DestroyAllMenuListings();

		foreach (Choice choice in nextDialogueLineRequest.Choices)
		{
			CreateNewListing<DialogueChoiceMenuListing>().DialogueChoice = choice;
		}

		SetDialogueLine(nextDialogueLineRequest.NextDialogueLine);
	}

	public void SetDialogueLine(string dialogueLine)
	{
		dialogueText.text = dialogueLine;
	}

	private void OnIsContentAnimationPlaying(bool isPlaying)
	{
		continueButton.interactable = isPlaying ? false : true;
	}

	private void OnValidate()
	{
		if(continueButton == null)
		{
			continueButton = ContentContainer.Find("ContinueButton").GetComponent<Button>();
		}

		if(dialogueText == null)
		{
			dialogueText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
		}

		if (contentAnimation == null)
		{
			contentAnimation = GetComponentInChildren<ContentAnimation>();
		}
	}
}
