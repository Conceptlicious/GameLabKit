using UnityEngine.UI;
using UnityEngine;
using GameLab;


public class ButtonManager : Singleton<ButtonManager>
{
	public Button EasyFun { get; private set; }
	public Button PeopleFun { get; private set; }
	public Button HardFun { get; private set; }
	public Button SeriousFun { get; private set; }
	private int activeButtonCount = 1;
	private Text easyFunText = null;
	private Text peopleFunText = null;
	private Text hardFunText = null;
	private Text seriousFunText = null;

	protected override void Awake()
	{
		base.Awake();
		SetVariables();	   		
	}

	public void EnableNextButton()
	{
		activeButtonCount++;
		switch(activeButtonCount)
		{		
			case 2:
				PeopleFun.enabled = true;
				peopleFunText.color = Color.green;
				break;
			case 3:
				HardFun.enabled = true;
				hardFunText.color = Color.green;
				break;
			case 4:
				SeriousFun.enabled = true;
				seriousFunText.color = Color.green;
				break;
			case 5:
				UIHandler.Instance.TypeText.text = "You won the mini game!";
				UIHandler.Instance.WonMiniGame();
				break;
		}
	}

	private void SetVariables()
	{
		EasyFun = transform.Find("EasyFun").GetComponent<Button>();
		easyFunText = EasyFun.GetComponentInChildren<Text>();
		EasyFun.enabled = true;
		easyFunText.color = Color.green;

		PeopleFun = transform.Find("PeopleFun").GetComponent<Button>();
		peopleFunText = PeopleFun.GetComponentInChildren<Text>();
		PeopleFun.enabled = false;
		peopleFunText.color = Color.red;

		HardFun = transform.Find("HardFun").GetComponent<Button>();
		hardFunText = HardFun.GetComponentInChildren<Text>();
		HardFun.enabled = false;
		hardFunText.color = Color.red;

		SeriousFun = transform.Find("SeriousFun").GetComponent<Button>();
		seriousFunText = SeriousFun.GetComponentInChildren<Text>();
		SeriousFun.enabled = false;
		seriousFunText.color = Color.red;
	}
}
