using GameLab;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlatformInformation : Singleton<DisplayPlatformInformation>
{
	public Sprite currentSelectedSprite { get; private set; }
	[SerializeField] private Text platformInformationText;
	[SerializeField] private Image platformImage;
	[SerializeField] private Sprite[] platformSprites;

	protected override void Awake()
	{
		base.Awake();
	}

	public void LoadInformation(int currentIndex)
	{
		TextAsset loadedText;

		switch (currentIndex)
		{
			case (int)PlatformType.VirtualReality:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_VR));
				UpdateInformation(loadedText, platformSprites[(int)PlatformType.VirtualReality]);
				break;

			case (int)PlatformType.AugmentedReality:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_AR));
				UpdateInformation(loadedText, platformSprites[(int)PlatformType.AugmentedReality]);
				break;

			case (int)PlatformType.Mobile:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_MOBILE));
				UpdateInformation(loadedText, platformSprites[(int)PlatformType.Mobile]);
				break;

			case (int)PlatformType.PC:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_PC));
				UpdateInformation(loadedText, platformSprites[(int)PlatformType.PC]);
				break;

			case (int)PlatformType.Exergames:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_EXERGAME));
				UpdateInformation(loadedText, platformSprites[(int)PlatformType.Exergames]);
				break;

			case (int)PlatformType.Tabletop:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_TABLETOP));
				UpdateInformation(loadedText, platformSprites[(int)PlatformType.Tabletop]);
				break;

			case (int)PlatformType.Console:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_CONSOLE));
				UpdateInformation(loadedText, platformSprites[(int)PlatformType.Console]);
				break;

			case (int)PlatformType.Wearables:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_WEARABLES));
				UpdateInformation(loadedText, platformSprites[(int)PlatformType.Wearables]);
				break;

			case (int)PlatformType.Web:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_WEB));
				UpdateInformation(loadedText, platformSprites[(int)PlatformType.Web]);
				break;
		}
	}

	public void UpdateInformation(TextAsset loadedText, Sprite platformSprite)
	{
		platformInformationText.text = loadedText.ToString();
		platformImage.sprite = platformSprite;
		currentSelectedSprite = platformSprite;
	}
}