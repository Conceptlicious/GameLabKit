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
	private static List<GameObject> foundObjects = new List<GameObject>();

	public void ObjectFound(GameObject foundObject)
	{
		HiddenObject currentHiddenObject = foundObject.GetComponent<HiddenObject>();

		if (foundObjects.Count == amountOfHiddenObjects)
		{
			WonMiniGame();
		}
		else if (!foundObjects.Contains(foundObject))
		{
			foundObject.SetActive(true);
			foundObjects.Add(foundObject);

			Debug.Log(foundObjects.Count);

			TextUpdater.Instance.CallUpdateTextCoroutine(foundObject.name,
				currentHiddenObject.Description);
		}
		else
		{
			TextUpdater.Instance.CallUpdateTextCoroutine(objectAlreadyFoundMessage, string.Empty);
		}
	}

	private void WonMiniGame()
	{
		TextUpdater.Instance.CallUpdateTextCoroutine(wonMiniGameMessage, string.Empty);

		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}
}