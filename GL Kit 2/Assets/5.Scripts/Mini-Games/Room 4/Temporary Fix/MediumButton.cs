using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

[RequireComponent(typeof(MonoBehaviour))]
public class MediumButton : BetterMonoBehaviour
{
	[SerializeField] private MediumTemplate mediumInformation;

	private string mediumDescription = string.Empty;
	private Sprite mediumSprite = null;
	private Button mediumButton = null;

	private void Start()
	{
		mediumButton = GetComponent<Button>();
		mediumDescription = mediumInformation.description;
		mediumSprite = mediumInformation.icon;

		mediumButton.onClick.AddListener(() => DisplayMediumInformationManager.Instance.SetDisplayText(mediumDescription));
		mediumButton.onClick.AddListener(() => SelectSprite(mediumSprite));
	}

	private void SelectSprite(Sprite newSprite)
	{
		SendSpriteRoom4.Instance.LastSelectedSprite = newSprite;
	}
}
