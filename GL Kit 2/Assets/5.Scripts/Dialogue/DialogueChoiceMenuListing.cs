using GameLab;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DialogueChoiceMenuListing : MenuListing
{
	private Button button = null;
	private TextMeshProUGUI text = null;
	private Choice dialogueChoice = null;
	public Choice DialogueChoice
	{
		get => dialogueChoice;
		set
		{
			dialogueChoice = value;
			Refresh();
		}
	}

	private void Awake()
	{
		button = GetComponent<Button>();
		text = button.GetComponentInChildren<TextMeshProUGUI>();
	}

	protected override void Refresh()
	{
		text.text = DialogueChoice.text;
	}

	protected override void RegisterToEvents()
	{
		button.onClick.AddListener(OnClick);
	}

	private void OnClick()
	{
		EventManager.Instance.RaiseEvent<DialogueChoiceSelectedEvent>(dialogueChoice);
	}

	protected override void UnregisterFromEvents()
	{
		button.onClick.RemoveAllListeners();
	}
}
