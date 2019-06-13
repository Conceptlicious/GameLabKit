using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class UIAnimatorManager : Singleton<UIAnimatorManager>
{
	private List<UIAnimator> animators = new List<UIAnimator>();
	
	void Awake()
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
