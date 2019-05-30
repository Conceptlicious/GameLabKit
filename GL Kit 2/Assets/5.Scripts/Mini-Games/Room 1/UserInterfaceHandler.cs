using UnityEngine.UI;
using UnityEngine;
using GameLab;
using System;

public class UserInterfaceHandler : Singleton<UserInterfaceHandler>
{
	public event Action<Persona> OnPersonaChanged;

	private Persona persona = new Persona();

	private Slider ageSilderMin = null;
	private Slider ageSilderMax = null;
	private Slider AverageAgeSlider = null;
	private Text targetAudienceText = null;
	private Text minAgeText = null;
	private Text maxAgeText = null;
	
	public void UpdateAgeSlider()
	{
		minAgeText.text = ageSilderMin.value.ToString();
		maxAgeText.text = ageSilderMax.value.ToString();

		float calculateAverageAge = (ageSilderMin.value + ageSilderMax.value) / 2;
		int averageAge = Mathf.RoundToInt(calculateAverageAge);

		if (averageAge >= 60)
		{
			targetAudienceText.text = $"Eldery\n{averageAge}+";
			persona.Age = AgeGroup.Elderly;
		}
		else if (averageAge >= 20)
		{
			targetAudienceText.text = $"Adult\n{averageAge}";
			persona.Age = AgeGroup.Adult;
		}
		else if (averageAge >= 13)
		{
			targetAudienceText.text = $"Teenager\n{averageAge}";
			persona.Age = AgeGroup.Teenager;
		}
		else
		{
			targetAudienceText.text = $"Child\n{averageAge}";
			persona.Age = AgeGroup.Child;
		}

		AverageAgeSlider.value = averageAge;
		OnPersonaChanged?.Invoke(persona);
	}

	public void GenderTogglePressed(Toggle pressedToggle)
	{
		if(pressedToggle.name == "Unspecified")
		{
			persona.Gender = Gender.Unspecified;
		}
		if (pressedToggle.name == "Female")
		{
			persona.Gender = Gender.Female;
		}
		if (pressedToggle.name == "Male")
		{
			persona.Gender = Gender.Male;
		}

		OnPersonaChanged?.Invoke(persona);
	}

	public void TogglePressed(Toggle pressedToggle)
	{
		if(pressedToggle.isOn)
		{
			ToggleInformation currentToggleInformation = pressedToggle.GetComponent<ToggleInformation>();
			ToggleManager.Instance.AddToggleToList(pressedToggle, 
				currentToggleInformation.toggleGroup, currentToggleInformation.disabilities);
		}
		else
		{
			ToggleManager.Instance.RemoveToggleFromList(pressedToggle);
		}
	}

	public void SetPersonaDisabilities(Disabilities disabilities)
	{
		persona.Disability = disabilities;
		OnPersonaChanged?.Invoke(persona);
	}

	public void SetVariables()
	{
		ageSilderMin = transform.Find("AgeSliders/AgeSliderMin").GetComponent<Slider>();
		ageSilderMax = transform.Find("AgeSliders/AgeSliderMax").GetComponent<Slider>();
		AverageAgeSlider = transform.Find("AgeSliders/AverageAge").GetComponent<Slider>();

		targetAudienceText = ageSilderMin.transform.Find("TargetAudicence").GetComponent<Text>();
		minAgeText = ageSilderMin.transform.Find("Handle Slide Area/Handle/Text").GetComponent<Text>();
		maxAgeText = ageSilderMax.transform.Find("Handle Slide Area/Handle/Text").GetComponent<Text>();
		UpdateAgeSlider();
	}

	public void DoneButton()
	{
		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}
}