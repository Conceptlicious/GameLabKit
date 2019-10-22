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

	private string currentKnot = string.Empty;
	public string CurrentKnot
	{
		get => currentKnot;
		set
		{
			currentKnot = value;
			InkStory.ChoosePathString(currentKnot);
		}
	}

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

	/// <summary>
	/// Returns true if the story can no longer continue and there are no choices left.
	/// </summary>
	public bool IsKnotFinished => !InkStory.canContinue && InkStory.currentChoices.Count == 0;

	public void Reset(string knotToStartFrom = null)
	{
		InkStory.ResetState();

		if(!string.IsNullOrEmpty(knotToStartFrom))
		{
			CurrentKnot = knotToStartFrom;
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

	public void SetStringVariable(string variableName, string value) => inkStory.variablesState[variableName] = value;
	public string GetStringVariable(string variableName) => inkStory.variablesState[variableName].ToString();

	public void SetIntVariable(string variableName, int value) => inkStory.variablesState[variableName] = value;
	public int GetIntVariable(string variableName) => (int)(inkStory.variablesState[variableName]);
	
	public void SetBoolVariable(string variableName, bool value) => inkStory.variablesState[variableName] = value;
	public bool GetBoolVariable(string variableName) => (bool)(inkStory.variablesState[variableName]);

	private void InitializeStory()
	{
		Debug.Assert(InkFileAsset != null && !string.IsNullOrEmpty(InkFileAsset.text));

		InkStory = new Story(InkFileAsset.text);
	}
}

