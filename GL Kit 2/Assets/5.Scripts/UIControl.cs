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

	private const int TEEN_CUT_OFF = 13;
	private const int ADULT_CUT_OFF = 20;
	private const int ELDERLY_CUT_OFF = 60;
	private const int ENABLED_TOGGLES_PER_LIST_MAX = 3;
	public event Action<Persona> OnPersonaChanged;


	[SerializeField] private Button doneButton;
	[SerializeField] private Transform ageSliders;
	[SerializeField] private Transform genderToggles;
	//Temporary for testing Room1
	private Slider ageSilderMin = null;
	private Slider ageSilderMax = null;
	private Text targetAudienceText = null;
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
		if (toggleGroup == ToggleGroup.Education && activeEducationToggles.Count < ENABLED_TOGGLES_PER_LIST_MAX)
		{
			activeEducationToggles.Add(toggleToAdd);
		}
		else if (toggleGroup == ToggleGroup.SpecialNeeds && activeSpecialNeedToggles.Count < ENABLED_TOGGLES_PER_LIST_MAX)
		{
			activeSpecialNeedToggles.Add(toggleToAdd);
			persona.Disability |= disabilities;
			OnPersonaChanged?.Invoke(persona);
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
			persona.Disability &= ~disabilitiesToRemove;
			OnPersonaChanged?.Invoke(persona);
		}
	}

	public void UpdateAgeSlider()
	{
		float calculateAverageAge = (ageSilderMin.value + ageSilderMax.value) / 2;
		int averageAge = Mathf.RoundToInt(calculateAverageAge);

		if (averageAge >= ELDERLY_CUT_OFF)
		{
			targetAudienceText.text = $"Eldery\n{averageAge}+";
			persona.Age = AgeGroup.Elderly;
		}
		else if (averageAge >= ADULT_CUT_OFF)
		{
			targetAudienceText.text = $"Adult\n{averageAge}";
			persona.Age = AgeGroup.Adult;
		}
		else if (averageAge >= TEEN_CUT_OFF)
		{
			targetAudienceText.text = $"Teenager\n{averageAge}";
			persona.Age = AgeGroup.Teenager;
		}
		else
		{
			targetAudienceText.text = $"Child\n{averageAge}";
			persona.Age = AgeGroup.Child;
		}

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


	/// <summary>
	/// Used to force a room transition
	/// </summary>
	public void FocusNextRoom()
	{
		NextRoomEvent newInfo = new NextRoomEvent();
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

	private void OnDialogueKnotCompleted(DialogueKnotCompletedEvent eventData)
	{
		if (eventData.Knot != "Part2" || eventData.CompletedRoomID != RoomType.TargetAudience)
		{			
			return;
		}

		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.TargetAudience));
		EventManager.Instance.RaiseEvent(new NextRoomEvent());
	}

	private void OnFinishedRoomTransition(FinishedRoomTransition eventData)
	{
		int currentRoomID = RoomManager.Instance.GetCurrentRoomID().z;

		if (currentRoomID == 1)
		{
			DialogueManager.Instance.SetCurrentDialogue(RoomType.TargetAudience);
			MenuManager.Instance.OpenMenu<DialogueMenu>();
		}
	}

	private void OnDonePressed()
	{
		DialogueManager.Instance.CurrentDialogue.Reset("Part2");
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}

	private void SetVariables()
	{
		ageSilderMin = ageSliders.Find("AgeSliderMin").GetComponent<Slider>();
		ageSilderMax = ageSliders.Find("AgeSliderMax").GetComponent<Slider>();

		ageSilderMax.onValueChanged.AddListener((_) => UpdateAgeSlider());
		ageSilderMin.onValueChanged.AddListener((_) => UpdateAgeSlider());

		targetAudienceText = ageSilderMin.transform.Find("TargetAudicence").GetComponent<Text>();

		UpdateAgeSlider();

		unspecified = genderToggles.Find("Unspecified").GetComponent<Toggle>();
		female = genderToggles.Find("Female").GetComponent<Toggle>();
		male = genderToggles.Find("Male").GetComponent<Toggle>();

		unspecified.onValueChanged.AddListener((isEnabled) => GenderTogglePressed(Genders.Unspecified, isEnabled));
		female.onValueChanged.AddListener((isEnabled) => GenderTogglePressed(Genders.Female, isEnabled));
		male.onValueChanged.AddListener((isEnabled) => GenderTogglePressed(Genders.Male, isEnabled));

		doneButton.onClick.AddListener(() => OnDonePressed());

		EventManager.Instance.AddListener<FinishedRoomTransition>(OnFinishedRoomTransition);
		EventManager.Instance.AddListener<DialogueKnotCompletedEvent>(OnDialogueKnotCompleted);
	}
}
