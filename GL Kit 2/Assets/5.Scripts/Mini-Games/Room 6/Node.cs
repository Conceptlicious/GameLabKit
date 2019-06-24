using System;
using System.Collections;
using System.Collections.Generic;
using GameLab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Node : BetterMonoBehaviour, IPointerEnterHandler
{
	/*
	Script for every node which sends information to other scripts
	*/
	public event Action<Node> OnInteract;
	[SerializeField] private NodePattern nodePattern = null;

	[SerializeField] private bool fakeCheck;
	public bool FakeCheck => fakeCheck;
	private Image startDotInstance = null;

	/* Don't use Hungarian notation, pState = state.
	 * public void Show(bool pState) */
	public void Show(bool state)
	{
		gameObject.SetActive(state);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!nodePattern.IsInteractable)
		{
			return;
		}

		OnInteract?.Invoke(this);		
	}
}
