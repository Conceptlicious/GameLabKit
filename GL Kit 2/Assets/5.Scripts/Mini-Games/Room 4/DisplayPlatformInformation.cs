using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlatformInformation : MonoBehaviour
{
	#region Varibles
	[SerializeField] Text platformInformationText;
	[SerializeField] Text platformTypeText;
	#endregion

	public void UpdateInformationText(string pPlatformInformation, string pPlatformType)
	{
		platformInformationText.text = pPlatformInformation;
		platformTypeText.text = pPlatformType;
	}
}