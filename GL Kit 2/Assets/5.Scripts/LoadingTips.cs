using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using GameLab;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

			//UIAnimator.Instance.AnimateObjects(slidingObject, Settings.VAL_CAMERA_TRANSITION_SECONDS, UIAnimator.MoveType.ARC);

			Debug.Log("Create loading tup");
			if (displayLoadingTips == true)
			{
				FillSpeechBubbleEvent newInfo = new FillSpeechBubbleEvent(dialogueObject, Settings.VAL_CAMERA_TRANSITION_SECONDS, UIAnimator.MoveType.ARC, method, true);
				EventManager.Instance.RaiseEvent(newInfo);
			}
			
		}
	}
}
