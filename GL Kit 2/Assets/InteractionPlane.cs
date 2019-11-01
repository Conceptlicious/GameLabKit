using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class InteractionPlane : BetterMonoBehaviour
{
	private bool cameraIsAnimating = false;

	private void Awake()
	{
		MenuManager.Instance.MenuOpened += OnMenuOpened;
		MenuManager.Instance.MenuClosed += OnMenuClosed;

		EventManager.Instance.AddListener<FinishedRoomTransition>(OnFinishedRoomTransition);
		EventManager.Instance.AddListener<NextRoomEvent>(OnNextRoomEvent);
	}

	private void OnDestroy()
	{
		if (MenuManager.IsInitialized)
		{
			MenuManager.Instance.MenuOpened -= OnMenuOpened;
			MenuManager.Instance.MenuClosed -= OnMenuClosed;
		}

		EventManager.InstanceIfInitialized?.RemoveListener<FinishedRoomTransition>(OnFinishedRoomTransition);
		EventManager.InstanceIfInitialized?.RemoveListener<NextRoomEvent>(OnNextRoomEvent);
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
		if (!(menu is DialogueMenu) || cameraIsAnimating)
		{
			return;
		}

		EnableInteraction();
	}

	private void OnNextRoomEvent()
	{
		DisableInteraction();
		cameraIsAnimating = true;
	}

	private void OnFinishedRoomTransition()
	{
		EnableInteraction();
		cameraIsAnimating = false;
	}

	//private void EnableInteraction() => Debug.Log("Enabling interaction...");
	private void EnableInteraction() => gameObject.SetActive(false);

	//private void DisableInteraction() => Debug.Log("Disabling interaction...");
	private void DisableInteraction() => gameObject.SetActive(true);
}