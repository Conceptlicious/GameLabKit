using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class DisplayMediumInformationManager : Manager<DisplayMediumInformationManager>
{
	private Text informationDisplay = null;
	private const string BEGIN_MESSAGE = "Select a medium to see the pros and cons";

	private void Start()
	{
		informationDisplay = GetComponent<Text>();
		informationDisplay.text = BEGIN_MESSAGE;
	}

	public void SetDisplayText(string newText)
	{
		informationDisplay.text = newText;
	}
}