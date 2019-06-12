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
	private string 

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
		fillTextMethod = info.method;
		//Only overwrite if required, otherwise utalise the last assigned dialogue object
		if (info.dialogueObject != null)
		{
			dialogueObject = info.dialogueObject;
		}
		else
		{
			Debug.Log("Passed dialogue object is null.");
		}

		//Our internal dialogue object
		if (dialogueObject != null)
		{
			Debug.Log("Reading from" + dialogueObject.GetFileName());
			switch (fillTextMethod)
			{
				case FillTextMethod.RANDOM:
					textField.text = dialogueObject.GetRandomText();
					break;
				
				case FillTextMethod.ITERATE:
					textField.text = dialogueObject.GetTextAndIterate();
					break;
				case FillTextMethod.NONE:
	
					break;
			
			}		
		}
		else
		{
			Debug.Log("Assigned dialogue object has been nullifed, or has not been assigned!");
		}

			

		
		if(info.shouldAnimate)
		{
		    UIAnimatorManager.Instance.AnimateObjects(slidingObject, info.time, info.moveType, info.blurType);
		}
		
		
	}


	private void SubText(string pText)
	{
		if (pText.Length <= Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE)
		{
			return;
		}

		//
		float timesIn = pText.Length / Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE;
		
		int remainingCharacters =
			pText.Length - (Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE * Mathf.RoundToInt(timesIn));
		
		
		string[] parts = new string[Mathf.CeilToInt(timesIn)];
		
		for (int i = 0; i < parts.Length; i++)
		{
			//Use the full length if we're not at the end, else use the remaining characters
			int length = i != parts.Length ? Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE : remainingCharacters;
			
			parts[i] = pText.Substring(i * Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE,
				length);
		}
	}
	
	 
}


