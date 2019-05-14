using System.Collections.Generic;
using UnityEngine.UI;
using GameLab;

public class ToggleManager : Singleton<ToggleManager>
{
	private List<Toggle> education = new List<Toggle>();
	private List<Toggle> specialNeeds = new List<Toggle>();
	private int maxOnTogglesPerList = 3;

	public void AddToggleToList(Toggle toggleToAdd, ToggleGroup toggleGroup)
	{
		if (toggleGroup == ToggleGroup.Education && education.Count < maxOnTogglesPerList)
		{
			education.Add(toggleToAdd);
		}
		else if (toggleGroup == ToggleGroup.SpecialNeeds && specialNeeds.Count < maxOnTogglesPerList)
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