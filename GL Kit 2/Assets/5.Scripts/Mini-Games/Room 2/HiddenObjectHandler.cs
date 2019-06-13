using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class HiddenObjectHandler : Singleton<HiddenObjectHandler>
{
	private const string OBJECT_ALREADY_FOUND_MESSAGE = "You have already found this object";
	private const string WON_MINIGAME_MESSAGE = "You won the mini-game!";
	private const int HIDDENOBJECTS_AMOUNT = 8;

	public Sprite lastSelectedObjectSprite { get; private set; }
	[SerializeField] private GameObject nextMinigameButton = null;
	private List<GameObject> foundObjects = new List<GameObject>();

	private void Start()
	{
		SetVariables();
	}

	public void ObjectFound(GameObject foundObject)
	{
		HiddenObject currentHiddenObject = foundObject.GetComponent<HiddenObject>();
		lastSelectedObjectSprite = currentHiddenObject.HiddenObjectSprite;

		if (!foundObjects.Contains(foundObject))
		{
			foundObject.SetActive(true);
			foundObjects.Add(foundObject);

			TextUpdater.Instance.CallUpdateTextCoroutine(foundObject.name,
				currentHiddenObject.Description);

			if (foundObjects.Count >= HIDDENOBJECTS_AMOUNT)
			{
				nextMinigameButton.SetActive(true);
			}
		}
		else
		{
			TextUpdater.Instance.CallUpdateTextCoroutine(foundObject.name, OBJECT_ALREADY_FOUND_MESSAGE);
		}
	}

	public void WonMiniGame()
	{
		TextUpdater.Instance.CallUpdateTextCoroutine(WON_MINIGAME_MESSAGE, string.Empty);

		SaveItemEvent saveItemEvent = new SaveItemEvent(RoomType.Goals);
		EventManager.Instance.RaiseEvent(saveItemEvent);

		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}

	private void SetVariables()
	{
		nextMinigameButton.SetActive(false);

		foreach(Transform child in transform)
		{
			child.gameObject.SetActive(false);
		}
	}
}