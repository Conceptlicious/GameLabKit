using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class FillSpeechBubbleEvent : GameLabEvent
{
	public DialogueObject dialogueObject;
	public float time;
	public UIAnimator.MoveType moveType;
	public UIAnimator.BlurType blurType;
	public SpeechBubble.FillTextMethod method;
	public bool shouldAnimate;
	
	public FillSpeechBubbleEvent(DialogueObject pDialogueObject, float pTime, UIAnimator.MoveType pMoveType, UIAnimator.BlurType pBlurType, SpeechBubble.FillTextMethod pFillTextMethod, bool pShouldAnimate)
	{
		dialogueObject = pDialogueObject;
		time = pTime;
		moveType = pMoveType;
		blurType = pBlurType;
		method = pFillTextMethod;
		shouldAnimate = pShouldAnimate;		
	}
}
