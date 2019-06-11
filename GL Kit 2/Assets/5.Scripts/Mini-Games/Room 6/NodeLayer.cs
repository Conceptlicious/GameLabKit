using System;
using System.Collections;
using System.Collections.Generic;
using GameLab;
using UnityEngine;

public class NodeLayer : BetterMonoBehaviour
{
	[SerializeField] private Node[] nodes = null;
	public event Action<NodeLayer, Node> OnInteracted;

	/* Don't use Hungarian notation, pState = state.
	 * Always put public classes at the top.
	 * public void SetActive(bool pState)*/
	public void SetActive(bool state)
	{
		for (int i = 0; i < nodes.Length; i++)
		{
			nodes[i].Show(state);
		}
	}

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
}
