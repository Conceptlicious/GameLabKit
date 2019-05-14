using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLayer : MonoBehaviour
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
