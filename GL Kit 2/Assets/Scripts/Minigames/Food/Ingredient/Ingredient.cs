using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public Sprite ingredientSprite;
    public string naam;
    public double vetten;
    public double koolhydraten;
    public int punten;
}
