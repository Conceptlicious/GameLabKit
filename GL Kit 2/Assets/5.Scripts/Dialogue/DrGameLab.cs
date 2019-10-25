using System.Collections;
using System.Collections.Generic;
using GameLab;
using UnityEngine;

public class DrGameLab : BetterMonoBehaviour
{
	[SerializeField] private float offScreenX = 0;
	[SerializeField] private float inScreenX = 0;
	[SerializeField] private float durationInSeconds;

	private void Awake()
	{
		EventManager.Instance.AddListener<DialogueCloseEvent>(MoveOut);
		EventManager.Instance.AddListener<DialogueOpenEvent>(MoveIn);
	}

	public void MoveIn()
	{
		LeanTween.moveLocalX(gameObject, inScreenX, durationInSeconds);
	}

	public void MoveOut()
	{
		LeanTween.moveLocalX(gameObject, offScreenX, durationInSeconds);
	}

}
