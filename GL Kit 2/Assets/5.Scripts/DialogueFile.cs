using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script is the highest level of information for each file of dialogue that is read and parsed. 
//It's used to create and store the containers.
//Usage: Used as part of the dialogue system.
//--------------------------------------------------


public class DialogueFile
{
	public string Name { get; set; } = Settings.STR_DEFAULT_DIALOGUE;

	private DialogueContainer[] containers = null;

	public void CreateContainers(int size)
	{
		containers = new DialogueContainer[size];
		for (int i = 0; i < size; i++)
		{
			containers[i] = new DialogueContainer();
		}

	}

	public void SetContainerInfo(int index, string name, string key, string value)
	{
		if (containers != null)
		{
			containers[index].Name = name;
			containers[index].SetInfo(key, value);
		}
	}

	public DialogueContainer[] GetContainers()
	{
		Debug.Assert(containers != null, Settings.ERR_ASSERT_DIAG_CONTAINERS_NULL);
		return containers;
	}

	public DialogueContainer GetContainer(int pIndex)
	{
		Debug.Assert(containers != null, Settings.ERR_ASSERT_DIAG_CONTAINERS_NULL);
		Debug.Assert(pIndex >= 0 && pIndex < containers.Length, Settings.ERR_ASSERT_DIAG_CONTAINERS_INVALID_INDEX);
		return containers[pIndex];
	}
}
