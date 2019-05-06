using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;


public class ButtonManager : Singleton<ButtonManager>
{
	public Button easyFun;
	public Button peopleFun;
	public Button hardFun;
	public Button seriousFun;

	[SerializeField] private UIHandler uiHandler = null;
	
	private int activeButtonCount = 1;

	private void Start()
	{
		easyFun.enabled = true;
		peopleFun.enabled = false;
		hardFun.enabled = false;
		seriousFun.enabled = false;
	}

	public void EnableNextButton()
	{
		activeButtonCount++;
		switch(activeButtonCount)
		{
			case 2:
				peopleFun.enabled = true;
				break;
			case 3:
				hardFun.enabled = true;
				break;
			case 4:
				seriousFun.enabled = true;
				break;
			case 5:
				uiHandler.typeText.text = "You won the minigame!";
				break;
		}
	}
}
