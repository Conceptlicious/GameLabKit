using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by Mathias
//Overview: This script is used for comparing Vectors because unity won't let you.
//Usage: It was used for testing.
//--------------------------------------------------

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