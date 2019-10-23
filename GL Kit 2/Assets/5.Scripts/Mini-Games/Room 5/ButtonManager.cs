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
	private const string FUNTYPE_VARIABLE = "funType";

	public Button EasyFun { get; private set; }
	public Button PeopleFun { get; private set; }
	public Button HardFun { get; private set; }
	public Button SeriousFun { get; private set; }
	public Sprite LastClickedButtonSprite { get; private set; }
	[HideInInspector] public Button startRotationButton;
	private int activeButtonCount = 1;
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
				DialogueManager.Instance.CurrentDialogue.CurrentKnot = "Part2";
				MenuManager.Instance.OpenMenu<DialogueMenu>();
				break;
			case 3:
				HardFun.interactable = true;
				break;
			case 4:
				SeriousFun.interactable = true;
				break;
			case 5:
				DialogueManager.Instance.CurrentDialogue.CurrentKnot = "Part3";
				MenuManager.Instance.OpenMenu<DialogueMenu>();
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

	private void SetLastClickedButton(string name, Sprite sprite)
	{
		if (!UIHandler.Instance.wonMinigame)
		{
			return;
		}

		LastClickedButtonSprite = sprite;

		DialogueManager.Instance.CurrentDialogue.Reset("Part4");
		DialogueManager.Instance.CurrentDialogue.SetStringVariable(FUNTYPE_VARIABLE, $"\"{name}\"");
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}

	private void OnDialogueChoiceSelected(DialogueChoiceSelectedEvent eventData)
	{
		if(eventData.DialogueChoice.text != "Yes" || DialogueManager.Instance.CurrentRoomID != RoomType.Dynamics)
		{
			return;
		}

		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Dynamics));
		EventManager.Instance.RaiseEvent(new NextRoomEvent());
	}

	private void SetVariables()
	{
		EasyFun = transform.Find("EasyFun").GetComponent<Button>();
		easyFunImage = EasyFun.transform.Find("Sprite").GetComponent<Image>();
		EasyFun.onClick.AddListener(() => SetLastClickedButton(EasyFun.name, easyFunImage.sprite));
		EasyFun.interactable = true;

		PeopleFun = transform.Find("PeopleFun").GetComponent<Button>();
		peopleFunImage = PeopleFun.transform.Find("Sprite").GetComponent<Image>();
		PeopleFun.onClick.AddListener(() => SetLastClickedButton(PeopleFun.name, peopleFunImage.sprite));
		PeopleFun.interactable = false;

		HardFun = transform.Find("HardFun").GetComponent<Button>();
		hardFunImage = HardFun.transform.Find("Sprite").GetComponent<Image>();
		HardFun.onClick.AddListener(() => SetLastClickedButton(HardFun.name, hardFunImage.sprite));
		HardFun.interactable = false;

		SeriousFun = transform.Find("SeriousFun").GetComponent<Button>();
		seriousFunImage = SeriousFun.transform.Find("Sprite").GetComponent<Image>();
		SeriousFun.onClick.AddListener(() => SetLastClickedButton(SeriousFun.name, seriousFunImage.sprite));
		SeriousFun.interactable = false;

		startRotationButton = transform.Find("StartRotation").GetComponent<Button>();
		startRotationButton.onClick.AddListener(() => UIHandler.Instance.StartGearRotation());
		startRotationButton.gameObject.SetActive(false);

		EventManager.Instance.AddListener<DialogueChoiceSelectedEvent>(OnDialogueChoiceSelected);
	}
}