using System;
using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using GameLab;

[CreateAssetMenu]
public class RoomStory : RuntimeScriptableObject
{
	[SerializeField] private RoomType roomID = RoomType.WhiteRoom;
	public RoomType RoomID => roomID;

	[SerializeField] private TextAsset inkFileAsset = null;
	public TextAsset InkFileAsset => inkFileAsset;

	private Story inkStory = null;
	public Story InkStory
	{
		get
		{
			if(inkStory == null)
			{
				InitializeStory();
			}

			return inkStory;
		}

		set => inkStory = value;
	}

	public void SetCurrentKnot(string knotName) => InkStory.ChoosePathString(knotName);

	public void Reset(string knotToStartFrom = null)
	{
		InkStory.ResetState();

		if(!string.IsNullOrEmpty(knotToStartFrom))
		{
			SetCurrentKnot(knotToStartFrom);
		}
	}

	public string GetNextLine()
	{
		if(InkStory.canContinue)
		{
			return InkStory.Continue();
		}
		else
		{
			return null;
		}
	}


	private void InitializeStory()
	{
		Debug.Assert(InkFileAsset != null && !string.IsNullOrEmpty(InkFileAsset.text));

		InkStory = new Story(InkFileAsset.text);
	}
}

