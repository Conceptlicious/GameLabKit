using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class SelectButton : BetterMonoBehaviour
{
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
		SaveItemEvent saveItemEvent = new SaveItemEvent(RoomType.ArtStyle);
		EventManager.Instance.RaiseEvent(saveItemEvent);

		NextRoomEvent newInfo = new NextRoomEvent();
		EventManager.Instance.RaiseEvent(newInfo);
	}
}
