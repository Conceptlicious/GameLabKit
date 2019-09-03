using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class SendSpriteRoom6 : Singleton<SendSpriteRoom6>
{
	public Sprite LastSelectedTubeSprite { get; private set; }
	private GameObject sendSpriteOverlay = null;

	protected override void Awake()
	{
		base.Awake();
		SetVariables();
	}

	public void WonMinigame()
	{
		sendSpriteOverlay.SetActive(true);
	}


	public void TubeSeleced(Image selectedTubeImage)
	{
		LastSelectedTubeSprite = selectedTubeImage.sprite;
	}

	private void ConfirmButton()
	{
		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.ArtStyle));
		EventManager.Instance.RaiseEvent(new NextRoomEvent());
	}

	private void SetVariables()
	{
		sendSpriteOverlay = gameObject;

		sendSpriteOverlay.transform.Find("ConfirmButton")
			.GetComponent<Button>().onClick.AddListener(() => ConfirmButton());

		sendSpriteOverlay.SetActive(false);
	}
}
