using System.Collections.Generic;
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

	private void Start()
	{
		SetVariables();
	}

	public void UpdateAgeSlider()
	{
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


		minAgeText.text = ageSilderMin.value.ToString();
		maxAgeText.text = ageSilderMax.value.ToString();

		AverageAgeSlider.value = averageAge;
		OnPersonaChanged?.Invoke(persona);
	}

	public void GenderTogglePressed(Toggle pressedToggle)
	{
		if(pressedToggle.name == "Unspecified")
		{
			persona.Gender = Genders.Unspecified;
		}
		if (pressedToggle.name == "Female")
		{
			persona.Gender = Genders.Female;
		}
		if (pressedToggle.name == "Male")
		{
			persona.Gender = Genders.Male;
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
			ToggleInformation currentToggleInformation = pressedToggle.GetComponent<ToggleInformation>();
			ToggleManager.Instance.RemoveToggleFromList(pressedToggle, currentToggleInformation.disabilities);
		}
	}

	public void SetPersonaDisabilities(List<Disabilities> disabilitiesList)
	{

		foreach(Disabilities disabilities in disabilitiesList)
		{
			foreach(Disabilities disability in disabilitiesList)
			{
				persona.Disability = persona.Disability | disability;
			}
			OnPersonaChanged?.Invoke(persona);
		}		
	}

	public void SetVariables()
	{
		Transform ageSliders = transform.Find("AgeSliders");

		ageSilderMin = ageSliders.Find("AgeSliderMin").GetComponent<Slider>();
		ageSilderMax = ageSliders.Find("AgeSliderMax").GetComponent<Slider>();
		AverageAgeSlider = ageSliders.Find("AverageAge").GetComponent<Slider>();

		ageSilderMax.onValueChanged.AddListener((_) => UpdateAgeSlider());
		ageSilderMin.onValueChanged.AddListener((_) => UpdateAgeSlider());

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