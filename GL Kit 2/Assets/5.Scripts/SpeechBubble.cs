using System;
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
	private string bubbleText = "";
	private int subdivisionIndex = 0;
	private int lastOffset = 0;
	private bool completeReading = false;

	public enum FillTextMethod
	{
		RANDOM,
		ITERATE,
		NONE,
		TOTAL
	};

	private FillTextMethod fillTextMethod;
	public DialogueObject DiagObject => dialogueObject;
	public bool Complete => completeReading;
	
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

		textField.text = GetSubdividedText();
		/*
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
					textField.text = GetSubdividedText();//dialogueObject.GetTextAndIterate();
					break;
				case FillTextMethod.NONE:
	
					break;
			
			}		
		}
		else
		{
			Debug.Log("Assigned dialogue object has been nullifed, or has not been assigned!");
		}*/

			

		
		if(info.shouldAnimate)
		{
		    UIAnimatorManager.Instance.AnimateObjects(slidingObject, info.time, info.moveType, info.blurType);
		}
		
		
	}

	private string GetTextFromDialogueObject()
	{
		string output = Settings.STR_DEFAULT_DIALOGUE;
		//Our internal dialogue object
		if (dialogueObject != null)
		{
			Debug.Log("Reading from" + dialogueObject.GetFileName());
			switch (fillTextMethod)
			{
				case FillTextMethod.RANDOM:
					output = dialogueObject.GetRandomText();
					break;
				
				case FillTextMethod.ITERATE:
					output = dialogueObject.GetTextAndIterate();
					break;
				case FillTextMethod.NONE:
	
					break;
			
			}		
		}
		else
		{
			Debug.Log("Assigned dialogue object has been nullifed, or has not been assigned!");
		}

		return output;
	}


	private string GetSubdividedText()
	{
		
		if (subdivisionIndex == 0)
		{
			bubbleText = GetTextFromDialogueObject();
		}
		
		
		
		if (bubbleText.Length <= Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE)
		{
			return bubbleText;
		}
		
		//450  200  2.25
		float timesIn = bubbleText.Length / Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE;
		int ceilTimesIn = Mathf.CeilToInt(timesIn);
		int remainingCharacters =
			bubbleText.Length - (Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE * Mathf.RoundToInt(timesIn));
		
		
		
		

		
		
		int accountedOffset = /*subdivisionIndex == ceilTimesIn ? 0 : */ (subdivisionIndex * Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE) - (lastOffset * subdivisionIndex);
		
		
		int length = subdivisionIndex == ceilTimesIn 
			? remainingCharacters + accountedOffset
			: Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE;
		
		Debug.Log("Subdiv: " + subdivisionIndex + " | Length: " + bubbleText.Length + " | Length of chars: " +  length + " | last offset: " + lastOffset + " | accounted offset: "  + accountedOffset + " | Remaining chars: " + remainingCharacters  + " | Times In Int: " + ceilTimesIn + " | BubbleText: " + bubbleText);
		
		string subdivision = bubbleText.Substring((subdivisionIndex * Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE) - accountedOffset,
			length);
		Debug.Log("Subbing from " + ((subdivisionIndex * Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE) - accountedOffset) +
		          " for " + length + " chars.");
		lastOffset = subdivision.LastIndexOf(' ');
		if (subdivisionIndex < ceilTimesIn)
		{
			subdivision = subdivision.Substring(0, lastOffset);
			Debug.Log("Will sub the existing sub for " + lastOffset + " chars");
		}
		
		
		
		
		
	    subdivisionIndex++;
		
		if (subdivisionIndex > ceilTimesIn)
		{
			subdivisionIndex = 0;
			lastOffset = 0;
			if (dialogueObject.Info.fieldIndex == 0)
			{
				Debug.Log("Fully read");
				completeReading = true;
			}
		}

		

		return subdivision.Trim(' ');
		/*
		
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
		}*/
	}
	
	 
}


