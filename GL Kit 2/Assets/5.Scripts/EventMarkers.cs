using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using GameLab;
using System;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script holds a serialised struct which links characters, known as 'markers', with events. These are
//designer-spesified. Part of the dialogue system will pass this script some text from a json file. This script
//will analyse it for a marker and, if found, strip that marker out, return the cleaned text and call the linked event.
//Usage: Used as part of the both dialogue system and the event system in order to 'script' from within the dialogue files.
//--------------------------------------------------

[Serializable]
public struct MarkedEvents
{
	[Tooltip("Char markers found in the dialogue file, used to call events.")]
	public string marker;
	[Tooltip("Type of event to be fired when specified marker is detected.")]
	[ClassExtends(typeof(GameLabEvent))] public ClassTypeReference eventToCall;
}

public class EventMarkers : Singleton<EventMarkers>
{
	[SerializeField] private MarkedEvents[] couplings;

	public string ParseAndCall(string pText)
	{
		string output = pText;
		//Debug.Log("Parsing");
	   
		for (int i = 0; i < couplings.Length; i++)
		{
			if (AnalyseText(couplings[i].marker, output, out output) && couplings[i].eventToCall != null)
			{
				Debug.Log("Enter call, calling " + couplings[i].eventToCall.ToString());
				
				EventManager.Instance.RaiseEvent(  Activator.CreateInstance(couplings[i].eventToCall).GetType() );
			}
		}

		return output;
	}
	
	/// <summary>
	/// Checks whether the text contains the given symbol, and outs a cleaned version of that string sans symbol.
	/// </summary>
	/// <param name="pMarker"></param>
	/// <param name="pText"></param>
	/// <param name="pOutText"></param>
	/// <returns></returns>
	private bool AnalyseText(string pMarker, string pText, out string pOutText)
	{
		bool output;
		pOutText = pText;
		if(pOutText.Contains(pMarker) == false)
		{
		   
			output = false;
		}
		else
		{          
			output = true;
			string[] parts = pOutText.Split(pMarker.ToCharArray());
			pOutText = "";
			for (int i = 0; i < parts.Length; i++)
			{
				pOutText += parts[i];
			}
		}
		//Debug.Log("Output: " + output);
		return output;
	}

	void OnValidate()
	{
		for (int i = 0; i < couplings.Length; i++)
		{
			if (String.IsNullOrEmpty(couplings[i].marker))
			{
				couplings[i].marker = Settings.DEFAULT_EVENTMARKER_MARKER;
			}       
		}
	}
	
   
}
