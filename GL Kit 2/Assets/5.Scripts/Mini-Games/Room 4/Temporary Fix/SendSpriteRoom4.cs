using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;

[DisallowMultipleComponent]
public class SendSpriteRoom4 : Manager<SendSpriteRoom4>
{
	[HideInInspector] public Sprite LastSelectedSprite;

	private Button confirmButton = null;

	private void Start()
	{
		confirmButton = GetComponent<Button>();
		confirmButton.onClick.AddListener(() => SendSprite());
	}

	private void SendSprite()
	{
		SaveItemEvent saveItemEvent = new SaveItemEvent(RoomType.Medium);
		EventManager.Instance.RaiseEvent(saveItemEvent);

		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}
}
