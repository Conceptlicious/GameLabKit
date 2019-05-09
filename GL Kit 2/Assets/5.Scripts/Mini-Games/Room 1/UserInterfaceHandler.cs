using UnityEngine.UI;
using GameLab;

public class UserInterfaceHandler : Singleton<UserInterfaceHandler>
{
	Slider ageSilder = null;
	Text targetAudienceText = null;


	private void Start()
	{
		SetVariables();
		UpdateAgeSlider();
	}

	public void UpdateAgeSlider()
	{
		int age = (int)ageSilder.value; 

		if (age == 60)
		{
			targetAudienceText.text = $"Eldery\n{ageSilder.value}+";
		}
		else if (age >= 20)
		{
			targetAudienceText.text = $"Adult\n{ageSilder.value}";
		}
		else if (age >= 13)
		{
			targetAudienceText.text = $"Teenager\n{ageSilder.value}";
		}
		else
		{
			targetAudienceText.text = $"Child\n{ageSilder.value}";
		}
	}

	public void TogglePressed(Toggle pressedToggle)
	{
		if(pressedToggle.isOn)
		{
			string toggleGroup = pressedToggle.GetComponent<ToggleInformation>().ToggleGroup;
			ToggleManager.Instance.AddToggleToList(pressedToggle, toggleGroup);
		}
		else
		{
			ToggleManager.Instance.RemoveToggleFromList(pressedToggle);
		}
	}

	private void SetVariables()
	{
		ageSilder = GetComponentInChildren<Slider>();
		targetAudienceText = ageSilder.GetComponentInChildren<Text>();
	}
}