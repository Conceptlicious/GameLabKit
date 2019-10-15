using GameLab;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum FunType
{
	EasyFun,
	PeopleFun,
	HardFun,
	SeriousFun
};

//--------------------------------------------------
//Produced by Mathias
//Overview: This is script is used to handle all the UI and visuals, except for the buttons.
//Usage: Once on the "UI Handler" Object
//--------------------------------------------------

public class UIHandler : Singleton<UIHandler>
{
	public Transform CurrentFunTypeTab { get; private set; }
	public List<DropZone> DropZones { get; } = new List<DropZone>();
	[HideInInspector] public bool wonMinigame = false;
	private Text typeText;
	private Transform dropZonesObject = null;
	private readonly List<GameObject> funTypeTabs = new List<GameObject>();

	private void Start()
	{
		SetVariables();
	}

	public void StartGearRotation()
	{
		GearInformation.isAbleToRotate = true;

		if (DropZone.combinationIsRight)
		{
			ButtonManager.Instance.EnableNextButton();

			foreach(DragAndDrop dragAndDrop in CurrentFunTypeTab.GetComponentsInChildren<DragAndDrop>())
			{
				dragAndDrop.isAbleToMove = false;
			}

			foreach (DropZone dropZone in CurrentFunTypeTab.GetComponentsInChildren<DropZone>())
			{
				dropZone.Unoccupy();
			}

			foreach (GearRotation gearRotation in CurrentFunTypeTab.GetComponentsInChildren<GearRotation>())
			{
				if (gearRotation.DeterminesSpeed)
				{
					gearRotation.CalculateRotaionSpeed();
				}
			}
		}
		else
		{
			foreach (GearInformation gearInformation in CurrentFunTypeTab.GetComponentsInChildren<GearInformation>())
			{
				gearInformation.StopGearRotationMethod(gearInformation.gameObject);
			}
		}

	}

	public void ChangeFunType(FunType funType)
	{
		GearInformation.isAbleToRotate = false;
		ButtonManager.Instance.startRotationButton.gameObject.SetActive(false);

		switch (funType)
		{
			case FunType.EasyFun:
				CurrentFunTypeTab = funTypeTabs[0].transform;

				SetupDropZones(CurrentFunTypeTab);
				DisableTypeTabs();

				funTypeTabs[0].SetActive(true);
				typeText.text = CurrentFunTypeTab.name;
				break;

			case FunType.PeopleFun:
				CurrentFunTypeTab = funTypeTabs[1].transform;

				SetupDropZones(CurrentFunTypeTab);
				DisableTypeTabs();

				funTypeTabs[1].SetActive(true);
				typeText.text = CurrentFunTypeTab.name;
				break;

			case FunType.HardFun:
				CurrentFunTypeTab = funTypeTabs[2].transform;

				SetupDropZones(CurrentFunTypeTab);
				DisableTypeTabs();

				funTypeTabs[2].SetActive(true);
				typeText.text = CurrentFunTypeTab.name;
				break;

			case FunType.SeriousFun:
				CurrentFunTypeTab = funTypeTabs[3].transform;

				SetupDropZones(CurrentFunTypeTab);
				DisableTypeTabs();

				funTypeTabs[3].SetActive(true);
				typeText.text = CurrentFunTypeTab.name;
				break;
		}
	}

	private void SetupDropZones(Transform newFuntypeTab)
	{
		foreach (DropZone dropZone in DropZones)
		{
			dropZone.Unoccupy();
		}
		DropZones.Clear();

		foreach (DropZone dropZone in newFuntypeTab.GetComponentsInChildren<DropZone>())
		{
			DropZones.Add(dropZone);
		}
	}

	private void DisableTypeTabs()
	{
		funTypeTabs[0].SetActive(false);
		funTypeTabs[1].SetActive(false);
		funTypeTabs[2].SetActive(false);
		funTypeTabs[3].SetActive(false);
	}

	private void OnFinishedRoomTransition(FinishedRoomTransition eventData)
	{
		int currentRoomID = RoomManager.Instance.GetCurrentRoomID().z;

		if (currentRoomID == 5)
		{
			DialogueManager.Instance.SetCurrentDialogue(RoomType.Dynamics);
			MenuManager.Instance.OpenMenu<DialogueMenu>();
		}
	}

	private void SetVariables()
	{
		typeText = GetComponentInChildren<Text>();

		funTypeTabs.Add(transform.Find("EasyFun").gameObject);
		funTypeTabs.Add(transform.Find("PeopleFun").gameObject);
		funTypeTabs.Add(transform.Find("HardFun").gameObject);
		funTypeTabs.Add(transform.Find("SeriousFun").gameObject);

		ButtonManager.Instance.EasyFun.onClick.AddListener(() => ChangeFunType(FunType.EasyFun));
		ButtonManager.Instance.PeopleFun.onClick.AddListener(() => ChangeFunType(FunType.PeopleFun));
		ButtonManager.Instance.HardFun.onClick.AddListener(() => ChangeFunType(FunType.HardFun));
		ButtonManager.Instance.SeriousFun.onClick.AddListener(() => ChangeFunType(FunType.SeriousFun));

		ChangeFunType(FunType.EasyFun);
		EventManager.Instance.AddListener<FinishedRoomTransition>(OnFinishedRoomTransition);
	}
}