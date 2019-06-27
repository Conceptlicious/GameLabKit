using UnityEngine;
using GameLab;

public enum ToggleGroup
{
	Education,
	SpecialNeeds
};

//--------------------------------------------------
//Produced by Mathias Bevers.
//Overview: This script is used for the toggles of the education/disability group in Room 1, 
//it contains the information that is set in the inspector. The UI control will get this info
//to place it in the right list.
//Usage: On every toggle in the education/disability group in Room 1
//--------------------------------------------------
public class ToggleInformation : BetterMonoBehaviour
{
	public ToggleGroup toggleGroup;
	public Disabilities disabilities;
}