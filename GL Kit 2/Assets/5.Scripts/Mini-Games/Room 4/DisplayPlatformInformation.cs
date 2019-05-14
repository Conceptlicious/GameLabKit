using GameLab;
using System.IO;
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

	public void LoadInformation(int currentIndex)
	{
		string platformInformation;

		switch (currentIndex)
		{

			case (int)PlatformType.VirtualReality:
				// Load in VR info from text file.
				using (FileStream fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_VR) + LoadingPaths.FILE_TYPE))
				{
					using (StreamReader textReader = new StreamReader(fileStream))
					{
						platformInformation = textReader.ReadToEnd();
					}
				}
				UpdateInformationText(platformInformation, "Virtual Reality");
				break;

			case (int)PlatformType.AugmentedReality:
				// Load in VR info from text file.
				using (FileStream fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_AR) + LoadingPaths.FILE_TYPE))
				{
					using (StreamReader textReader = new StreamReader(fileStream))
					{
						platformInformation = textReader.ReadToEnd();
					}
				}
				UpdateInformationText(platformInformation, "Augmented Reality");
				break;

			case (int)PlatformType.Mobile:
				// Load in VR info from text file.
				using (FileStream fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_MOBILE) + LoadingPaths.FILE_TYPE))
				{
					using (StreamReader textReader = new StreamReader(fileStream))
					{
						platformInformation = textReader.ReadToEnd();
					}
				}
				UpdateInformationText(platformInformation, "Mobile");
				break;

			case (int)PlatformType.PC:
				// Load in VR info from text file.
				using (FileStream fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_PC) + LoadingPaths.FILE_TYPE))
				{
					using (StreamReader textReader = new StreamReader(fileStream))
					{
						platformInformation = textReader.ReadToEnd();
					}
				}
				UpdateInformationText(platformInformation, "PC");
				break;

			case (int)PlatformType.Exergames:
				// Load in VR info from text file.
				using (FileStream fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_EXERGAME) + LoadingPaths.FILE_TYPE))
				{
					using (StreamReader textReader = new StreamReader(fileStream))
					{
						platformInformation = textReader.ReadToEnd();
					}
				}
				UpdateInformationText(platformInformation, "Exergame");
				break;

			case (int)PlatformType.Tabletop:
				// Load in VR info from text file.
				using (FileStream fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_TABLETOP) + LoadingPaths.FILE_TYPE))
				{
					using (StreamReader textReader = new StreamReader(fileStream))
					{
						platformInformation = textReader.ReadToEnd();
					}
				}
				UpdateInformationText(platformInformation, "Tabletop");
				break;

			case (int)PlatformType.Console:
				// Load in VR info from text file.
				using (FileStream fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_CONSOLE) + LoadingPaths.FILE_TYPE))
				{
					using (StreamReader textReader = new StreamReader(fileStream))
					{
						platformInformation = textReader.ReadToEnd();
					}
				}
				UpdateInformationText(platformInformation, "Console");
				break;

			case (int)PlatformType.Wearables:
				// Load in VR info from text file.
				using (FileStream fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_WEARABLES) + LoadingPaths.FILE_TYPE))
				{
					using (StreamReader textReader = new StreamReader(fileStream))
					{
						platformInformation = textReader.ReadToEnd();
					}
				}
				UpdateInformationText(platformInformation, "Wearables");
				break;

			case (int)PlatformType.Web:
				// Load in VR info from text file.
				using (FileStream fileStream = File.OpenRead(Path.Combine(LoadingPaths.PATH_DEFAULT, LoadingPaths.FILE_NAME_WEB) + LoadingPaths.FILE_TYPE))
				{
					using (StreamReader textReader = new StreamReader(fileStream))
					{
						platformInformation = textReader.ReadToEnd();
					}
				}
				UpdateInformationText(platformInformation, "Web");
				break;
		}
	}

	public void UpdateInformationText(string platformInformation, string platformType)
	{
		platformInformationText.text = platformInformation;
		platformTypeText.text = platformType;
	}
}