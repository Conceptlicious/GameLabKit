using UnityEngine.UI;

[System.Serializable]
public class ToggleOutput
{
    [System.Serializable]
    public struct ToggleData
    {
        public Toggle[] toggleArray;
    }

    public ToggleData[] toggles;

    public ToggleOutput(int size)
    {
        toggles = new ToggleData[size];
    }
}
