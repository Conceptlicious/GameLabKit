using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class HiddenObject : BetterMonoBehaviour
{
	[SerializeField][TextArea] private string description = string.Empty;
	public string Description => description;
	private Image hiddenObjectImage = null;
	public Sprite HiddenObjectSprite { get; private set; }

	private void Start()
	{
		hiddenObjectImage = GetComponent<Image>();
		HiddenObjectSprite = hiddenObjectImage.sprite;
	}
}
