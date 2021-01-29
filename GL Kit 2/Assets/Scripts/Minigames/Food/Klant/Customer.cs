using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "Customer")]
public class Customer : ScriptableObject
{
    [Header("Customer information")]
    public Sprite customerPortrait;
    public string customerSaying;
    public int minIngredients;
    public int maxIngredients;
    [Header("Customer Order")]
    public Ingredient bread;
    public Ingredient optionalBread;
    public Ingredient[] breadToppings;
    [Header("Optional butter")]
    public Ingredient[] optionalButter;
    [Header("Not allowed ingredients")]
    public Ingredient[] notAllowedIngredients;
    [Header("Ideale broodje")]
    public string[] ideaalBroodje;
}
