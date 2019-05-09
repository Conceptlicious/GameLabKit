using UnityEngine;

public class ToggleInformation : MonoBehaviour
{
	public string ToggleGroup { get; private set; } = string.Empty;
	[SerializeField] bool isEducationToggle = false;

	private void Start()
	{
		if(isEducationToggle)
		{
			ToggleGroup = "Education";
		}
		else
		{
			ToggleGroup = "Special Needs";
		}
	}
}
