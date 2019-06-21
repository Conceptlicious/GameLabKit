using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;

public class ProceduralButtonCreation : GameLabEvent
{
	public string ButtonName;
	public Button Button;

	public ProceduralButtonCreation(string name, Button button)
	{
		ButtonName = name;
		Button = button;
	}

}
