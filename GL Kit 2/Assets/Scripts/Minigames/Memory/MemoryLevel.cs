using UnityEngine;

[CreateAssetMenu(fileName = "New Memory Level", menuName = "Memory Level")]
public class MemoryLevel : ScriptableObject
{
    public string opdracht;
    public Sprite[] requiredObject;
    public Sprite[] randomSprites;
    public int width;
    public int height;
}
