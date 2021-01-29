using UnityEngine;

[CreateAssetMenu(fileName = "New Search Object", menuName = "Search Object")]
public class SearchItem : ScriptableObject
{
    public Sprite sprite;
    public GameObject prefab;
}
