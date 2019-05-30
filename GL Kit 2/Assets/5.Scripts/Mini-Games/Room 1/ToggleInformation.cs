using UnityEngine;
using GameLab;

public enum ToggleGroup
{
	Education,
	SpecialNeeds
};

public class ToggleInformation : BetterMonoBehaviour
{
	public ToggleGroup toggleGroup;
	public Disabilities disabilities;
}