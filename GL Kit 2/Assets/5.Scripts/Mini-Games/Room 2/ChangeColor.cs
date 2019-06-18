using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class ChangeColor
{
	public static Color32 NewColor(byte red, byte green, byte blue, byte alpha)
	{
		Color32 newColor = Color.white;
		newColor.r = red;
		newColor.g = green;
		newColor.b = blue;
		newColor.a = alpha;

		return newColor;
	}

	public static Color ColorByHex(string htmlString)
	{
		Color newColor = Color.white;

		if(ColorUtility.TryParseHtmlString(htmlString, out Color parsedColor))
		{
			newColor = parsedColor;
		}

		return newColor;
	}
}
