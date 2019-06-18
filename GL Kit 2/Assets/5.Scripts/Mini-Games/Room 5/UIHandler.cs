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

public class UIHandler : Singleton<UIHandler>
{
	public Transform FunTypeTab { get; private set; }
	public Text TypeText { get; set; }
	public List<DropZone> DropZones { get; } = new List<DropZone>();
	[HideInInspector] public bool wonMinigame = false;
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
			GearInformation.isAbleToRotate = true;

			if (DropZone.combinationIsRight)
			{
				ButtonManager.Instance.EnableNextButton();

				foreach (Transform child in FunTypeTab)
				{
					DragAndDrop currentDragAndDrop = child.GetComponent<DragAndDrop>();
					if (currentDragAndDrop != null)
					{
						currentDragAndDrop.isAbleToMove = false;
					}
				}
			}
			else
			{
				foreach (Transform child in FunTypeTab)
				{
					GearInformation currentGearInformation = child.GetComponent<GearInformation>();
					if (currentGearInformation != null)
					{
						currentGearInformation.StopGearRotationMethod(child.gameObject);
					}
				}
			}
		}
	}

	public void ChangeFunType(FunType funType)
	{
		DropZones.Clear();
		GearInformation.isAbleToRotate = false;

		switch (funType)
		{
			case FunType.EasyFun:
				FunTypeTab = funTypeTabs[0].transform;

				foreach (Transform child in FunTypeTab)
				{
					DropZone currentDropZone = child.GetComponent<DropZone>();

					if (currentDropZone != null)
					{
						currentDropZone.Unoccupy();
						DropZones.Add(currentDropZone);
					}
				}

				DisableTypeTabs();
				funTypeTabs[0].SetActive(true);
				TypeText.text = "Easy Fun";
				break;

			case FunType.PeopleFun:
				FunTypeTab = funTypeTabs[1].transform;

				foreach (Transform child in FunTypeTab)
				{
					DropZone currentDropZone = child.GetComponent<DropZone>();

					if (currentDropZone != null)
					{
						currentDropZone.Unoccupy();
						DropZones.Add(currentDropZone);
					}
				}

				DisableTypeTabs();
				funTypeTabs[1].SetActive(true);
				TypeText.text = "People Fun";
				break;

			case FunType.HardFun:
				FunTypeTab = funTypeTabs[2].transform;

				foreach (Transform child in FunTypeTab)
				{

					DropZone currentDropZone = child.GetComponent<DropZone>();

					if (currentDropZone != null)
					{
						currentDropZone.Unoccupy();
						DropZones.Add(currentDropZone);
					}
				}

				DisableTypeTabs();
				funTypeTabs[2].SetActive(true);
				TypeText.text = "Hard Fun";
				break;

			case FunType.SeriousFun:
				FunTypeTab = funTypeTabs[3].transform;

				foreach (Transform child in FunTypeTab)
				{
					DropZone currentDropZone = child.GetComponent<DropZone>();

					if (currentDropZone != null)
					{
						currentDropZone.Unoccupy();
						DropZones.Add(currentDropZone);
					}
				}

				DisableTypeTabs();
				funTypeTabs[3].SetActive(true);
				TypeText.text = "Serious Fun";
				break;
		}


	}

	public void WonMiniGame()
	{
		if (wonMinigame)
		{
			SaveItemEvent saveItemEvent = new SaveItemEvent(RoomType.Dynamics);
			EventManager.Instance.RaiseEvent(saveItemEvent);

			NextRoomEvent nextRoomEvent = new NextRoomEvent();
			EventManager.Instance.RaiseEvent(nextRoomEvent);
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