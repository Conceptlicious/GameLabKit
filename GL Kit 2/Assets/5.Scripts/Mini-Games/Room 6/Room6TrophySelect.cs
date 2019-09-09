using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;

public class Room6TrophySelect : BetterMonoBehaviour
{
	[SerializeField] private Image selectedStyle;
	[SerializeField] private Image trophy;


	public void ConfirmSelection()
	{
		trophy.overrideSprite = selectedStyle.sprite;
		//trophy.color = new Color32(255, 0, 0, 255);
	}

	public void NewSelection(Image selecedImage)
	{
		selectedStyle = selecedImage;
	}

}
