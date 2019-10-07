using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[RequireComponent(typeof(Image))]
public class Flask : BetterMonoBehaviour
{
	public static Sprite FlaskSprite { get; private set; }
	[SerializeField] private Image completeButtons;

	public void OnFlaskSelected()
	{
		//Set the trophy to this
		FlaskSprite = GetComponent<Image>().sprite;

		completeButtons.gameObject.SetActive(true);
		EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
		//EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Genre));
		//EventManager.Instance.RaiseEvent(new NextRoomEvent());
	}

	public void YesButton()
	{
		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Genre));
		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		completeButtons.gameObject.SetActive(false);
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}

	public void NoButton()
	{
		completeButtons.gameObject.SetActive(false);
		DialogueProgression.Instance.RemoveChildren();
		ProgressDialogueEvent.ResetKnotID(8);
	}
}
