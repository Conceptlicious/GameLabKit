using GameLab;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlatformInformation : Singleton<DisplayPlatformInformation>
{
	private Text platformInformationText;
	private Text platformTypeText;

	protected override void Awake()
	{
		base.Awake();
		platformInformationText = transform.Find("Background_InfoText").GetComponentInChildren<Text>();
		platformTypeText = transform.Find("Background_TypeText").GetComponentInChildren<Text>();
	}

	public void UpdateInformationText(string platformInformation, string platformType)
	{
		platformInformationText.text = platformInformation;
		platformTypeText.text = platformType;
	}
}