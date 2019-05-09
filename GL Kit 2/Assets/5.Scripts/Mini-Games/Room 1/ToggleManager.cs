using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class ToggleManager : Singleton<ToggleManager>
{
	private List<Toggle> education = new List<Toggle>();
	private List<Toggle> specialNeeds = new List<Toggle>();
	private int maxOnTogglesPerList = 3;

	public void AddToggleToList(Toggle toggleToAdd, string listName)
	{
		if (listName == "Education" && education.Count < maxOnTogglesPerList)
		{
			education.Add(toggleToAdd);
		}
		else if (listName == "Special Needs" && specialNeeds.Count < maxOnTogglesPerList)
		{
			specialNeeds.Add(toggleToAdd);
		}
		else
		{
			toggleToAdd.isOn = false;
		}
	}

	public void RemoveToggleFromList(Toggle toggleToRemove)
	{
		if (education.Contains(toggleToRemove))
		{
			education.Remove(toggleToRemove);
		}
		else
		{
			specialNeeds.Remove(toggleToRemove);
		}
	}
}