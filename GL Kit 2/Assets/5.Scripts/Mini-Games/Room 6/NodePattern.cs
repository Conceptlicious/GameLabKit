using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;
using UnityEngine.UI;

public class NodePattern : BetterMonoBehaviour
{
	[SerializeField] private Image logoImage;
	[SerializeField] private Image startDotPrefab;
	[SerializeField] private NodePattern nextPattern;
	[SerializeField] private NodeLayer[] layers;
	[SerializeField] private Transform currentTube;
	[SerializeField] private Transform nextTube;
	
	[SerializeField] private bool isInteractable = true;
	[SerializeField] private int activeLayer;
	[SerializeField] private int previousLayer;

	private IEnumerator coroutine;
	[SerializeField] GameObject tubeSprite;
	[SerializeField] Sprite normalTubeSprite;
	[SerializeField] Sprite wrongInputSprite;
	private float wrongInputTimer;
	private bool fakeNodePressed;


	public int ActiveLayer => activeLayer;
	private Image startDot = null;
	private DrawLines drawLines;
	private bool isComplete = false;
	
	public bool IsInteractable => isInteractable;

	private void Start()
	{
		drawLines = gameObject.GetComponent<DrawLines>();
		Debug.Assert(drawLines != null, "No drawlines component detected");
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
		if(!node.FakeCheck && !fakeNodePressed)
		{			
			if (nodeLayer == layers[0] && ActiveLayer == 0)
			{
				SpawnStartDot(node.CachedTransform.parent, node.CachedRectTransform.anchoredPosition);
			}

			drawLines.AddPosition(node.gameObject);
			SetLayer();
		}
		else if (coroutine == null)
		{
			coroutine = WrongInputSpriteSwap(2.0f);
			StartCoroutine(coroutine);		
		}
	}

	public IEnumerator WrongInputSpriteSwap(float pWaitTime)
	{
		
		Image currentImage = tubeSprite.GetComponent<Image>();
		currentImage.sprite = wrongInputSprite;
		fakeNodePressed = true;
		yield return new WaitForSeconds(pWaitTime);
		
		drawLines.ResetLine();
		ActiveLayerReset();
		currentImage.sprite = normalTubeSprite;
		coroutine = null;
		fakeNodePressed = false;
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

		

		if (activeLayer >= layers.Length)
		{
			isComplete = true;

			if (nextPattern != null)
			{	
				/////
				CameraTargetSelectEvent newInfo = new CameraTargetSelectEvent(currentTube, nextTube, Settings.VAL_CAMERA_MINOR_ZOOM_DISTANCE, false, false);
				EventManager.Instance.RaiseEvent(newInfo);
				
				nextPattern.isInteractable = true;
			}
			else if(nextPattern == null && isComplete)
			{
				Debug.Log("To The White Room");
				NextRoomEvent info = new NextRoomEvent();
				EventManager.Instance.RaiseEvent(info);
			}

			drawLines.ResetLine();

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
