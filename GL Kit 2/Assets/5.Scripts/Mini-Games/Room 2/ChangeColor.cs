using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class ChangeColor
{
	public static Color NewColor(float red, float green, float blue, float alpha)
	{
		Color.RGBToHSV(new Color(red, green, blue, alpha),
			out float H, out float S, out float V);

		Color newColor = Color.white;


		return newColor;
	}
}
