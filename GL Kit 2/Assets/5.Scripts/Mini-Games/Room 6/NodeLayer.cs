using System.Collections;
using System.Collections.Generic;
using GameLab;
using UnityEngine;

public class NodeLayer : BetterMonoBehaviour
{
	[SerializeField] private Node[] nodes;

	public void SetActive(bool pState)
	{
		for (int i = 0; i < nodes.Length; i++)
		{
			nodes[i].Show(pState);
		}
	}
}
