using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script is used to represent animateable UI objects.
//Usage: Used for UI animation.
//--------------------------------------------------

public class UISlidingObject : MonoBehaviour
{    
	[SerializeField] private GameObject mainObject;
	
	//Also understanable as: Target- and Origin positions
	[SerializeField] private Transform hiddenPosition;
	[SerializeField] private Transform shownPosition;

	//Percentages control at which points the UI will stop animating. 
	//E.G.: at 0.25 and 0.75 the object will only animating in the beginning
	//and ending quarters. Used for stalling the animation in some places.

	[SerializeField] private Vector2 inOutPercentages;

	public GameObject MainObject => mainObject;
	public Transform HiddenPosition => hiddenPosition;
	public Transform ShownPosition => shownPosition;
	public Vector2 InOutPercentages => inOutPercentages;

	void OnValidate()
	{
		float x = inOutPercentages.x;
		float y = inOutPercentages.y;

		x = Mathf.Clamp01(x);
		y = Mathf.Clamp01(y);

		//BUG: When typing "0.x" the leading zero forces y to become x and therefore wipe x's data.
		float tempX = x;
		float tempY = y;

		y = Mathf.Max(tempX, tempY);
		x = Mathf.Min(tempX, tempY);

		inOutPercentages = new Vector2(x, y);		
	}
}
