using System.IO;
using UnityEngine;

public enum PlatformType
{
	Mobile,
	Wearables,
	Tabletop,
	VirtualReality,
	PC,
	Console,
	Web,
	Exergames,
	AugmentedReality
};

public class PlatformInformation : MonoBehaviour
{
	#region Varibles
	[SerializeField] DisplayPlatformInformation displayPlatformInformation;

	string platformInformation;
	#endregion

	public void LoadInformation(int pCurrentIndex)
	{
		switch (pCurrentIndex)
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
				displayPlatformInformation.UpdateInformationText(platformInformation, "Virtual Reality");
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
				displayPlatformInformation.UpdateInformationText(platformInformation, "Augmented Reality");
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
				displayPlatformInformation.UpdateInformationText(platformInformation, "Mobile");
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
				displayPlatformInformation.UpdateInformationText(platformInformation, "PC");
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
				displayPlatformInformation.UpdateInformationText(platformInformation, "Exergame");
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
				displayPlatformInformation.UpdateInformationText(platformInformation, "Tabletop");
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
				displayPlatformInformation.UpdateInformationText(platformInformation, "Console");
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
				displayPlatformInformation.UpdateInformationText(platformInformation, "Wearables");
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
				displayPlatformInformation.UpdateInformationText(platformInformation, "Web");
				break;
		}
	}
}