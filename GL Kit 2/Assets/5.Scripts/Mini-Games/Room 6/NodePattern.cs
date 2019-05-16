using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Dot at the start

public class NodePattern : MonoBehaviour
{
	

	[SerializeField] private bool isInteractable = true;
	[SerializeField] private NodeLayer[] layers;
	[SerializeField] private int activeLayer;
	[SerializeField] private int previousLayer;
	[SerializeField] private Image startDotPrefab;
	[SerializeField] private NodePattern nextPattern;

	public int ActiveLayer => activeLayer;
	private Image startDot = null;
	private DrawLines drawLines;
	private bool isComplete = false;
	

	public bool IsInteractable => isInteractable;

	private void Start()
	{
		ActiveLayerReset();
		//nextPattern.gameObject.SetActive(false);
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

			// Remove other buttons in layer
			for (int i = 0; i < layers.Length; i++)
			{
				layers[i].SetActive(false);
			}

			if(nextPattern != null)
				nextPattern.isInteractable = true;
			// Remove line and dot
			// Show art
			// Play Dialogue
			//Move on to the next pattern
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
