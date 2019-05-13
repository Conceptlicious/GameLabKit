using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class UserInterfaceHandler : Singleton<UserInterfaceHandler>
{
	private Slider ageSilderMin = null;
	private Slider ageSilderMax = null;
	private Slider AverageAgeSlider = null;
	private Text targetAudienceText = null;
	
	public void UpdateAgeSlider()
	{
		float calculateAverageAge = (ageSilderMin.value + ageSilderMax.value) / 2;
		int averageAge = Mathf.RoundToInt(calculateAverageAge);

		if (averageAge == 60)
		{
			targetAudienceText.text = $"Eldery\n{averageAge}+";
		}
		else if (averageAge >= 20)
		{
			targetAudienceText.text = $"Adult\n{averageAge}";
		}
		else if (averageAge >= 13)
		{
			targetAudienceText.text = $"Teenager\n{averageAge}";
		}
		else
		{
			targetAudienceText.text = $"Child\n{averageAge}";
		}

		AverageAgeSlider.value = averageAge;
	}

	public void TogglePressed(Toggle pressedToggle)
	{
		if(pressedToggle.isOn)
		{
			ToggleInformation currentToggleInformation = pressedToggle.GetComponent<ToggleInformation>();
			ToggleManager.Instance.AddToggleToList(pressedToggle, currentToggleInformation.toggleGroup);
		}
		else
		{
			ToggleManager.Instance.RemoveToggleFromList(pressedToggle);
		}
	}

	public void SetVariables()
	{
		ageSilderMin = transform.Find("AgeSliders/AgeSliderMin").GetComponent<Slider>();
		ageSilderMax = transform.Find("AgeSliders/AgeSliderMax").GetComponent<Slider>();
		AverageAgeSlider = transform.Find("AgeSliders/AverageAge").GetComponent<Slider>();
		targetAudienceText = ageSilderMin.GetComponentInChildren<Text>();

		UpdateAgeSlider();
	}
}