using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;
using UnityEditor;
using UnityEngine.UI;

public class SettingsMenu : BetterMonoBehaviour
{
	/*
	Just a simple script to help buttons toggle the settings menu on and off
	*/
	[SerializeField] private Canvas settingsCanvas;
	private bool menuToggled = false;

	public void MenuToggle()
	{
		if (menuToggled)
		{			
			settingsCanvas.gameObject.SetActive(false);
			menuToggled = false;
		}
		else
		{
			settingsCanvas.gameObject.SetActive(true);
			menuToggled = true;
		}
	}
}
