using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

[CreateAssetMenu(fileName = "New HiddenObject", menuName = "HiddenObject")]
public class HiddenObjectTemplate : ScriptableObject
{
	public new string name;
	public string description;
	public Sprite notFoundSprite;
	public Sprite foundSprite;
}