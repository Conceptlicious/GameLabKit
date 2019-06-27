using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Mathias Bevers
//Overview: This script contains all the info of the hidden items. All of it is set in the inspector.
//This info is later read in the hiddenItemHandler;
//Usage: On every item in the "CollectedHiddenObjectBar".
//--------------------------------------------------

[RequireComponent(typeof(MonoBehaviour))]
public class HiddenObject : BetterMonoBehaviour
{
	public Sprite HiddenObjectSprite { get; private set; }
	[SerializeField] [TextArea] private string description = string.Empty;
	public string Description => description;
	private Image hiddenObjectImage = null;

	private void Start()
	{
		hiddenObjectImage = GetComponent<Image>();
		HiddenObjectSprite = hiddenObjectImage.sprite;
	}
}
