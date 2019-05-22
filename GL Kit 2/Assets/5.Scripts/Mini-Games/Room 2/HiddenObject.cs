using UnityEngine.UI;
using UnityEngine;
using GameLab;

public class HiddenObject : BetterMonoBehaviour
{
	[SerializeField] HiddenObjectTemplate hiddenObjectTemplate;
	private string objectName = string.Empty;
	private string objectDescription = string.Empty;
	private Image CurrentSprite; 

	private void Start()
	{
		SetValues();	
	}

	private void SetValues()
	{
		objectName = hiddenObjectTemplate.name;
		objectDescription = hiddenObjectTemplate.description;
		CurrentSprite.sprite = hiddenObjectTemplate.notFoundSprite;
	}
}
