using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Mathias
//Overview: This script is used in the second room, it gets called in the HiddenObjectHandler. 
//It sets the text of the clicked object.
//Usage: Once in room two on the UI object.
//--------------------------------------------------

public class TextUpdater : Singleton<TextUpdater>
{
	[SerializeField] private const int TIME_OBJECT_INFO_SHOWN = 10;
	[SerializeField] private const int TIME_LAST_OBJECT_INFO_SHOWN = 5;
	private const string WON_MINIGAME_MESSAGE_NAME = "You won the mini-game!";
	private const string WON_MINIGAME_MESSAGE_DESCRIPTION = "Select the item you want to take to the White Room";

	private Text foundObjectNameText = null;
	private Text foundObjectDescriptionText = null;
	private IEnumerator runningCoroutine = null;

	private void Start()
	{
		SetValues();
	}

	public void CallUpdateTextCoroutine(string foundObjectName, string foundObjectDescription, bool lastHiddenObjectFound = false)
	{
		StopAllCoroutines();

		if (HiddenObjectHandler.Instance.MinigameIsWon)
		{
			foundObjectNameText.text = foundObjectName;
			foundObjectDescriptionText.text = foundObjectDescription;
			return;
		}

		StartCoroutine(UpdateText(foundObjectName, foundObjectDescription, lastHiddenObjectFound));
	}

	private IEnumerator UpdateText(string foundObjectName, string foundObjectDescription, bool lastHiddenObjectFound)
	{
		foundObjectNameText.text = foundObjectName;
		foundObjectDescriptionText.text = foundObjectDescription;

		if (lastHiddenObjectFound)
		{
			yield return new WaitForSeconds(TIME_LAST_OBJECT_INFO_SHOWN);

			foundObjectNameText.text = WON_MINIGAME_MESSAGE_NAME;
			foundObjectDescriptionText.text = WON_MINIGAME_MESSAGE_DESCRIPTION;
			yield break;
		}

		yield return new WaitForSeconds(TIME_OBJECT_INFO_SHOWN);

		foundObjectNameText.text = string.Empty;
		foundObjectDescriptionText.text = string.Empty;
	}

	private void SetValues()
	{
		foundObjectNameText = transform.Find("ObjectNameText").GetComponent<Text>();
		foundObjectDescriptionText = transform.Find("ObjectDescriptionText").GetComponent<Text>();

		foundObjectNameText.text = string.Empty;
		foundObjectDescriptionText.text = string.Empty;
	}
}