using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[RequireComponent(typeof(Image))]
public class Flask : BetterMonoBehaviour
{
	public void OnFlaskSelected()
	{
		//Set the trophy to this
		SaveItemEvent saveItemEvent = new SaveItemEvent(RoomType.Genre);
		EventManager.Instance.RaiseEvent(saveItemEvent);

		NextRoomEvent newInfo = new NextRoomEvent();
		EventManager.Instance.RaiseEvent(newInfo);
	}
}
