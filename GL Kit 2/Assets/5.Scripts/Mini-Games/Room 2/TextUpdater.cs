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
	private Text foundObjectNameText = null;
	private Text foundObjectDisciptionText = null;
	[SerializeField] private const int timeObjectInformationShown = 10;

	private void Start()
	{
		SetValues();
	}

	public void CallUpdateTextCoroutine(string foundObjectName, string foundObjectDisciption)
	{
		StartCoroutine(UpdateText(foundObjectName, foundObjectDisciption));
	}

	private IEnumerator UpdateText(string foundObjectName, string foundObjectDisciption)
	{
		foundObjectNameText.text = foundObjectName;
		foundObjectDisciptionText.text = foundObjectDisciption;

		yield return new WaitForSeconds(timeObjectInformationShown);

		foundObjectNameText.text = string.Empty;
		foundObjectDisciptionText.text = string.Empty;
	}

	private void SetValues()
	{
		foundObjectNameText = transform.Find("ObjectNameText").GetComponent<Text>();
		foundObjectDisciptionText = transform.Find("ObjectDescriptionText").GetComponent<Text>();

		foundObjectNameText.text = string.Empty;
		foundObjectDisciptionText.text = string.Empty;
	}
}