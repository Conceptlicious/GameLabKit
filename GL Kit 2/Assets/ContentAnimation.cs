using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class ContentAnimation : BetterMonoBehaviour
{
	private Animator contentAnimator = null;

	private Menu contentOwner = null;
	private bool didFlyOut = true; // Start of as true because the menu is closed by default

	private void Awake()
	{
		contentAnimator = GetComponent<Animator>();
		contentOwner = GetComponentInParent<Menu>();

		contentOwner.Opened += OnContentOwnerOpened;
		contentOwner.Closing += OnContentOwnerClosing;
	}

	private void OnDestroy()
	{
		contentOwner.Opened -= OnContentOwnerOpened;
		contentOwner.Closing -= OnContentOwnerClosing;
	}

	private void OnContentOwnerOpened(Menu menu)
	{
		didFlyOut = false;
		contentAnimator.SetTrigger("FlyIn");
	}

	private bool OnContentOwnerClosing(Menu menu)
	{
		if(!didFlyOut)
		{
			contentAnimator.SetTrigger("FlyOut");
			return false;
		}

		return true;
	}

	private void OnFlyOutCompleted()
	{
		didFlyOut = true;
		contentOwner.Close();
	}


}
