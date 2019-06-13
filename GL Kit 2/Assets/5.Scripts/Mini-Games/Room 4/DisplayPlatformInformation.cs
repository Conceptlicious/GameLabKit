using GameLab;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlatformInformation : Singleton<DisplayPlatformInformation>
{
	[SerializeField] private Text platformInformationText;
	[SerializeField] private Image platformImage;
	[SerializeField] private Sprite[] platformSprites;

	protected override void Awake()
	{
		base.Awake();
	}

	public void LoadInformation(int currentIndex)
	{
		TextAsset loadedText = null;

		switch (currentIndex)
		{
			case (int)PlatformType.VirtualReality:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_VR));
				//UpdateInformation(loadedText, LoadingPaths.FILE_NAME_VR);
				break;

			case (int)PlatformType.AugmentedReality:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_AR));
				//UpdateInformation(loadedText, LoadingPaths.FILE_NAME_AR);
				break;

			case (int)PlatformType.Mobile:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_MOBILE));
				//UpdateInformation(loadedText, LoadingPaths.FILE_NAME_MOBILE);
				break;

			case (int)PlatformType.PC:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_PC));
				//UpdateInformation(loadedText, LoadingPaths.FILE_NAME_PC);
				break;

			case (int)PlatformType.Exergames:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_EXERGAME));
				//UpdateInformation(loadedText, LoadingPaths.FILE_NAME_EXERGAME);
				break;

			case (int)PlatformType.Tabletop:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_TABLETOP));
				//UpdateInformation(loadedText, LoadingPaths.FILE_NAME_TABLETOP);
				break;

			case (int)PlatformType.Console:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_CONSOLE));
				//UpdateInformation(loadedText, LoadingPaths.FILE_NAME_CONSOLE);
				break;

			case (int)PlatformType.Wearables:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_WEARABLES));
				//UpdateInformation(loadedText, LoadingPaths.FILE_NAME_WEARABLES);
				break;

			case (int)PlatformType.Web:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_WEB));
				//UpdateInformation(loadedText, LoadingPaths.FILE_NAME_WEB);
				break;
		}
	}

	public void UpdateInformation(TextAsset loadedText, Sprite platformSprite)
	{
		platformInformationText.text = loadedText.ToString();
		platformImage.sprite = platformSprite;
	}
}