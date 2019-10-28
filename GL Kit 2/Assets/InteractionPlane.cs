using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class InteractionPlane : BetterMonoBehaviour
{
	private void Awake()
	{
		MenuManager.Instance.MenuOpened += OnMenuOpened;
		MenuManager.Instance.MenuClosed += OnMenuClosed;

		EventManager.Instance.AddListener<FinishedRoomTransition>(EnableInteraction);
		EventManager.Instance.AddListener<NextRoomEvent>(DisableInteraction);
	}

	private void OnDestroy()
	{
		if (MenuManager.IsInitialized)
		{
			MenuManager.Instance.MenuOpened -= OnMenuOpened;
			MenuManager.Instance.MenuClosed -= OnMenuClosed;
		}

		EventManager.InstanceIfInitialized?.RemoveListener<FinishedRoomTransition>(EnableInteraction);
		EventManager.InstanceIfInitialized?.RemoveListener<NextRoomEvent>(DisableInteraction);
	}

	private void OnMenuOpened(Menu menu)
	{
		if(!(menu is DialogueMenu))
		{
			return;
		}

		DisableInteraction();
	}

	private void OnMenuClosed(Menu menu)
	{
		if(!(menu is DialogueMenu))
		{
			return;
		}

		EnableInteraction();
	}

	private void EnableInteraction() => gameObject.SetActive(false);

	private void DisableInteraction() => gameObject.SetActive(true);
}
