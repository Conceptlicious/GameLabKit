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
		if(pressCount == 1)
		{
			EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
			pressCount++;
		}
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
