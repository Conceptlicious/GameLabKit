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
}
