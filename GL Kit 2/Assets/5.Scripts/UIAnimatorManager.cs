using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script is the publicly accessable point for using UIAnimators. It's a singleton to allow for access
//whereas each UIAnimator needs to exists independantly so their animations can play out and overlap.
//It creates, removes and handles those animators.
//Usage: Used with the UI system.
//--------------------------------------------------

public class UIAnimatorManager : Singleton<UIAnimatorManager>
{
	private List<UIAnimator> animators = new List<UIAnimator>();
	
	private void OnEnable()
	{
		RegisterAllListeners();
	}

	private void RegisterAllListeners()
	{
		EventManager.Instance.AddListener<FinishedAnimatingUIEvent>(RemoveAnimatorFromList);
	} 
	
	public void AnimateObjects(UISlidingObject[] pSlidingObject, float pLengthOfAnimation, UIAnimator.MoveType pMoveType, UIAnimator.BlurType pBlurType)
	{
		animators.Add(new UIAnimator(pSlidingObject, pLengthOfAnimation, pMoveType, pBlurType));
	}

	void FixedUpdate()
	{
		for (int i = 0; i < animators.Count; i++)
		{
			animators[i].FixedUpdateAnimator();
		}
	}

	private void RemoveAnimatorFromList(FinishedAnimatingUIEvent info)
	{
		if (info.animator != null)
		{
			animators.Remove(info.animator);
		}
	}
}
