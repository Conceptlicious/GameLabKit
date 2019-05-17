using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;
using UnityEngine.UI;
// Dot at the start

public class NodePattern : BetterMonoBehaviour
{
	[SerializeField] private Image logoImage;
	[SerializeField] private Image startDotPrefab;
	[SerializeField] private NodePattern nextPattern;
	[SerializeField] private NodeLayer[] layers;

	[SerializeField] private bool isInteractable = true;
	[SerializeField] private int activeLayer;
	[SerializeField] private int previousLayer;

	public int ActiveLayer => activeLayer;
	private Image startDot = null;
	[SerializeField] private DrawLines drawLines;
	private bool isComplete = false;
	

	public bool IsInteractable => isInteractable;

	private void Start()
	{
		ActiveLayerReset();		
	}

	private void OnEnable()
	{
		// register to node events
		foreach(NodeLayer layer in layers)
		{
			layer.OnInteracted += OnInteracted;
		}
	}

	private void OnDisable()
	{
		// unregister from node events	
		foreach (NodeLayer layer in layers)
		{
			layer.OnInteracted -= OnInteracted;
		}
	}

	private void OnInteracted(NodeLayer nodeLayer, Node node)
	{
		if(!node.FakeCheck)
		{
			if (nodeLayer == layers[0] && ActiveLayer == 0)
			{
				SpawnStartDot(node.CachedTransform.parent, node.CachedRectTransform.anchoredPosition);
			}

			drawLines.AddPosition(node.gameObject);
			SetLayer();
		}
		else
		{			
			drawLines.ResetLine();
			ActiveLayerReset();
		}
	}

	//When a correct button is pressed, it sets current layer
	public void SetLayer()
	{

		if(isComplete || !isInteractable)
		{			
			return;			
		}

		activeLayer++;
		previousLayer = activeLayer - 1;

		// Check if pattern is finished
		if (activeLayer >= layers.Length)
		{
			isComplete = true;

			if (nextPattern != null)
			{
				nextPattern.isInteractable = true;				
			}

			drawLines.ResetLine();

			// Remove other buttons in layer
			for (int i = 0; i < layers.Length; i++)
			{
				layers[i].SetActive(false);
				logoImage.gameObject.SetActive(true);
			}			

			Destroy(startDot.gameObject);

		}
		else
		{
			layers[activeLayer].SetActive(true);
			layers[previousLayer].SetActive(false);
		}
	}

	public void SpawnStartDot(Transform parent, Vector3 position)
	{
		startDot = Instantiate(startDotPrefab, parent);
		startDot.rectTransform.anchoredPosition = position;
	}

	//When an incorrect button is pressed, reset layers
	public void ActiveLayerReset()
	{

		activeLayer = 0;
		for (int i = 1; i < layers.Length; i++)
		{
			layers[i].SetActive(false);
		}

		if(startDot != null)
		{
			Destroy(startDot.gameObject);
		}
	}
}
