using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientPanel : MonoBehaviour
{
    public static FoodHandler foodHandler => FoodHandler.instance;

    [SerializeField] TMP_Text ingredientNameText;
    [SerializeField] Image ingredientSprite;

    private Ingredient ingredient;
    
    /// <summary>
    /// Handles setting the information of a panel that is added to the list
    /// </summary>
    /// <param name="ingredient"></param>
    public void SetInformation(Ingredient ingredient)
    {
        ingredientSprite.sprite = ingredient.ingredientSprite;
        ingredientNameText.text = ingredient.name;
        this.ingredient = ingredient;
    }

    /// <summary>
    /// Handles removing a ingredient from the list when the delete button is clicked
    /// </summary>
    public void RemoveIngredient()
    {
        foodHandler.IngredientAdded(ingredient, true);
        Destroy(gameObject);
    }
}
