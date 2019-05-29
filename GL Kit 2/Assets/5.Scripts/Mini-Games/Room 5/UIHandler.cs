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
	public Transform GearsObject { get; private set; }
	public Text TypeText { get; set; }
	public List<DropZone> DropZones { get; } = new List<DropZone>();
	private Transform dropZonesObject = null;
	private readonly List<GameObject> funTypeTabs = new List<GameObject>();

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

				if (!DropZone.isCombinationRight)
				{
					child.GetComponent<GearInformation>().StopGearRotationMethod(true);
				}
			}

			if (DropZone.isCombinationRight)
			{
				ButtonManager.Instance.EnableNextButton();

				foreach (Transform child in GearsObject)
				{
					child.GetComponent<DragAndDrop>().isAbleToMove = false;
				}
			}

			GridHandler.Instance.EmptyDropZones();
		}
	}

	public void ChangeFunType(FunType funType)
	{
		DropZones.Clear();

		switch (funType)
		{
			case FunType.EasyFun:
				GearsObject = funTypeTabs[0].transform;

				foreach (Transform child in GearsObject)
				{
					DropZone currentDropZone = child.GetComponent<DropZone>();
					if (currentDropZone != null)
					{
						DropZones.Add(child.GetComponent<DropZone>());
						child.GetComponent<DropZone>().Unoccupy();
					}
				}

				DropZones[0].neededType = GearType.Exploration;
				DropZones[1].neededType = GearType.Fantasy;
				DropZones[2].neededType = GearType.Creativity;

				DisableTypeTabs();
				funTypeTabs[0].SetActive(true);
				TypeText.text = "Easy Fun";
				break;

			case FunType.PeopleFun:
				GearsObject = funTypeTabs[1].transform;

				foreach (Transform child in GearsObject)
				{
					DropZone currentDropZone = child.GetComponent<DropZone>();
					if (currentDropZone != null)
					{
						DropZones.Add(child.GetComponent<DropZone>());
						child.GetComponent<DropZone>().Unoccupy();
					}
				}

				DropZones[0].neededType = GearType.Communication;
				DropZones[1].neededType = GearType.Cooperation;
				DropZones[2].neededType = GearType.Competition;

				DisableTypeTabs();
				funTypeTabs[1].SetActive(true);
				TypeText.text = "People Fun";
				break;

			case FunType.HardFun:
				GearsObject = funTypeTabs[2].transform;

				foreach (Transform child in GearsObject)
				{
					DropZone currentDropZone = child.GetComponent<DropZone>();
					if (currentDropZone != null)
					{
						DropZones.Add(child.GetComponent<DropZone>());
						child.GetComponent<DropZone>().Unoccupy();
					}
				}

				DropZones[0].neededType = GearType.Goals;
				DropZones[1].neededType = GearType.Obstacles;
				DropZones[2].neededType = GearType.Stategy;

				DisableTypeTabs();
				funTypeTabs[2].SetActive(true);
				TypeText.text = "Hard Fun";
				break;

			case FunType.SeriousFun:
				GearsObject = funTypeTabs[3].transform;

				foreach (Transform child in GearsObject)
				{
					DropZone currentDropZone = child.GetComponent<DropZone>();
					if (currentDropZone != null)
					{
						DropZones.Add(child.GetComponent<DropZone>());
						child.GetComponent<DropZone>().Unoccupy();
					}
				}

				DropZones[0].neededType = GearType.Learing;
				DropZones[1].neededType = GearType.Rhythm;
				DropZones[2].neededType = GearType.Collection;

				DisableTypeTabs();
				funTypeTabs[3].SetActive(true);
				TypeText.text = "Serious Fun";
				break;
		}


	}

	public void WonMiniGame()
	{
		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		EventManager.Instance.RaiseEvent(nextRoomEvent);
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