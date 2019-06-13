 using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventType = CustomEventCallbacks.EventType;
using GameLab;


public class UIControl : MonoBehaviour
{
	public event Action<Persona> OnPersonaChanged;

	private Persona persona = new Persona();

	private Slider ageSilderMin = null;
	private Slider ageSilderMax = null;
	private Slider AverageAgeSlider = null;
	private Text targetAudienceText = null;
	private Text minAgeText = null;
	private Text maxAgeText = null;

	private const int maxOnTogglesPerList = 3;

	private List<Toggle> education = new List<Toggle>();
	private List<Toggle> specialNeeds = new List<Toggle>();
	private List<Disabilities> disabilitiesList = new List<Disabilities>();

	[SerializeField] private int teenCutOff = 13;
	[SerializeField] private int adultCutOff = 20 ;
	[SerializeField] private int elderlyCutOff = 60;
	
	private bool isTransitioning = false;

	

	private void OnTransitionStart()
	{
		isTransitioning = true;
	}

	private void OnTransitionEnd()
	{
		isTransitioning = false;
	}


	private void Start()
	{
		RegisterAllListeners();
		SetVariables();
	}
	
	/// <summary>
	/// Registers all event listeners this class needs to care about.
	/// </summary>
	private void RegisterAllListeners()
	{
		EventManager.Instance.AddListener<NextRoomEvent>(OnTransitionStart);
		EventManager.Instance.AddListener<FinishedRoomTransition>(OnTransitionEnd);
	}

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
			SetPersonaDisabilities(disabilitiesList);
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

	public void GenderTogglePressed(Toggle pressedToggle)
	{
		if (pressedToggle.name == "Unspecified")
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
			persona.Disability = persona.Disability | disability;
		}
		OnPersonaChanged?.Invoke(persona);
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
	public void FocusNextRoom()
	{
		NextRoomEvent newInfo = new NextRoomEvent();
		//EventSystem.ExecuteEvent(EventType.UI_NEXT_ROOM, newInfo);
		EventManager.Instance.RaiseEvent(newInfo);
	}
	
	public void S()
	{
		
	}

	public void ProgressDialogue()
	{
		Debug.Log("Touch");
		if (SpeechBubble.Instance.DiagObject != null && isTransitioning == false)
		{
		   
			
			
			//If we have looped back to the start after an iteration
			if (SpeechBubble.Instance.Complete == true)
			{
				FillSpeechBubbleEvent repeatInfo = new FillSpeechBubbleEvent(null, Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, UIAnimator.BlurType.OUT, SpeechBubble.FillTextMethod.NONE, true);
				EventManager.Instance.RaiseEvent(repeatInfo);
			}
			else
			{
				FillSpeechBubbleEvent newInfo = new FillSpeechBubbleEvent(null, Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, UIAnimator.BlurType.NONE, SpeechBubble.FillTextMethod.ITERATE, false);
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
