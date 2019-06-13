﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;
using UnityEngine.UI;
using System;


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
		RegisterAllListeners();
	}
	
	
	/// <summary>
	/// Registers all event listeners this class needs to care about.
	/// </summary>
	private void RegisterAllListeners()
	{
		//EventManager.Instance.AddListener(settings.eventType );
		EventManager.Instance.AddListener<FillSpeechBubbleEvent>( OnBubbleFill );
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
