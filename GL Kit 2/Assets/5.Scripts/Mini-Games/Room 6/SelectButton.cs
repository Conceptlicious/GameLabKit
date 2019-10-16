using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class SelectButton : BetterMonoBehaviour
{
	[SerializeField] private Image completeButtons;
	NodePattern nodePattern;
	public static Sprite ArtSprite { get; private set; }
	private Image buttonImage;
	private Button selectButton;

	private void Start()
	{
		selectButton = GetComponent<Button>();
		buttonImage = GetComponent<Image>();

		selectButton.onClick.AddListener(() => SelectSprite(buttonImage.sprite));
	}

	private void SelectSprite(Sprite selectedSprite)
	{
		if(!ActivePatternManager.Instance.isWon)
		{
			return;
		}

		ArtSprite = selectedSprite;
		completeButtons.gameObject.SetActive(true);

		DialogueManager.Instance.CurrentDialogue.SetCurrentKnot("Part4");
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}

	public void YesButton()
	{
		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.ArtStyle));
		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		completeButtons.gameObject.SetActive(false);
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}

	public void NoButton()
	{
		completeButtons.gameObject.SetActive(false);
		MenuManager.Instance.CloseMenu<DialogueMenu>();
	}
}
