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
	[SerializeField] private GameObject toggledGameObject;
	private bool gameObjectToggled = false;

	public void MenuToggle()
	{
		if (gameObjectToggled)
		{
			toggledGameObject.gameObject.SetActive(false);
			gameObjectToggled = false;
		}
		else
		{
			toggledGameObject.gameObject.SetActive(true);
			gameObjectToggled = true;
		}
	}
}
