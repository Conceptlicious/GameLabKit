using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script holds the name of top-level arrays in our JSON dialogue files, along with that it holds
//a dictionary which holds the names and values of the fields contained within.
//Usage: Used as part of the dialogue system.
//--------------------------------------------------


public class DialogueContainer
{
	public string Name { get; set; } = Settings.STR_DEFAULT_DIALOGUE;
	private Dictionary<string, string> info = new Dictionary<string, string>();


	public void SetInfo(string key, string value)
	{
		info.Add(key, value);
	}

	public Dictionary<string, string> GetInfoDictionary()
	{
		return info;
	}	
}
