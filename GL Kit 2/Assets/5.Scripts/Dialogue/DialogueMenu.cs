using GameLab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMenu : Menu
{
	[SerializeField] private Button continueButton = null;
	[SerializeField] private TextMeshProUGUI dialogueText = null;

	private void OnEnable()
	{
		DialogueManager.Instance.DialogueStarted += Open;
		DialogueManager.Instance.DialogueEnded += Close;
	}

	private void OnDisable()
	{
		DialogueManager.Instance.DialogueStarted -= Open;
		DialogueManager.Instance.DialogueEnded -= Close;
	}

	protected override void OnOpened()
	{
		base.OnOpened();

		DialogueManager.Instance.DialogueContinued += SetDialogueLine;
	}

	protected override void OnClosed()
	{
		base.OnClosed();

		DialogueManager.Instance.DialogueContinued -= SetDialogueLine;
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
