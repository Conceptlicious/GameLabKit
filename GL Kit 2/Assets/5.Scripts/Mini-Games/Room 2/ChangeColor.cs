using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class ChangeColor
{
	public static Color NewColor(float red, float green, float blue, float alpha)
	{
		Color newColor = Color.white;
		newColor.r = red;
		newColor.g = green;
		newColor.b = blue;
		newColor.a = alpha;

		return newColor;
	}
}
