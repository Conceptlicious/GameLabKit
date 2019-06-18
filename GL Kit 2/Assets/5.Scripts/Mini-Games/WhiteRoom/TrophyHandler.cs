﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class TrophyHandler : BetterMonoBehaviour
{
	private Image[] trophySprites = new Image[6];

	private void Awake()
	{
		EventManager.Instance.AddListener<SaveItemEvent>(UpdateWhiteRoom);

		int currentLoop = 0;
		foreach(Transform child in transform)
		{
			Image currentImage = child.GetComponent<Image>();
			trophySprites[currentLoop] = currentImage;

			currentImage.gameObject.SetActive(false);

			currentLoop++;
		}
	}

	private void UpdateWhiteRoom(SaveItemEvent eventInfo)
	{
		switch(eventInfo.CurrentRoomType)
		{
			case RoomType.TargetAudience:
				trophySprites[(int)RoomType.TargetAudience].gameObject.SetActive(true);
				trophySprites[(int)RoomType.TargetAudience].sprite = SpriteHandler.personaSprite;
				break;

			case RoomType.Goals:
				trophySprites[(int)RoomType.Goals].gameObject.SetActive(true);
				trophySprites[(int)RoomType.Goals].sprite = 
					HiddenObjectHandler.Instance.lastSelectedObjectSprite;
				break;

			case RoomType.Genre:
				trophySprites[(int)RoomType.Genre].gameObject.SetActive(true);
				break;

			case RoomType.Medium:
				trophySprites[(int)RoomType.Medium].gameObject.SetActive(true);
				trophySprites[(int)RoomType.Medium].sprite =
					DisplayPlatformInformation.Instance.currentSelectedSprite;
				break;

			case RoomType.Dynamics:
				trophySprites[(int)RoomType.Dynamics].gameObject.SetActive(true);
				//trophySprites[(int)RoomType.Dynamics].sprite =
				//	ButtonManager.Instance.LastSelectedFunTypeSprite;
				break;

			case RoomType.ArtStyle:
				trophySprites[(int)RoomType.ArtStyle].gameObject.SetActive(true);
				break;

		}
	}
}