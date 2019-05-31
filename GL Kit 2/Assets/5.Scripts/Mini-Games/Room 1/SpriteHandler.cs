using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;

[RequireComponent(typeof(Image))]
public class SpriteHandler : BetterMonoBehaviour
{
	[SerializeField] private Character characterOptions;
	private Persona currentPersona;
	[SerializeField] private AgeGroup age;
	private Image image;

	private void Awake()
	{
		image = GetComponent<Image>();
		UserInterfaceHandler.Instance.OnPersonaChanged += OnPersonaChange;

	}

	private void OnPersonaChange(Persona newPersona)
	{
		bool isMaleOrUnspecified = (newPersona.Gender == Genders.Male || newPersona.Gender == Genders.Unspecified);
		bool blind = (newPersona.Disability.HasFlag(Disabilities.LowVision));
		bool deaf = (newPersona.Disability.HasFlag(Disabilities.Deaf));
		bool disabled = (newPersona.Disability.HasFlag(Disabilities.PhysicalDisabileties));
		bool depressed = (newPersona.Disability.HasFlag(Disabilities.Anxiety));

		if (currentPersona.ComparePersonas(newPersona))
		{
			return;
		}

		if (!currentPersona.ComparePersonasAge(newPersona))
		{
			image.sprite = null;
			image.color = new Color32(0, 0, 0, 0);
			currentPersona = newPersona;
			return;
		}

		currentPersona = newPersona;
		image.color = new Color32(0, 0, 0, 255);

		if (blind && deaf && disabled && depressed)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MBlindDisabledDeafDepressed;
			}
			else
			{
				image.sprite = characterOptions.FBlindDisabledDeafDepressed;
			}
		}

		if (blind && deaf && disabled)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MBlindDisabledDeaf;
			}
			else
			{
				image.sprite = characterOptions.FBlindDisabledDeaf;
			}
		}

		if(blind && deaf && depressed)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MBlindDeafDepressed;
			}
			else
			{
				image.sprite = characterOptions.FBlindDeafDepressed;
			}
		}

		if(disabled && deaf && depressed)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MDisabledDeafDepressed;
			}
			else
			{
				image.sprite = characterOptions.FDisabledDeafDepressed;
			}
		}

		if(blind && deaf)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MBlindDeaf;
			}
			else
			{
				image.sprite = characterOptions.FBlindDeaf;
			}
		}

		if (blind && disabled)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MBlindDisabled;
			}
			else
			{
				image.sprite = characterOptions.FBlindDisabled;
			}
		}

		if (blind && depressed)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MBlindDepressed;
			}
			else
			{
				image.sprite = characterOptions.FBlindDepressed;
			}
		}
		if (disabled && deaf)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MDisabledDeaf;
			}
			else
			{
				image.sprite = characterOptions.FDisabledDeaf;
			}
		}
		if (depressed && deaf)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MDeafDepressed;
			}
			else
			{
				image.sprite = characterOptions.FDeafDepressed;
			}
		}
		if (disabled && depressed)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MDisabledDepressed;
			}
			else
			{
				image.sprite = characterOptions.FDisabledDepressed;
			}
		}

		if (blind)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MBlind;
			}
			else
			{
				image.sprite = characterOptions.FBlind;
			}
		}
		if (deaf)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MDeaf;
			}
			else
			{
				image.sprite = characterOptions.FDeaf;
			}
		}
		if (disabled)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MDisabled;
			}
			else
			{
				image.sprite = characterOptions.FDisabled;
			}
		}
		if (depressed)
		{
			if (isMaleOrUnspecified)
			{
				image.sprite = characterOptions.MDepressed;
			}
			else
			{
				image.sprite = characterOptions.FDepressed;
			}
		}
	}

}
