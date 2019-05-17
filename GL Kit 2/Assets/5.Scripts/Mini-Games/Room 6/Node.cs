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
	//public event Action<Node> OnInteract;
	[SerializeField] private bool fakeCheck;
	[SerializeField] private DrawLines drawLines = null;
	[SerializeField] private NodePattern nodePattern = null;

	private Image startDotInstance = null;

	public void Show(bool pState)
	{
		gameObject.SetActive(pState);
	}

	//Ran when mouse over button
	public void OnPointerEnter(PointerEventData eventData)
	{
		// Raise event
		//OnInteract.Invoke(this);

		if(!nodePattern.IsInteractable)
		{
			return;
		}

		if (!fakeCheck)
		{
			if (nodePattern.ActiveLayer == 0)
			{
				nodePattern.SpawnStartDot(transform.parent, (transform as RectTransform).anchoredPosition);
			}

			drawLines.AddPosition(gameObject);
			nodePattern.SetLayer();
		}
		else
		{
			drawLines.ResetLine();
			nodePattern.ActiveLayerReset();
		}
	}
}
