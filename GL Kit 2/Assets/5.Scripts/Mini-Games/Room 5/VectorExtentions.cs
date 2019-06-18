using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class VectorExtentions
{
	public static bool CompareVector3(Vector3 vectorA, Vector3 vectorB)
	{
		if (vectorA.x != vectorB.x || vectorA.y != vectorB.y || vectorA.z != vectorB.z)
		{
			return false;
		}

		return true;
	}

	public static bool CompareVector2(Vector2 vectorA, Vector2 vectorB)
	{
		if (vectorA.x != vectorB.x || vectorA.y != vectorB.y)
		{
			return false;
		}

		return true;
	}
}