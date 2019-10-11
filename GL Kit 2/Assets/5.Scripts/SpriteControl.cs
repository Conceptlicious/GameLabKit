using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;
using UnityEngine.UI;
using System;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script allows for a number of sprites to be assigned to an image, and have the game cycle or pick 
//between them.
//Usage: Used for UI.
//--------------------------------------------------


public class SpriteControl : BetterMonoBehaviour
{
	[SerializeField] private Sprite[] sprites;
	private int spriteIndex = 0;

	[SerializeField]
	private ControlSettings settings;
	
	private Image image;
	public enum SwitchType { RANDOM, ITERATIVE }

	[Serializable]
	public struct ControlSettings
	{
		[ClassExtends(typeof(GameLabEvent))] public ClassTypeReference eventType;
		public SwitchType switchType;
	}
	
	void Start()
	{
		image = GetComponent<Image>();
		if (image == null)
		{
			gameObject.AddComponent<Image>();
		}
		image = gameObject.EnsureComponent<Image>();
		RegisterAllListeners();
	}
	
	
	/// <summary>
	/// Registers all event listeners this class needs to care about.
	/// </summary>
	private void RegisterAllListeners()
	{
		//EventManager.Instance.AddListener(settings.eventType );
	}


	private void OnBubbleFill()
	{
		ChangeSprite();
	}

	private void ChangeSprite()
	{
		switch (settings.switchType)
		{
				case SwitchType.RANDOM:
					image.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
					break;
					
				case SwitchType.ITERATIVE:
					spriteIndex++;
					image.sprite = sprites[spriteIndex % sprites.Length];
					break;
		}
		
	}
}
