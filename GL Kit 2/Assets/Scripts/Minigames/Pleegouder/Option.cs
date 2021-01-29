using UnityEngine;

[CreateAssetMenu(fileName = "New Pleegouder option", menuName = "Pleegouder option")]
public class Option : ScriptableObject
{
    public string situatie;
    public Action[] options;
    public bool[] dialoog;
}

[System.Serializable]
public class Action
{
    public string text;
    public bool[] style;
}