using UnityEngine.UI;
using GameLab;


public class ButtonManager : Singleton<ButtonManager>
{
	public Button EasyFun { get; private set; }
	public Button PeopleFun { get; private set; }
	public Button HardFun { get; private set; }
	public Button SeriousFun { get; private set; }
	private int activeButtonCount = 1;

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
				break;
			case 3:
				HardFun.enabled = true;
				break;
			case 4:
				SeriousFun.enabled = true;
				break;
			case 5:
				UIHandler.Instance.TypeText.text = "You won the mini game!";
				break;
		}
	}

	private void SetVariables()
	{
		EasyFun = transform.Find("EasyFun").GetComponent<Button>();
		PeopleFun = transform.Find("PeopleFun").GetComponent<Button>();
		HardFun = transform.Find("HardFun").GetComponent<Button>();
		SeriousFun = transform.Find("SeriousFun").GetComponent<Button>();

		EasyFun.enabled = true;  
		PeopleFun.enabled = false;
		HardFun.enabled = false;
		SeriousFun.enabled = false;
	}
}
