using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLab
{
	public static class Color32Extensions
	{
		public static bool CompareRGB(this Color32 color, Color32 otherColor)
		{
			return color.r == otherColor.r && color.g == otherColor.g && color.b == otherColor.b;
		}

		public static bool CompareRGBA(this Color32 color, Color32 otherColor)
		{
			return color.CompareRGB(otherColor) && color.a == otherColor.a;
		}
	}
}
