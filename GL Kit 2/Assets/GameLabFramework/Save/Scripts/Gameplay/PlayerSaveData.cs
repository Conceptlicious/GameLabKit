using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameLab;
using Newtonsoft.Json;

[Serializable]
public struct PlayerSaveData
{
	public string Name;
	public float Health;

	[JsonProperty]
	private int currentHealth;

	public void SetCurrentHealth(int h)
	{
		currentHealth = h;
	}
}
