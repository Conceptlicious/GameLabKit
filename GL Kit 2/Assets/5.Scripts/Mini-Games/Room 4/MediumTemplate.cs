using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;

[CreateAssetMenu(fileName = "new Medium", menuName = "Room 4/Medium")]
public class MediumTemplate : ScriptableObject
{
	public Sprite icon;
	public new string name;
	[TextArea] public string description;
	public MediumType mediumType;
}
