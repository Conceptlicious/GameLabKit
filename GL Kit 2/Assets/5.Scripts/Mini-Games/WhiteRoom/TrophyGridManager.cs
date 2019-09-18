using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class TrophyGridManager : Manager<TrophyGridManager>
{
	[SerializeField] float nextRoomDelay;
	public float NextRoomDelaySeconds => nextRoomDelay;

	private List<TrophySocket> trophySockets = new List<TrophySocket>();

	private void Start()
	{
		foreach(Transform child in transform)
		{
			TrophySocket trophySocketToAdd = child.GetComponent<TrophySocket>();

			if(trophySocketToAdd != null)
			{
				trophySockets.Add(trophySocketToAdd);
			}
		}
	}

	public TrophySocket GetTrophySocketUnder(RectTransform rectTransform)
	{
		foreach(TrophySocket trophySocket in trophySockets)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(trophySocket.transform as RectTransform, rectTransform.position))
			{
				return trophySocket;
			}
		}

		return null;

	}
}
