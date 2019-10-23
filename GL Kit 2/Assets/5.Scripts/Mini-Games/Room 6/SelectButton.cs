using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class SelectButton : BetterMonoBehaviour
{
	public static Sprite ArtSprite { get; private set; }
	private const string KNOT_NAME = "Part4";
	private const string ARTSTYLE_VARIABLE = "artStyle";

	[SerializeField] private string artStyleName;
	private Image buttonImage;
	private Button selectButton;

	private void Start()
	{
		selectButton = GetComponent<Button>();
		buttonImage = GetComponent<Image>();

		selectButton.onClick.AddListener(() => SelectSprite(artStyleName, buttonImage.sprite));
	}

	private void SelectSprite(string name, Sprite selectedSprite)
	{
		if (!ActivePatternManager.Instance.isWon)
		{
			return;
		}

		ArtSprite = selectedSprite;

		DialogueManager.Instance.CurrentDialogue.Reset(KNOT_NAME);
		DialogueManager.Instance.CurrentDialogue.SetStringVariable(ARTSTYLE_VARIABLE, $"\"{name}\"");
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}
}
