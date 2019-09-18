using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventType = CustomEventCallbacks.EventType;
using GameLab;

//--------------------------------------------------
//Produced by: Josh van Asten, expanded by Noah and Mathias.
//Overview: This script is the single interface point for UI-to-script communication. It should be attached on a single
//manager object in-game and each button or UI object which needs to call a method, should use this script to do so.
//There were once separate scripts made by each programmer to do this same job, which were later consolidated into one. 
//As such the quality and consistency of this script is debateable.
//Usage: Used globally.
//--------------------------------------------------

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
	private Dictionary<string, UnityEngine.Events.UnityAction> events = new Dictionary<string, UnityEngine.Events.UnityAction>();
	
	private bool isTransitioning = false;
	public Button speechBoxButton;

	private void OnTransitionStart()
	{
		isTransitioning = true;
		speechBoxButton.GetComponent<Button>().enabled = false;
		
	}

	private void OnTransitionEnd()
	{
		isTransitioning = false;
		speechBoxButton.GetComponent<Button>().enabled = true;
	}


	private void Start()
	{
		RegisterAllListeners();
		SetVariables();
	}
	
	//NOTE: Please rename, Noah
	/// <summary>
	/// Holds the fuction procedural buttons should receive based on their name.
	/// </summary>
	private void ThatMethodJoshToldMeToRename()
	{
		events.Add("Next", OnNextButton);
		events.Add("Back", OnBackButton);
		events.Add("Confirm", OnConfirmButton);
		events.Add("Start", OnStartButton);
		events.Add("Redo", OnRedoButton);
	}

	/// <summary>
	/// Registers all event listeners this class needs to care about.
	/// </summary>
	private void RegisterAllListeners()
	{
		EventManager.Instance.AddListener<NextRoomEvent>(OnTransitionStart);
		EventManager.Instance.AddListener<FinishedRoomTransition>(OnTransitionEnd);
		EventManager.Instance.AddListener<ProceduralButtonCreation>(OnButtonCreate);
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
			persona.Disability |= disabilities;
			OnPersonaChanged.Invoke(persona);
		}
		else
		{
			toggleToAdd.isOn = false;
		}
	}

	public void OnNextButton()
	{  
		Debug.Log("Next button Pressed");
	}

	public void OnStartButton()
	{
		Debug.Log("Start button Pressed");
	}

	public void OnBackButton()
	{
		Debug.Log("Back button Pressed");
	}

	public void OnConfirmButton()
	{
		Debug.Log("Confirm button Pressed");
	}

	public void OnRedoButton()
	{
		Debug.Log("Redo button pressed");
	}

	private void OnButtonCreate(ProceduralButtonCreation info)
	{
		Debug.Log(info.ButtonName);
		info.Button.onClick.AddListener(events[info.ButtonName]);
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
			persona.Disability ^= disabilitiesToRemove;
			OnPersonaChanged.Invoke(persona);
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

	
	/// <summary>
	/// Used in Room 1 for character creation.
	/// </summary>
	/// <param name="gender"></param>
	/// <param name="isEnabled"></param>
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

	/// <summary>
	/// Used in Room 1 for character creation.
	/// </summary>
	/// <param name="newDisabilities"></param>
	public void SetPersonaDisabilities(Disabilities newDisabilities)
	{
	/*	if(newDisabilities.Count != 0)
		{
			foreach (Disabilities disability in newDisabilities)
			{
				persona.Disability |= disability;
			}
		}
		else
		{
			persona.Disability = new Disabilities();
		}*/
		OnPersonaChanged?.Invoke(persona);
	}

	/// <summary>
	/// Used in Room 1 for character creation.
	/// </summary>
	public void SetVariables()
	{
		ThatMethodJoshToldMeToRename();
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
		SaveItemEvent saveItemEvent = new SaveItemEvent(RoomType.TargetAudience);
		EventManager.Instance.RaiseEvent(saveItemEvent);

		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}

	/// <summary>
	/// Used to force a room transition
	/// </summary>
	public void FocusNextRoom()
	{
		NextRoomEvent newInfo = new NextRoomEvent();
		//EventSystem.ExecuteEvent(EventType.UI_NEXT_ROOM, newInfo);
		EventManager.Instance.RaiseEvent(newInfo);
	}

	/// <summary>
	/// Used in the settings menu to change languages
	/// </summary>
	/// <param name="pLanguage"></param>
	public void ChangeLanguage(int pLanguage)
	{
		int languageOption = Mathf.Clamp(pLanguage, 0, (int)GameData.Language.TOTAL);
		GameData.SetLanguage((GameData.Language)languageOption);
	}

	/// <summary>
	/// Used when clicking on speech bubbles to progress dialogue.
	/// </summary>
	public void ProgressDialogue()
	{
		Debug.Log("Touch");
		if (SpeechBubble.Instance.DiagObject != null && isTransitioning == false)
		{
			//If we have looped back to the start after an iteration
			if (SpeechBubble.Instance.Complete == true)
			{
				speechBoxButton.GetComponent<Button>().enabled = false;
				DismissSpeechBubbleEvent dismissInfo = new DismissSpeechBubbleEvent();
				EventManager.Instance.RaiseEvent(dismissInfo);
			}
			else
			{
				speechBoxButton.GetComponent<Button>().enabled = true;
				FillSpeechBubbleEvent newInfo = new FillSpeechBubbleEvent(null, Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, UIAnimator.BlurType.NONE, SpeechBubble.FillTextMethod.ITERATE, false);
				EventManager.Instance.RaiseEvent(newInfo);

				Debug.Log("Field Index: " + SpeechBubble.Instance.DiagObject.Info.fieldIndex);
			}
		}

	}

	/// <summary>
	/// Used for creating a popup on button press.
	/// </summary>
	/// <param name="pDialogueObject"></param>
	public void PopUpWithParam(DialogueObject pDialogueObject)
	{
		CreateSpesifiedPopUpEvent newInfo = new CreateSpesifiedPopUpEvent(pDialogueObject);

	}

}
