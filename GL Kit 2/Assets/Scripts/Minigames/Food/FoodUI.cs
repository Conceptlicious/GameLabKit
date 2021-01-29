using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodUI : MonoBehaviour
{
    private FoodHandler foodHandler => FoodHandler.instance;

    [Header("Ingredient List Data")]
    [SerializeField] GameObject ingredientPrefab;
    [SerializeField] Transform ingredientParent;
    [Header("Health meter")]
    [SerializeField] Image healthMeter;
    [SerializeField] TMP_Text healthProcentage;
    [SerializeField] TMP_Text currentPointsText;

    private void Start()
    {
        foodHandler.onIngredientListModified += UpdateUI;
    }

    /// <summary>
    /// Handles updating the ui when a ingredient is removed or added
    /// </summary>
    /// <param name="ingredient"></param>
    /// <param name="punten"></param>
    /// <param name="remove"></param>
    private void UpdateUI(Ingredient ingredient, int punten, bool remove = false)
    {
        //Calculates the procentage
        double oneProcent = 65 / 100D;
        int currentProcentage = (int) (punten / oneProcent);

        //Add the panel if its a new product and ingredient isnt null
        if (!remove && ingredient != null)
        {
            GameObject panel = Instantiate(ingredientPrefab, Vector3.zero, Quaternion.identity);
            IngredientPanel ingredientPanel = panel.GetComponent<IngredientPanel>();
            panel.name = $"{ingredient.name}";
            panel.transform.SetParent(ingredientParent);

            ingredientPanel.SetInformation(ingredient);
        }

        //Handles cleaning all the panels within the ingredient parent object
        if(ingredient == null)
            foreach(Transform child in ingredientParent.transform)
                Destroy(child.gameObject);

        //Update the correct panels
        healthMeter.fillAmount = (float) ((float) currentProcentage / 100);
        healthProcentage.text = $"{(100 - currentProcentage)}%\nGezond";
        currentPointsText.text = $"Punten: {punten}/65";
    }
}
