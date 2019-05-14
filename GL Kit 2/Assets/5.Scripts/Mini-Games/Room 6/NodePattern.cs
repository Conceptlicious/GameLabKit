using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Dot at the start

public class NodePattern : MonoBehaviour
{
	public int ActiveLayer => activeLayer;

	[SerializeField] private NodeLayer[] layers;
	[SerializeField] private int activeLayer;
	[SerializeField] private int previousLayer;
	[SerializeField] private Image startDotPrefab;

	private Image startDot = null;

	private bool isComplete = false;

	private void Start()
	{
		ActiveLayerReset();
	}

	//When a correct button is pressed, it sets current layer
	public void SetLayer()
	{
		if(isComplete)
		{
			return;
		}

		activeLayer++;
		previousLayer = activeLayer - 1;

		// Check if pattern is finished
		if (activeLayer >= layers.Length)
		{
			// fill in what you want to do once the pattern is completed
			isComplete = true;

			// do when complete
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
