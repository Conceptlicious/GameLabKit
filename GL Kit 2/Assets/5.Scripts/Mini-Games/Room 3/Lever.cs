using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;
using Room3;

public class Lever : BetterMonoBehaviour
{

	[SerializeField] private Sprite upSprite;
	[SerializeField] private Sprite downSprite;
	private int pressCount = 1;

	public void OnButtonPressed()
	{
		
		TileGrid.Instance.SetGridInteractable(true);
		SwitchSprites();

		if(pressCount != 1)
		{
			return;
		}

		DialogueManager.Instance.CurrentDialogue.CurrentKnot = "Part2";
		MenuManager.Instance.OpenMenu<DialogueMenu>();

		++pressCount;
	}

	public void SwitchSprites()
	{
		Image image = GetComponent<Image>();
		if (image.sprite == upSprite)
		{
			image.sprite = downSprite;
		}
		else
		{
			image.sprite = upSprite;
		}

	}

}
