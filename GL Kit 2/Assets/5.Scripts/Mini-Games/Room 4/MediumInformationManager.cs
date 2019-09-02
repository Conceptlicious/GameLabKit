using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;

[DisallowMultipleComponent]
public class MediumInformationManager : Manager<MediumInformationManager>
{
	[SerializeField] private Image MediumIcon;
	[SerializeField] private Text MediumDescription;
	[SerializeField] private Sprite NoImageFound;

	public void LoadInformation(MediumTemplate currentMedium)
	{
		MediumIcon.sprite = currentMedium.icon;
		MediumDescription.text = currentMedium.description;
	}
}