using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using GameLab;
using UnityEngine.Serialization;
using UnityEngine.UI;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script interfaces with the UIAnimator and SpeechBubble in order to create similar events, but
//used during scene transition to provide tips to the player.
//Usage: Used on scene transition.
//--------------------------------------------------

public class LoadingTips : MonoBehaviour
{
	[SerializeField] private DialogueObject dialogueObject = null;
	[SerializeField] private bool tipsAreRandom = false;
	[SerializeField] private bool displayLoadingTips = true;

	private int tipsIndex = 0;

	private delegate void Updateables();
	private Updateables handler;

	// Start is called before the first frame update
	void Start()
	{
		RegisterAllListeners();
	}

	/// <summary>
	/// Registers all event listeners this class needs to care about.
	/// </summary>
	private void RegisterAllListeners()
	{
		EventManager.Instance.AddListener<CameraTargetSelectEvent>(OnTransition);
	}

	private void OnTransition(CameraTargetSelectEvent info)
	{
		SpeechBubble.FillTextMethod method;
		if (info != null & info.showTips == true)
		{
			method = tipsAreRandom == true
				? SpeechBubble.FillTextMethod.RANDOM
				: SpeechBubble.FillTextMethod.ITERATE;

			Debug.Log("Create loading tip");
			if (displayLoadingTips == true)
			{
				//Make button interactable false
				FillSpeechBubbleEvent newInfo = new FillSpeechBubbleEvent(dialogueObject, Settings.VAL_CAMERA_TRANSITION_SECONDS, UIAnimator.MoveType.ARC, UIAnimator.BlurType.NONE, method, true);
				EventManager.Instance.RaiseEvent(newInfo);
			}
			
		}
	}
}
