using System;
using System.Collections;
using System.Collections.Generic;
using GameLab;
using UnityEngine;

public class DrGameLab : BetterMonoBehaviour
{
	public event Action<bool> DrGameLabMovementCompleted = null;

	[SerializeField]
	
	public void MoveDrGameLab(bool moveIn)
	{
		//LeanTween.move(gameObject, moveIn ? onScreenDestination : offScreenDestination, durationInSeconds).setOnComplete(() => DrGameLabMovementCompleted(moveIn));
	}

	private void DrGameLabMovedIn()
	{
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}
}
