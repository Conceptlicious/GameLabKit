using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class DismissSpeechBubbleEvent : GameLabEvent
{
	public DismissSpeechBubbleEvent()
	{
		FillSpeechBubbleEvent repeatInfo = new FillSpeechBubbleEvent(null, Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, UIAnimator.BlurType.OUT, SpeechBubble.FillTextMethod.NONE, true);
		EventManager.Instance.RaiseEvent(repeatInfo);
	}

}
