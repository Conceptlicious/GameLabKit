using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;
using Room3;

public class Lever : BetterMonoBehaviour
{
	
	public void OnButtonPressed()
	{
		TileGrid.Instance.SetInteractedWith(true);
	}

}
