using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FunType
{
	EasyFun,
	PeopleFun,
	HardFun,
	SeriousFun
};

public class UIHandler : MonoBehaviour
{
	public Text typeText = null;

	[SerializeField] private Transform dropZonesObject = null;
	[SerializeField] private Transform gearsObject = null;
	public Transform GearsObject => gearsObject;
	[SerializeField] private List<GameObject> funTypeTabs = new List<GameObject>();

	private void Start()
	{
		ButtonManager.Instance.easyFun.onClick.AddListener(() => ChangeFunType(FunType.EasyFun));
		ButtonManager.Instance.peopleFun.onClick.AddListener(() => ChangeFunType(FunType.PeopleFun));
		ButtonManager.Instance.hardFun.onClick.AddListener(() => ChangeFunType(FunType.HardFun));
		ButtonManager.Instance.seriousFun.onClick.AddListener(() => ChangeFunType(FunType.SeriousFun));

		ChangeFunType(FunType.EasyFun);
	}

	public void StartGearRotation()
	{
		if (DropZone.OccupiedPlaces == 3)
		{
			foreach (Transform child in gearsObject)
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
				dropZones[0].NeededType = GearType.Exploration;
				dropZones[1].NeededType = GearType.Fantasy;
				dropZones[2].NeededType = GearType.Creativity;

				DisableTypeTabs();
				funTypeTabs[0].SetActive(true);
				gearsObject = funTypeTabs[0].transform;
				typeText.text = "Easy Fun";
				break;

			case FunType.PeopleFun:
				dropZones[0].NeededType = GearType.Communication;
				dropZones[1].NeededType = GearType.Cooperation;
				dropZones[2].NeededType = GearType.Competition;

				DisableTypeTabs();
				funTypeTabs[1].SetActive(true);
				gearsObject = funTypeTabs[1].transform;
				typeText.text = "People Fun";
				break;

			case FunType.HardFun:
				dropZones[0].NeededType = GearType.Goals;
				dropZones[1].NeededType = GearType.Obstacles;
				dropZones[2].NeededType = GearType.Stategy;

				DisableTypeTabs();
				funTypeTabs[2].SetActive(true);
				gearsObject = funTypeTabs[2].transform;
				typeText.text = "Hard Fun";
				break;

			case FunType.SeriousFun:
				dropZones[0].NeededType = GearType.Learing;
				dropZones[1].NeededType = GearType.Rhythm;
				dropZones[2].NeededType = GearType.Collection;

				DisableTypeTabs();
				funTypeTabs[3].SetActive(true);
				gearsObject = funTypeTabs[3].transform;
				typeText.text = "Serious Fun";
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
}