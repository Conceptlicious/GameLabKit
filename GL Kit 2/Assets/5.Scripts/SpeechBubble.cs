using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;
using UnityEngine.UI;

public class SpeechBubble : Singleton<SpeechBubble>
{
	[SerializeField] private UISlidingObject[] slidingObject;
	[SerializeField] private Text textField = null;
	private DialogueObject dialogueObject = null;

	public enum FillTextMethod
	{
		RANDOM,
		ITERATE,
		NONE,
		TOTAL
	};

	private FillTextMethod fillTextMethod;
	public DialogueObject DiagObject => dialogueObject;
	void Awake()
	{
		RegisterAllListeners();
	}
	
	/// <summary>
	/// Registers all event listeners this class needs to care about.
	/// </summary>
	private void RegisterAllListeners()
	{
		EventManager.Instance.AddListener<FillSpeechBubbleEvent>(OnBubbleFill);
	}

	private void OnBubbleFill(FillSpeechBubbleEvent info)
	{
		if (info.dialogueObject != null)
		{
			dialogueObject = info.dialogueObject;
			
			switch (fillTextMethod)
			{
				case FillTextMethod.RANDOM:
					textField.text = info.dialogueObject.GetRandomText();
					break;
				case FillTextMethod.ITERATE:
					textField.text = info.dialogueObject.GetTextAndIterate();
					break;
				case FillTextMethod.NONE:
	
					break;
			
			}
		}
		

			

		
		
		UIAnimator.Instance.AnimateObjects(slidingObject, info.time, info.moveType);
		
	}
	 
}


