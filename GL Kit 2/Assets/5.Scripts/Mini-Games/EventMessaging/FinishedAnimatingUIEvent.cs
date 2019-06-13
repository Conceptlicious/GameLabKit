using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class FinishedAnimatingUIEvent : GameLabEvent
{
	public UIAnimator animator;
	public FinishedAnimatingUIEvent(UIAnimator pAnimator)
	{
		animator = pAnimator;
	}
}
