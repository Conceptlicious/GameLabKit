using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;
using UnityEngine.UI;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script fills the speech bubbles that apper along Dr GameLab.
//Usage: Used along with scene transitions, UI and the dialogue system.
//--------------------------------------------------


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

		
		if(info.shouldAnimate)
		{
		    UIAnimatorManager.Instance.AnimateObjects(slidingObject, info.time, info.moveType, info.blurType);
		}
		
		
	}

	/// <summary>
	/// Fetch more dialogue from our file.
	/// </summary>
	/// <returns></returns>
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

    /// <summary>
    /// Get a version of our text that fit the length that can be displayed in a single bubble, and accounting for formatting and word-cutting.
    /// </summary>
    /// <returns></returns>
	private string GetSubdividedText()
	{
		//If we are beginning our subdivision, grab the dialogue and load it into the bubbleText string
		if (subdivisionIndex == 0)
		{
			bubbleText = GetTextFromDialogueObject();
		}
		
		
		//If our text is already shorter than the amount of characters that can fit in a bubble, just return it as is.
		if (bubbleText.Length <= Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE)
		{
			return bubbleText;
		}
		
		//450  200  2.25
		//How many speech bubbles it will take to display the full text
		float timesIn = bubbleText.Length / Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE;
		
		//The int version of the previous number. Float version contained the percentage of text that'd be in the last bubble.
		int ceilTimesIn = Mathf.CeilToInt(timesIn);
		
		//How many characters are left over in the last bubble.
		int remainingCharacters =
			bubbleText.Length - (Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE * Mathf.RoundToInt(timesIn));
		
		
		//A number that represents an offset for our starting point when using substring. If we left a few characters in the previous bubble, our start point
		//needs to be pushed back this amount for the current bubble.
		int accountedOffset = /*subdivisionIndex == ceilTimesIn ? 0 : */ (subdivisionIndex * Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE) - (lastOffset * subdivisionIndex);
		
		//How long is our substring
		int length = subdivisionIndex == ceilTimesIn 
			? remainingCharacters + accountedOffset
			: Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE;
		
		//Debugging
		Debug.Log("Subdiv: " + subdivisionIndex + " | Length: " + bubbleText.Length + " | Length of chars: " +  length + " | last offset: " + lastOffset + " | accounted offset: "  + accountedOffset + " | Remaining chars: " + remainingCharacters  + " | Times In Int: " + ceilTimesIn + " | BubbleText: " + bubbleText);
		
		//Our string, starting at the index of our 'loop' times how many characters are in a bubble, and accounting for offset
		string subdivision = bubbleText.Substring((subdivisionIndex * Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE) - accountedOffset,
			length);
			
	    //Debugging
		Debug.Log("Subbing from " + ((subdivisionIndex * Settings.VAL_CHARACTERS_PER_SPEECH_BUBBLE) - accountedOffset) +
		          " for " + length + " chars.");
		          
		//The position of the last space, used so that the final word is not cut off mid-way
		lastOffset = subdivision.LastIndexOf(' ');
		
		//If our 'loop' index is under the amount of times we should do this
		if (subdivisionIndex < ceilTimesIn)
		{
			subdivision = subdivision.Substring(0, lastOffset);
			Debug.Log("Will sub the existing sub for " + lastOffset + " chars");
		}
			
		
		//Increment
	    subdivisionIndex++;
		
		//If we are done
		if (subdivisionIndex > ceilTimesIn)
		{
		    //Reset our values
			subdivisionIndex = 0;
			lastOffset = 0;
			
			//Ideally, an event should be called here to broadcast the full text is read, but that's for later
			if (dialogueObject.Info.fieldIndex == 0)
			{
				Debug.Log("Fully read");
				completeReading = true;
			}
		}

		
		//Get rid of an extra spaces that might be left in
		return subdivision.Trim(' ');
	}
	
	 
}


