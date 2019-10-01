using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;

[RequireComponent(typeof(Image))]
public class SpriteHandler : BetterMonoBehaviour
{
	[SerializeField] private Character characterOptions;
	[SerializeField] private UIControl uIControl;
	private Persona currentPersona;
	[SerializeField] private AgeGroup age;
	private Image personaImage;
	public static Sprite personaSprite;

	private void Awake()
	{
		personaImage = GetComponent<Image>();
		uIControl.OnPersonaChanged += OnPersonaChange;
	}

	private void OnPersonaChange(Persona newPersona)
	{
		if (ReturnPersonaSprite(newPersona) != null)
		{
			personaSprite = ReturnPersonaSprite(newPersona);
		}
	}

	private Sprite ReturnPersonaSprite(Persona newPersona)
	{
		bool isMaleOrUnspecified = (newPersona.Gender == Genders.Male || newPersona.Gender == Genders.Unspecified);
		bool blind = (newPersona.Disability.HasFlag(Disabilities.LowVision));
		bool deaf = (newPersona.Disability.HasFlag(Disabilities.Deaf));
		bool disabled = (newPersona.Disability.HasFlag(Disabilities.PhysicalDisabileties));
		bool depressed = (newPersona.Disability.HasFlag(Disabilities.Anxiety));

		//Debug.Log("event called");
		if (age != newPersona.Age)
		{
			personaImage.sprite = null;
			personaImage.color = new Color32(255, 255, 255, 0);
			currentPersona = newPersona;
			return personaImage.sprite;
		}

		currentPersona = newPersona;
		personaImage.color = new Color32(255, 255, 255, 255);

		if (blind && deaf && disabled && depressed)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MBlindDisabledDeafDepressed;
			}
			else
			{
				personaImage.sprite = characterOptions.FBlindDisabledDeafDepressed;
			}
			return personaImage.sprite;
		}

		if (blind && deaf && disabled)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MBlindDisabledDeaf;
			}
			else
			{
				personaImage.sprite = characterOptions.FBlindDisabledDeaf;
			}
			return personaImage.sprite;
		}

		if (blind && deaf && depressed)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MBlindDeafDepressed;
			}
			else
			{
				personaImage.sprite = characterOptions.FBlindDeafDepressed;
			}
			return personaImage.sprite;
		}

		if (disabled && deaf && depressed)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MDisabledDeafDepressed;
			}
			else
			{
				personaImage.sprite = characterOptions.FDisabledDeafDepressed;
			}
			return personaImage.sprite;
		}

		if (blind && deaf)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MBlindDeaf;
			}
			else
			{
				personaImage.sprite = characterOptions.FBlindDeaf;
			}
			return personaImage.sprite;
		}

		if (blind && depressed && disabled)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MDisabledBlindDepressed;
			}
			else
			{
				personaImage.sprite = characterOptions.FDisabledBlindDepressed;
			}
			return personaImage.sprite;
		}

		if (blind && disabled)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MBlindDisabled;
			}
			else
			{
				personaImage.sprite = characterOptions.FBlindDisabled;
			}
			return personaImage.sprite;
		}

		if (blind && depressed)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MBlindDepressed;
			}
			else
			{
				personaImage.sprite = characterOptions.FBlindDepressed;
			}
			return personaImage.sprite;
		}

		if (disabled && deaf)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MDisabledDeaf;
			}
			else
			{
				personaImage.sprite = characterOptions.FDisabledDeaf;
			}
			return personaImage.sprite;
		}

		if (depressed && deaf)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MDeafDepressed;
			}
			else
			{
				personaImage.sprite = characterOptions.FDeafDepressed;
			}
			return personaImage.sprite;
		}

		if (disabled && depressed)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MDisabledDepressed;
			}
			else
			{
				personaImage.sprite = characterOptions.FDisabledDepressed;
			}
			return personaImage.sprite;
		}

		if (blind)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MBlind;
			}
			else
			{
				personaImage.sprite = characterOptions.FBlind;
			}
			return personaImage.sprite;
		}

		if (deaf)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MDeaf;
			}
			else
			{
				personaImage.sprite = characterOptions.FDeaf;
			}
			return personaImage.sprite;
		}

		if (disabled)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MDisabled;
			}
			else
			{
				personaImage.sprite = characterOptions.FDisabled;
			}
			return personaImage.sprite;
		}

		if (depressed)
		{
			if (isMaleOrUnspecified)
			{
				personaImage.sprite = characterOptions.MDepressed;
			}
			else
			{
				personaImage.sprite = characterOptions.FDepressed;
			}
			return personaImage.sprite;
		}

		if (isMaleOrUnspecified)
		{
			personaImage.sprite = characterOptions.Male;
		}
		else
		{
			personaImage.sprite = characterOptions.Female;
		}

		return personaImage.sprite;
	}
}
