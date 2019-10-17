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

	protected override void OnOpened()
	{
		base.OnOpened();

		continueButton.onClick.AddListener(RequestNextDialogueLine);
		EventManager.Instance.AddListener<RequestMakeDialogueChoiceEvent>(OnRequestMakeDialogueChoiceEvent);

		RequestNextDialogueLine();
	}

	protected override void OnClosed()
	{
		base.OnClosed();

		continueButton.onClick.RemoveListener(RequestNextDialogueLine);
	}

	private void OnRequestMakeDialogueChoiceEvent()
	{
		DestroyAllMenuListings();
	}

	private void RequestNextDialogueLine()
	{
		RequestNextDialogueLineEvent nextDialogueLineRequest = new RequestNextDialogueLineEvent();
		EventManager.Instance.RaiseEvent<RequestNextDialogueLineEvent>(nextDialogueLineRequest);
		if(!nextDialogueLineRequest.Consumed)
		{
			Debug.LogWarning("Nothing is listening or consuming the dialogue line request event");
			return;
		}
		DestroyAllMenuListings();
		foreach (Choice choice in nextDialogueLineRequest.Choices)
		{
			CreateNewListing<DialogueChoiceMenuListing>().DialogueChoice = choice;
		}

		if (nextDialogueLineRequest.DialogueCompleted || nextDialogueLineRequest.NextDialogueLine == "")
		{
			Close();
		}
		else
		{
			SetDialogueLine(nextDialogueLineRequest.NextDialogueLine);
		}
	}

	public void SetDialogueLine(string dialogueLine)
	{
		dialogueText.text = dialogueLine;
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
	}
}
