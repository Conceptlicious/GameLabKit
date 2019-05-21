using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
