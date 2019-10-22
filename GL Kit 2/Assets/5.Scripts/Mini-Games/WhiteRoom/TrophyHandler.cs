using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class TrophyHandler : BetterMonoBehaviour
{
	[SerializeField] Image targetAudeniceImage;
	private Image[] trohpyImages;

	private void Awake()
	{
		EventManager.Instance.AddListener<SaveItemEvent>(OnItemSaved);
		SetVariables();
	}

	private void OnItemSaved(SaveItemEvent eventData)
	{
		switch (eventData.CurrentRoomType)
		{
			case RoomType.TargetAudience:
				targetAudeniceImage.gameObject.SetActive(true);
				targetAudeniceImage.color = Color.white;
				targetAudeniceImage.sprite = SpriteHandler.personaSprite;
				break;
			case RoomType.Goals:
				trohpyImages[(int)RoomType.Goals].gameObject.SetActive(true);
				trohpyImages[(int)RoomType.Goals].sprite =
					HiddenObjectHandler.Instance.SelectedObjectSprite;
				break;
			case RoomType.Genre:
				trohpyImages[(int)RoomType.Genre].gameObject.SetActive(true);
				trohpyImages[(int)RoomType.Genre].sprite = Flask.FlaskSprite;
				break;
			case RoomType.Medium:
				trohpyImages[(int)RoomType.Medium].gameObject.SetActive(true);
				trohpyImages[(int)RoomType.Medium].sprite = MediumUIHandler.Instance.SelectedMediumSprite;
				break;
			case RoomType.Dynamics:
				trohpyImages[(int)RoomType.Dynamics].gameObject.SetActive(true);
				trohpyImages[(int)RoomType.Dynamics].sprite =
					ButtonManager.Instance.LastClickedButtonSprite;
				break;
			case RoomType.ArtStyle:
				trohpyImages[(int)RoomType.ArtStyle].gameObject.SetActive(true);
				trohpyImages[(int)RoomType.ArtStyle].sprite = SelectButton.ArtSprite;
				break;
		}
	}

	private void SetVariables()
	{
		trohpyImages = GetComponentsInChildren<Image>();
		foreach(Image trophyImage in trohpyImages)
		{
			trophyImage.gameObject.SetActive(false);
		}

		targetAudeniceImage.gameObject.SetActive(false);
	}
}