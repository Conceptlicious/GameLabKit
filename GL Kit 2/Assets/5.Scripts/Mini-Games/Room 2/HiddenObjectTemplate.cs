using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public enum GoalType
{
	Awarenss,
	Education,
	Health,
	Training,
	Advertisement,
	Activation,
	Recruitment,
	Research
};

[CreateAssetMenu(fileName = "New HiddenObject", menuName = "HiddenObject")]
public class HiddenObjectTemplate : ScriptableObject
{
	public GoalType goalType;
	[TextArea] public string description;
	public Sprite notFoundSprite;
	public Sprite foundSprite;
}