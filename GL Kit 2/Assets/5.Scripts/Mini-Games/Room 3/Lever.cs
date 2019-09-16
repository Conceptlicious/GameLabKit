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

	public void OnButtonPressed()
	{		
		TileGrid.Instance.SetGridInteractable(true);
		SwitchSprites();
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
