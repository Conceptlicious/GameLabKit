using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class TrophySocket : BetterMonoBehaviour
{
	[SerializeField] RoomType neededRoomType;

	private bool isOccupied = false;

	public void Occupy(Transform trophy)
	{
		trophy.position = trophy.GetComponent<TrophyDrag>().BeginPosition;

		if (isOccupied)
		{
			return;
		}

		if (IsRightThrophy(trophy))
		{
			isOccupied = true;
			SnapToSocket(trophy.GetComponent<Image>().sprite);
			Destroy(trophy.gameObject);

			StartCoroutine(NextRoomDelay());
			return;
		}
	}

	private bool IsRightThrophy(Transform trophy)
	{
		//If trophy room type is the same is needed room type then return true, else return false;
		TrophyDrag trophyDrag = trophy.GetComponent<TrophyDrag>();

		if (neededRoomType == trophyDrag.roomType)
		{
			return true;
		}

		return false;
	}

	private void SnapToSocket(Sprite trophySprite)
	{
		Image socketImage = GetComponent<Image>();

		socketImage.sprite = trophySprite;
		socketImage.color = new Color32(255, 255, 255, 255);
	}

	private IEnumerator NextRoomDelay()
	{
		yield return new WaitForSeconds(TrophyGridManager.Instance.NextRoomDelaySeconds);

		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}
}