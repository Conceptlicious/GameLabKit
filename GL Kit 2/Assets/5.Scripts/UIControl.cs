using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventType = CustomEventCallbacks.EventType;
using GameLab;


public class UIControl : MonoBehaviour
{
	private Persona persona = new Persona();

	private const int ENABLED_TOGGLES_PER_LIST_MAX = 3;
	public event Action<Persona> OnPersonaChanged;

	[SerializeField] private int teenCutOff = 13;
	[SerializeField] private int adultCutOff = 20;
	[SerializeField] private int elderlyCutOff = 60;
	[SerializeField] private Transform ageSliders = null;
	[SerializeField] private Transform genderToggles = null;
	private Slider ageSilderMin = null;
	private Slider ageSilderMax = null;
	private Slider AverageAgeSlider = null;
	private Text targetAudienceText = null;
	private Text minAgeText = null;
	private Text maxAgeText = null;
	private Toggle unspecified = null;
	private Toggle female = null;
	private Toggle male = null;
	private List<Toggle> activeEducationToggles = new List<Toggle>();
	private List<Toggle> activeSpecialNeedToggles = new List<Toggle>();
	private List<Disabilities> disabilitiesList = new List<Disabilities>();

	private void Start()
	{
		SetVariables();
	}

	public void AddToggleToList(Toggle toggleToAdd, ToggleGroup toggleGroup, Disabilities disabilities)
	{
		if (toggleGroup == ToggleGroup.Education && activeEducationToggles.Count < ENABLED_TOGGLES_PER_LIST_MAX)
		{
			activeEducationToggles.Add(toggleToAdd);
		}
		else if (toggleGroup == ToggleGroup.SpecialNeeds && activeSpecialNeedToggles.Count < ENABLED_TOGGLES_PER_LIST_MAX)
		{
			activeSpecialNeedToggles.Add(toggleToAdd);
			disabilitiesList.Add(disabilities);
			SetPersonaDisabilities(disabilitiesList);
		}
		else
		{
			toggleToAdd.isOn = false;
		}
	}

	public void RemoveToggleFromList(Toggle toggleToRemove, Disabilities disabilitiesToRemove)
	{
		if (activeEducationToggles.Contains(toggleToRemove))
		{
			activeEducationToggles.Remove(toggleToRemove);
		}
		else
		{
			activeSpecialNeedToggles.Remove(toggleToRemove);
			disabilitiesList.Remove(disabilitiesToRemove);
			SetPersonaDisabilities(disabilitiesList);
		}
	}

	public void UpdateAgeSlider()
	{
		float calculateAverageAge = (ageSilderMin.value + ageSilderMax.value) / 2;

		int averageAge = Mathf.RoundToInt(calculateAverageAge);

		if (averageAge >= elderlyCutOff)
		{
			targetAudienceText.text = $"Eldery\n{averageAge}+";
			persona.Age = AgeGroup.Elderly;
		}
		else if (averageAge >= adultCutOff)
		{
			targetAudienceText.text = $"Adult\n{averageAge}";
			persona.Age = AgeGroup.Adult;
		}
		else if (averageAge >= teenCutOff)
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

	public void GenderTogglePressed(Genders gender, bool isEnabled)
	{
		if (isEnabled)
		{
			persona.Gender = gender;
		}

		OnPersonaChanged?.Invoke(persona);
	}

	public void TogglePressed(Toggle pressedToggle)
	{
		if (pressedToggle.isOn)
		{
			ToggleInformation currentToggleInformation = pressedToggle.GetComponent<ToggleInformation>();
			AddToggleToList(pressedToggle, currentToggleInformation.toggleGroup, currentToggleInformation.disabilities);
		}
		else
		{
			ToggleInformation currentToggleInformation = pressedToggle.GetComponent<ToggleInformation>();
			RemoveToggleFromList(pressedToggle, currentToggleInformation.disabilities);
		}
	}

	public void SetPersonaDisabilities(List<Disabilities> disabilitiesList)
	{
		foreach (Disabilities disability in disabilitiesList)
		{
			persona.Disability |= disability;
		}
		OnPersonaChanged?.Invoke(persona);
	}

	public void SetVariables()
	{
		ageSilderMin = ageSliders.Find("AgeSliderMin").GetComponent<Slider>();
		ageSilderMax = ageSliders.Find("AgeSliderMax").GetComponent<Slider>();
		AverageAgeSlider = ageSliders.Find("AverageAge").GetComponent<Slider>();

		ageSilderMax.onValueChanged.AddListener((_) => UpdateAgeSlider());
		ageSilderMin.onValueChanged.AddListener((_) => UpdateAgeSlider());

		targetAudienceText = ageSilderMin.transform.Find("TargetAudicence").GetComponent<Text>();
		minAgeText = ageSilderMin.transform.Find("Handle Slide Area/Handle/Text").GetComponent<Text>();
		maxAgeText = ageSilderMax.transform.Find("Handle Slide Area/Handle/Text").GetComponent<Text>();

		UpdateAgeSlider();

		unspecified = genderToggles.Find("Unspecified").GetComponent<Toggle>();
		female = genderToggles.Find("Female").GetComponent<Toggle>();
		male = genderToggles.Find("Male").GetComponent<Toggle>();

		unspecified.onValueChanged.AddListener((isEnabled) => GenderTogglePressed(Genders.Unspecified,
			isEnabled));
		female.onValueChanged.AddListener((isEnabled) => GenderTogglePressed(Genders.Female, 
			isEnabled));
		male.onValueChanged.AddListener((isEnabled) => GenderTogglePressed(Genders.Male, isEnabled));
	}

	public void DoneButton()
	{
		SaveItemEvent saveItemEvent = new SaveItemEvent(RoomType.TargetAudience, this);
		EventManager.Instance.RaiseEvent(saveItemEvent);

		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}

	public void FocusNextRoom()
	{
		NextRoomEvent newInfo = new NextRoomEvent();
		//EventSystem.ExecuteEvent(EventType.UI_NEXT_ROOM, newInfo);
		EventManager.Instance.RaiseEvent(newInfo);
	}

	public void ProgressDialogue()
	{
		Debug.Log("Touch");
		if (SpeechBubble.Instance.DiagObject != null)
		{
			//If we have looped back to the start after an iteration
			if (SpeechBubble.Instance.DiagObject.Info.fieldIndex == 0)
			{
				FillSpeechBubbleEvent repeatInfo = new FillSpeechBubbleEvent(null, Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, SpeechBubble.FillTextMethod.NONE, true);
				EventManager.Instance.RaiseEvent(repeatInfo);
			}
			else
			{
				FillSpeechBubbleEvent newInfo = new FillSpeechBubbleEvent(null, Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, SpeechBubble.FillTextMethod.ITERATE, false);
				EventManager.Instance.RaiseEvent(newInfo);

				Debug.Log("Field Index: " + SpeechBubble.Instance.DiagObject.Info.fieldIndex);
			}
		}

	}

	public void PopUpWithParam(DialogueObject pDialogueObject)
	{
		CreateSpesifiedPopUpEvent newInfo = new CreateSpesifiedPopUpEvent(pDialogueObject);

	}

}
