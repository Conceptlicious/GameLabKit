using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

[DisallowMultipleComponent]
public class ConvyerBeltManager : Manager<ConvyerBeltManager>
{
	[SerializeField] private List<MediumTemplate> mediums = new List<MediumTemplate>();
	private int currentMediumIndex = 0;

	private void Start()
	{
		MediumInformationManager.Instance.LoadInformation(mediums[currentMediumIndex]);
	}

	public void Next()
	{
		currentMediumIndex = (currentMediumIndex + 1) % mediums.Count;
		MediumInformationManager.Instance.LoadInformation(mediums[currentMediumIndex]);
	}

	public void Previous()
	{
		if( currentMediumIndex <= 0)
		{
			currentMediumIndex = mediums.Count - 1;
		}
		else
		{
			currentMediumIndex = (currentMediumIndex - 1) % mediums.Count;
		}

		MediumInformationManager.Instance.LoadInformation(mediums[currentMediumIndex]);
	}
}
