using System;
using System.Collections;
using System.Collections.Generic;
using GameLab;
using UnityEngine;

public class NodeLayer : BetterMonoBehaviour
{
	[SerializeField] private Node[] nodes = null;
	public event Action<NodeLayer, Node> OnInteracted;

	private void OnEnable()
	{
		foreach(Node node in nodes)
		{
			node.OnInteract += OnInteract;
		}
	}

	private void OnDisable()
	{
		foreach (Node node in nodes)
		{
			node.OnInteract -= OnInteract;
		}
	}

	private void OnInteract(Node node)
	{
		OnInteracted?.Invoke(this, node);
	}

	public void SetActive(bool pState)
	{
		for (int i = 0; i < nodes.Length; i++)
		{
			nodes[i].Show(pState);
		}
	}
}
