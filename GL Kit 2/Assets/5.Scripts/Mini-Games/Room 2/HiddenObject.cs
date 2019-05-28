using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class HiddenObject : BetterMonoBehaviour
{
	[SerializeField][TextArea] private string description = string.Empty;
	public string Description => description;
}
