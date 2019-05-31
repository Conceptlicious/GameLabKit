using System.Collections.Generic;
using UnityEngine.UI;
using GameLab;

public class ToggleManager : Singleton<ToggleManager>
{
	private const int maxOnTogglesPerList = 3;

	private List<Toggle> education = new List<Toggle>();
	private List<Toggle> specialNeeds = new List<Toggle>();
	private List<Disabilities> disabilitiesList = new List<Disabilities>();

	public void AddToggleToList(Toggle toggleToAdd, ToggleGroup toggleGroup, Disabilities disabilities)
	{
		if (toggleGroup == ToggleGroup.Education && education.Count < maxOnTogglesPerList)
		{
			education.Add(toggleToAdd);
		}
		else if (toggleGroup == ToggleGroup.SpecialNeeds && specialNeeds.Count < maxOnTogglesPerList)
		{
			specialNeeds.Add(toggleToAdd);
			disabilitiesList.Add(disabilities);
			UserInterfaceHandler.Instance.SetPersonaDisabilities(disabilitiesList);
		}
		else
		{
			toggleToAdd.isOn = false;
		}
	}

	public void RemoveToggleFromList(Toggle toggleToRemove, Disabilities disabilitiesToRemove)
	{
		if (education.Contains(toggleToRemove))
		{
			education.Remove(toggleToRemove);
		}
		else
		{
			specialNeeds.Remove(toggleToRemove);
			disabilitiesList.Remove(disabilitiesToRemove);
			UserInterfaceHandler.Instance.SetPersonaDisabilities(disabilitiesList);
		}
	}	
}