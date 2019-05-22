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
	public event Action<Node> OnInteract;

	[SerializeField] private bool fakeCheck;
	public bool FakeCheck => fakeCheck;

	[SerializeField] private NodePattern nodePattern = null;

	private Image startDotInstance = null;

	public void Show(bool pState)
	{
		gameObject.SetActive(pState);
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
