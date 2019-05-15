using GameLab;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum FunType
{
	EasyFun,
	PeopleFun,
	HardFun,
	SeriousFun
};

public class UIHandler : Singleton<UIHandler>
{
	private const int otherChildsInHandler = 2;

	public Transform GearsObject { get; private set; }
	public Text TypeText { get; set; }
	private Transform dropZonesObject = null;
	private List<GameObject> funTypeTabs = new List<GameObject>();

	private void Start()
	{
		SetVariables();
	}

	public void StartGearRotation()
	{
		if (DropZone.OccupiedPlaces == 3)
		{
			foreach (Transform child in GearsObject)
			{
				child.GetComponent<GearInformation>().isAbleToRotate = true;

				if (!DropZone.IsCombinationRight)
				{
					child.GetComponent<GearInformation>().StopGearRotationMethod();
				}
			}

			if (DropZone.IsCombinationRight)
			{
				ButtonManager.Instance.EnableNextButton();
			}

			GridHandler.Instance.EmptyDropZones();
		}
	}

	public void ChangeFunType(FunType funType)
	{
		List<DropZone> dropZones = new List<DropZone>();

		foreach (Transform dropZone in dropZonesObject)
		{
			dropZones.Add(dropZone.GetComponent<DropZone>());
		}

		switch (funType)
		{
			case FunType.EasyFun:
				dropZones[0].neededType = GearType.Exploration;
				dropZones[1].neededType = GearType.Fantasy;
				dropZones[2].neededType = GearType.Creativity;

				DisableTypeTabs();
				funTypeTabs[0].SetActive(true);
				GearsObject = funTypeTabs[0].transform;
				TypeText.text = "Easy Fun";
				break;

			case FunType.PeopleFun:
				dropZones[0].neededType = GearType.Communication;
				dropZones[1].neededType = GearType.Cooperation;
				dropZones[2].neededType = GearType.Competition;

				DisableTypeTabs();
				funTypeTabs[1].SetActive(true);
				GearsObject = funTypeTabs[1].transform;
				TypeText.text = "People Fun";
				break;

			case FunType.HardFun:
				dropZones[0].neededType = GearType.Goals;
				dropZones[1].neededType = GearType.Obstacles;
				dropZones[2].neededType = GearType.Stategy;

				DisableTypeTabs();
				funTypeTabs[2].SetActive(true);
				GearsObject = funTypeTabs[2].transform;
				TypeText.text = "Hard Fun";
				break;

			case FunType.SeriousFun:
				dropZones[0].neededType = GearType.Learing;
				dropZones[1].neededType = GearType.Rhythm;
				dropZones[2].neededType = GearType.Collection;

				DisableTypeTabs();
				funTypeTabs[3].SetActive(true);
				GearsObject = funTypeTabs[3].transform;
				TypeText.text = "Serious Fun";
				break;
		}
	}

	private void DisableTypeTabs()
	{
		funTypeTabs[0].SetActive(false);
		funTypeTabs[1].SetActive(false);
		funTypeTabs[2].SetActive(false);
		funTypeTabs[3].SetActive(false);
	}

	private void SetVariables()
	{
		TypeText = GetComponentInChildren<Text>();
				
		dropZonesObject = transform.Find("DropZones");
		funTypeTabs.Add(transform.Find("EasyFun").gameObject);
		funTypeTabs.Add(transform.Find("PeopleFun").gameObject);
		funTypeTabs.Add(transform.Find("HardFun").gameObject);
		funTypeTabs.Add(transform.Find("SeriousFun").gameObject);

		ButtonManager.Instance.EasyFun.onClick.AddListener(() => ChangeFunType(FunType.EasyFun));
		ButtonManager.Instance.PeopleFun.onClick.AddListener(() => ChangeFunType(FunType.PeopleFun));
		ButtonManager.Instance.HardFun.onClick.AddListener(() => ChangeFunType(FunType.HardFun));
		ButtonManager.Instance.SeriousFun.onClick.AddListener(() => ChangeFunType(FunType.SeriousFun));
		ChangeFunType(FunType.EasyFun);
	}
}