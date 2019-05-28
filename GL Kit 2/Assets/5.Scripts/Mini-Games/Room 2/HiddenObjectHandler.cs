using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class HiddenObjectHandler : BetterMonoBehaviour
{
	private const string objectAlreadyFoundMessage = "You have already found this object";
	private const string wonMiniGameMessage = "You won the mini-game!";
	private const int amountOfHiddenObjects = 8;

	[SerializeField] private GameObject nextMinigameButton = null;
	private List<GameObject> foundObjects = new List<GameObject>();

	private void Start()
	{
		SetVariables();
	}

	public void ObjectFound(GameObject foundObject)
	{
		HiddenObject currentHiddenObject = foundObject.GetComponent<HiddenObject>();

		if (!foundObjects.Contains(foundObject))
		{
			foundObject.SetActive(true);
			foundObjects.Add(foundObject);

			TextUpdater.Instance.CallUpdateTextCoroutine(foundObject.name,
				currentHiddenObject.Description);

			if (foundObjects.Count >= amountOfHiddenObjects)
			{
				nextMinigameButton.SetActive(true);
			}
		}
		else
		{
			TextUpdater.Instance.CallUpdateTextCoroutine(objectAlreadyFoundMessage, string.Empty);
		}
	}

	public void WonMiniGame()
	{
		TextUpdater.Instance.CallUpdateTextCoroutine(wonMiniGameMessage, string.Empty);

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