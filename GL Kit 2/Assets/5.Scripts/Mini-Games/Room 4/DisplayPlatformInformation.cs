using GameLab;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlatformInformation : Singleton<DisplayPlatformInformation>
{
	public Sprite CurrentSelectedSprite { get; private set; }
	private Text platformInformationText;
	private Image platformImage;

	protected override void Awake()
	{
		base.Awake();
		platformInformationText = GetComponentInChildren<Text>();
		platformImage = GetComponentInChildren<Image>();
	}

	public void LoadInformation(int currentIndex)
	{
		TextAsset loadedText;

		switch (currentIndex)
		{
			case (int)PlatformType.VirtualReality:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_VR));
				UpdateInformation(loadedText, ConveyorBeltMovement.Instance.platformImages[(int)PlatformType.VirtualReality].sprite);
				break;

			case (int)PlatformType.AugmentedReality:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_AR));
				UpdateInformation(loadedText, ConveyorBeltMovement.Instance.platformImages[(int)PlatformType.AugmentedReality].sprite);
				break;

			case (int)PlatformType.Mobile:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_MOBILE));
				UpdateInformation(loadedText, ConveyorBeltMovement.Instance.platformImages[(int)PlatformType.Mobile].sprite);
				break;

			case (int)PlatformType.PC:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_PC));
				UpdateInformation(loadedText, ConveyorBeltMovement.Instance.platformImages[(int)PlatformType.PC].sprite);
				break;

			case (int)PlatformType.Exergames:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_EXERGAME));
				UpdateInformation(loadedText, ConveyorBeltMovement.Instance.platformImages[(int)PlatformType.Exergames].sprite);
				break;

			case (int)PlatformType.Tabletop:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_TABLETOP));
				UpdateInformation(loadedText, ConveyorBeltMovement.Instance.platformImages[(int)PlatformType.Tabletop].sprite);
				break;

			case (int)PlatformType.Console:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_CONSOLE));
				UpdateInformation(loadedText, ConveyorBeltMovement.Instance.platformImages[(int)PlatformType.Console].sprite);
				break;

			case (int)PlatformType.Wearables:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_WEARABLES));
				UpdateInformation(loadedText, ConveyorBeltMovement.Instance.platformImages[(int)PlatformType.Wearables].sprite);
				break;

			case (int)PlatformType.Web:
				loadedText = Resources.Load<TextAsset>(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_WEB));
				UpdateInformation(loadedText, ConveyorBeltMovement.Instance.platformImages[(int)PlatformType.Web].sprite);
				break;
		}
	}

	public void UpdateInformation(TextAsset loadedText, Sprite platformSprite)
	{
		platformInformationText.text = loadedText.ToString();
		platformImage.sprite = platformSprite;
		CurrentSelectedSprite = platformSprite;
	}
}