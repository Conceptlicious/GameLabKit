using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Mathias
//Overview: This script is used to handle all the buttons in room 5, it is called from the UIHandler.
//Usage: Once on the Buttons object.
//--------------------------------------------------

public class ButtonManager : Singleton<ButtonManager>
{
	private const float WON_MINIGAME_DELAY = 0.1f;

	[HideInInspector] public Button startRotationButton;

	[SerializeField] private Image buttonImages;

	public Button EasyFun { get; private set; }
	public Button PeopleFun { get; private set; }
	public Button HardFun { get; private set; }
	public Button SeriousFun { get; private set; }
	public Sprite LastClickedButtonSprite { get; private set; }
	private int activeButtonCount = 1;
	private Vector2 bigTypetextBox = new Vector2(900, 200);
	private Image easyFunImage = null;
	private Image peopleFunImage = null;
	private Image hardFunImage = null;
	private Image seriousFunImage = null;

	protected override void Awake()
	{
		base.Awake();
		SetVariables();
	}

	public void EnableNextButton()
	{
		activeButtonCount++;
		switch (activeButtonCount)
		{
			case 2:
				PeopleFun.interactable = true;
				EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
				break;
			case 3:
				HardFun.interactable = true;
				break;
			case 4:
				SeriousFun.interactable = true;
				break;
			case 5:
				EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
				StartCoroutine(WonMinigameDelay());
				break;
		}
	}

	public void DisableButtons()
	{
		EasyFun.interactable = false;
		PeopleFun.interactable = false;
		HardFun.interactable = false;
		SeriousFun.interactable = false;
	}

	public void EnableButtons()
	{
		switch (activeButtonCount)
		{
			case 1:
				EasyFun.interactable = true;
				break;
			case 2:
				EasyFun.interactable = true;
				PeopleFun.interactable = true;
				break;
			case 3:
				EasyFun.interactable = true;
				PeopleFun.interactable = true;
				HardFun.interactable = true;
				break;
			case 4:
				EasyFun.interactable = true;
				PeopleFun.interactable = true;
				HardFun.interactable = true;
				SeriousFun.interactable = true;
				break;
		}
	}

	private IEnumerator WonMinigameDelay()
	{
		yield return new WaitForSeconds(WON_MINIGAME_DELAY);
		UIHandler.Instance.wonMinigame = true;
	}

	private void SetLastClickedButton(Sprite lastClikedButtonSprite)
	{
		if (UIHandler.Instance.wonMinigame)
		{
			LastClickedButtonSprite = lastClikedButtonSprite;

			buttonImages.gameObject.SetActive(true);
			EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
			//EventManager.Instance.RaiseEvent(new NextRoomEvent());
			//EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Dynamics));
		}
	}

	public void YesButton()
	{
		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Dynamics));
		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		buttonImages.gameObject.SetActive(false);
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}

	public void NoButton()
	{
		buttonImages.gameObject.SetActive(false);
		DialogueProgression.Instance.RemoveChildren();
		ProgressDialogueEvent.ResetKnotID(3);
	}

	private void SetVariables()
	{
		EasyFun = transform.Find("EasyFun").GetComponent<Button>();
		easyFunImage = EasyFun.transform.Find("Sprite").GetComponent<Image>();
		EasyFun.onClick.AddListener(() => SetLastClickedButton(easyFunImage.sprite));
		EasyFun.interactable = true;

		PeopleFun = transform.Find("PeopleFun").GetComponent<Button>();
		peopleFunImage = PeopleFun.transform.Find("Sprite").GetComponent<Image>();
		PeopleFun.onClick.AddListener(() => SetLastClickedButton(peopleFunImage.sprite));
		PeopleFun.interactable = false;

		HardFun = transform.Find("HardFun").GetComponent<Button>();
		hardFunImage = HardFun.transform.Find("Sprite").GetComponent<Image>();
		HardFun.onClick.AddListener(() => SetLastClickedButton(hardFunImage.sprite));
		HardFun.interactable = false;

		SeriousFun = transform.Find("SeriousFun").GetComponent<Button>();
		seriousFunImage = SeriousFun.transform.Find("Sprite").GetComponent<Image>();
		SeriousFun.onClick.AddListener(() => SetLastClickedButton(seriousFunImage.sprite));
		SeriousFun.interactable = false;

		startRotationButton = transform.Find("StartRotation").GetComponent<Button>();
		startRotationButton.onClick.AddListener(() => UIHandler.Instance.StartGearRotation());
		startRotationButton.gameObject.SetActive(false);
	}
}