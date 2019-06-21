using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;
using UnityEngine.UI;

public class NodePattern : BetterMonoBehaviour
{
	[SerializeField] private NodePattern nextPattern;
	private IEnumerator coroutine;
	private DrawLines drawLines;

	/* You don't need a SerializeField because you don't have to change it in the editor.
	 * In that case you can better use a property instead of a pointer.
	 * [SerializeField] private bool isInteractable = true;
	 * public bool IsInteractable => isInteractable;*/
	public bool IsInteractable { get; private set; } = true;
	[SerializeField] private Image logoImage;
	[SerializeField] private Image startDotPrefab;
	[SerializeField] private NodeLayer[] layers;
	[SerializeField] private Transform currentTube;
	[SerializeField] private Transform nextTube;

	/* Always put a access modifiers in front of a variable. (private, public etc.)
	 * [SerializeField] GameObject tubeSprite;
	 * [SerializeField] Sprite normalTubeSprite;
	 * [SerializeField] Sprite wrongInputSprite;*/
	[SerializeField] private GameObject tubeSprite;
	[SerializeField] private Sprite normalTubeSprite;
	[SerializeField] private Sprite wrongInputSprite;
	[SerializeField] private int previousLayer;
	private Image startDot = null;

	/* You don't need a SerializeField because you don't need to change it in the editor.
	 * You don't need a pointer because you only use it in this class.
	 * [SerializeField] private int activeLayer;
	 * public int ActiveLayer => activelayer;*/
	private int activeLayer;
	private float wrongInputTimer;
	private bool fakeNodePressed;
	private bool isComplete = false;

	private void Start()
	{
		/* You don't need the "gameObject." for the GetComponent. 
		 * drawLines = gameObject.GetComponent<DrawLines>(); */
		drawLines = GetComponent<DrawLines>();
		Debug.Assert(drawLines != null, "No drawLines component detected");
		ActiveLayerReset();
	}

	private void OnEnable()
	{
		// register to node events
		foreach (NodeLayer layer in layers)
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
		if (!node.FakeCheck && !fakeNodePressed)
		{
			if (nodeLayer == layers[0] && activeLayer == 0)
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

	/*Don't use Hungarian notation, pWaitTime = waitTime.
	 * public IEnumerator WrongInputSpriteSwap(float pWaitTime)
	 */
	public IEnumerator WrongInputSpriteSwap(float waitTime)
	{
		Image currentImage = tubeSprite.GetComponent<Image>();
		currentImage.sprite = wrongInputSprite;
		fakeNodePressed = true;

		yield return new WaitForSeconds(waitTime);

		drawLines.ResetLine();
		ActiveLayerReset();
		currentImage.sprite = normalTubeSprite;
		coroutine = null;
		fakeNodePressed = false;
	}

	//When a correct button is pressed, it sets current layer
	public void SetLayer()
	{
		if (isComplete || !IsInteractable)
		{
			return;
		}

		/* First set the  previous layer so you don't have to do math.
		 * activeLayer++;
		 * previousLayer = activeLayer - 1; */

		previousLayer = activeLayer;
		activeLayer++;

		if (activeLayer >= layers.Length)
		{
			isComplete = true;

			if (nextPattern != null)
			{
				/*CameraTargetSelectEvent newInfo = new CameraTargetSelectEvent(currentTube, nextTube, Settings.VAL_CAMERA_MINOR_ZOOM_DISTANCE, false, false);
				EventManager.Instance.RaiseEvent(newInfo);*/

				nextPattern.IsInteractable = true;
			}
			/* Why the check for the "isComplete", it is set to true above.
			 * Then you only need an else instead of else if.
			 * else if (nextPattern == null && isComplete)*/
			else
			{
				SendSpriteRoom6.Instance.WonMinigame();
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

		if (startDot != null)
		{
			Destroy(startDot.gameObject);
		}
	}
}
