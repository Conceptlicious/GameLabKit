using System;
using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class RoomStory
{
	[SerializeField] private RoomType roomID = RoomType.WhiteRoom;
	public RoomType RoomID => roomID;

	[SerializeField] private TextAsset inkFileAsset = null;
	public TextAsset InkFileAsset => inkFileAsset;

	[SerializeField] private Story inkStory = null;
	public Story InkStory => inkStory;

	public void Initialize()
	{
		Debug.Assert(InkFileAsset != null && !string.IsNullOrEmpty(InkFileAsset.text));

		inkStory = new Story(InkFileAsset.text);
	}

	public void PlayKnot(string knotName)
	{
		inkStory.ChoosePathString(knotName);
	}

	public string GetNextLine()
	{
		if(inkStory.canContinue)
		{
			return inkStory.Continue();
		}
		else
		{
			return null;
		}
	}

}

