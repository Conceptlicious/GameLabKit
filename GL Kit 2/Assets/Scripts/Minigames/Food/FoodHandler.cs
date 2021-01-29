using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodHandler : Minigame
{
    public static FoodHandler instance;
    [Header("Canvas with data")]
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject bonnetjeObject;
    [Header("All customers")]
    [SerializeField] List<Customer> customers = new List<Customer>();
    [Header("Customer Infromation")]
    [SerializeField] TMP_Text orderText;
    [SerializeField] Customer currentCustomer;
    [SerializeField] int maxPoints = 65;
    [Header("Game Information")]
    [SerializeField] int currentCustomerIndex;
    [SerializeField] List<Ingredient> ingredientsUsed = new List<Ingredient>();
    [Header("Art styles")]
    [SerializeField] GameObject realisticStyle;
    [SerializeField] GameObject cartoonishStyle;

    [Header("Broodje compare")]
    [SerializeField] GameObject compareObject;
    [SerializeField] TMP_Text compareText;
    [SerializeField] bool insideCompare;
    [SerializeField] string placeholderCompare;

    
    /// <summary>
    /// Event used to update the ui whenever a new ingredient is added to the lsit
    /// </summary>
    public delegate void OnIngredientListModified(Ingredient ingredient, int punten, bool remove = false);
    public OnIngredientListModified onIngredientListModified;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        base.Start();
        placeholderCompare = compareText.text;
    }

    private void Update()
    {
        base.Update();
        if (currentState.Equals(State.SWITCH_CUSTOMERS))
        {
            //Handles clicking the game objects
            if (Input.GetMouseButtonDown(0))
            {
                GameObject clickedObject = GetClickedGameObject();
                if (clickedObject != null && clickedObject.name.Equals("Red button"))
                    ClickRedButton();
            }
        }
    }

    public override void StartMinigame()
    {
        currentCustomerIndex = 0;
        currentCustomer = customers[currentCustomerIndex];
    }

    /// <summary>
    /// Handles adding a ingredient to the list
    /// </summary>
    /// <param name="ingredient"></param>
    public void IngredientAdded(Ingredient ingredient)
    {
        if (!hasStarted || minigameHandler.gamePaused || insideCompare) return;
        IngredientAdded(ingredient, false);
    }

    /// <summary>
    /// Handles adding or removing a ingredient from the list
    /// </summary>
    /// <param name="ingredient"></param>
    /// <param name="remove"></param>
    public void IngredientAdded(Ingredient ingredient, bool remove = false)
    {
        if(ingredient != null)
        {
            if (!remove)
            {
                if (ingredientsUsed.Contains(ingredient) || (ingredient.naam.Contains("brood") && HasBread())) return;

                if (ingredientsUsed.Count >= 6)
                {
                    Debug.Log("wowoowowow");
                    return;
                }

                if ((ingredient.punten + GetPunten()) > maxPoints)
                {
                    Debug.Log("Will exceed the point total");
                    return;
                }
            }

            if (remove) ingredientsUsed.Remove(ingredient);
            else ingredientsUsed.Add(ingredient);
        }
        onIngredientListModified?.Invoke(ingredient, GetPunten(), remove);
    }

    /// <summary>
    /// Checks if there is any bread within the list
    /// </summary>
    /// <returns></returns>
    bool HasBread()
    {
        foreach(Ingredient ingredient in ingredientsUsed)
        {
            if (!ingredient.naam.Contains("brood")) continue;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if there is any butter on the bread
    /// </summary>
    /// <returns></returns>
    bool HasButter()
    {
        foreach (Ingredient ingredient in ingredientsUsed)
        {
            if(ingredient.naam.Equals("Boter") || ingredient.naam.Equals("Margarine"))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Calculates how many points the sandwich is worth at the moment
    /// </summary>
    /// <returns></returns>
    int GetPunten()
    {
        int punten = 0;
        foreach (Ingredient ingredient in ingredientsUsed)
            punten += ingredient.punten;
        return punten;
    }

    /// <summary>
    /// Checks if the user meets the requirments of the sandwich for the customer
    /// </summary>
    /// <returns></returns>
    public bool MeetsRequirements()
    {
        if (ingredientsUsed.Count < currentCustomer.minIngredients)
        {
            Debug.Log("Does not meet the required ingredient amount.");
            return false;
        }

        if(ingredientsUsed.Count > currentCustomer.maxIngredients)
        {
            Debug.Log("Too many ingredients have been used for the sandwich");
            return false;
        }

        if(!HasBread())
        {
            Debug.Log("No bread was found within the ingredients, so sandwich not complete");
            return false;
        }

        if (!ingredientsUsed.Contains(currentCustomer.bread) && currentCustomer.optionalBread == null)
        {
            Debug.Log("Does not have the required bread...");
            return false;
        }

        if (currentCustomer.optionalButter.Length != 0 && !HasButter())
        {
            Debug.Log("Does not have butter on the bread, when the customer requires it");
            return false;
        }

        if(currentCustomer.notAllowedIngredients.Length != 0)
        {
            foreach(Ingredient ingredient in currentCustomer.notAllowedIngredients)
            {
                if (ingredientsUsed.Contains(ingredient))
                {
                    Debug.Log("Has one or more of the items that are not allowed within this sandwich...");
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Handles finishing the customer
    /// </summary>
    public void FinishCustomer()
    {
        if (!hasStarted || minigameHandler.gamePaused || insideCompare) return;

        bool meetsRequirements = MeetsRequirements();
        Debug.LogError("meetsRequirements: " + meetsRequirements);
        if (meetsRequirements)
        {
            if (currentCustomerIndex == (customers.Count - 1))
            {
                Debug.LogError("Finished with all customers");
                Reset();
                gameCanvas.SetActive(false);
                DisplayTrophy();
                return;
            }

            string text = compareText.text = placeholderCompare;
            string[] right = new string[] { "A2", "B2", "C2", "D2", "E2", "F2" };

            for (int index = 0; index < right.Length; index++)
                text = text.Replace(right[index], index >= currentCustomer.ideaalBroodje.Length ? "" : currentCustomer.ideaalBroodje[index]);

            currentCustomer = customers[++currentCustomerIndex];
            /*if (currentCustomerIndex == 3)
            {
                gameCanvas.SetActive(false);
                dialogueHandler.SetDialogue("Food truck", "Switch");
                return;
            }*/
            if (currentCustomerIndex == 3)
            {
                bonnetjeObject.SetActive(false);
                gameCanvas.SetActive(false);
            }


            string[] left = new string[] { "A1", "B1", "C1", "D1", "E1", "F1" };

            for (int index = 0; index < left.Length; index++)
                text = text.Replace(left[index], index >= ingredientsUsed.Count ? "" : ingredientsUsed[index].naam);

            compareText.text = $"{text}";

            Reset();
            insideCompare = true;
            //TODO: Setup the text
            //compareText

            compareObject.SetActive(true);
        }
    }

    public void CloseCompare()
    {
        insideCompare = false;
        compareObject.SetActive(false);
        orderText.text = GetOrder();
        if (currentCustomerIndex == 3)
            dialogueHandler.SetDialogue("Food truck", "Switch");
    }

    /// <summary>
    /// Handles resetting the ingredients and UI
    /// </summary>
    private void Reset()
    {
        ingredientsUsed.Clear();
        IngredientAdded(null);
    }

    void ClickRedButton()
    {
        cartoonishStyle.SetActive(true);
        realisticStyle.SetActive(false);
        dialogueHandler.SetDialogue("Food truck", "Switched");
    }

    private string GetOrder()
    {
        int orderIndex = 1;
        string order = $"{(currentCustomerIndex == 3 ? "Veganistisch\n" : "")}{orderIndex++}. <indent=10%>"; //1. <indent=10%>Wit of bruin brood</indent>
        if (currentCustomer.bread != null && currentCustomer.optionalBread != null)
            order += "Wit/bruinbrood</indent>\n";
        else order += currentCustomer.bread.naam + "</indent>\n";


        if(currentCustomer.optionalButter.Length > 0)
        {
            order += $"{orderIndex++}. <indent=10%>";

            if (currentCustomer.optionalButter.Length >= 2)
                order += $"Boter of margarine</indent>\n";
            else if (currentCustomer.optionalButter.Length > 0 && currentCustomer.optionalButter.Length < 2) order += currentCustomer.optionalButter[0].naam + "</indent>\n";
        }

        foreach (Ingredient ingredient in currentCustomer.breadToppings)
            order += $"{orderIndex++}. <indent=10%>{ingredient.naam}</indent>\n";

        for(int index = orderIndex; index <= currentCustomer.minIngredients; index++)
            order += $"{orderIndex++}. <indent=10%>???</indent>\n";

        return order;
    }

    /// <summary>
    /// Handles switching the art style of the food minigame
    /// </summary>
    void SwitchCustomers()
    {
        bonnetjeObject.SetActive(true);
        currentState = State.CHILDREN_CUSTOMERS;
        Reset();
        orderText.text = GetOrder();
        gameCanvas.SetActive(true);
    }

    public override void FinishMinigame()
    {
        player.AddTrophy(trophy);
        dialogueHandler.SetDialogue("Food truck", "Finished");
    }

    public override void OnDialogueFinished()
    {
        if (!hasStarted)
        {
            switch (labHandler.CurrentState)
            {
                case LabHandler.State.FOOD_MINIGAME:
                    instructionScreen[0].SetActive(true);
                    break;
            }
        }
        switch(currentState)
        {
            case State.REGULAR_CUSTOMERS:
                if (currentCustomerIndex == 3)
                    currentState = State.SWITCH_CUSTOMERS;
                break;
            case State.SWITCH_CUSTOMERS:
                SwitchCustomers();
                break;
            case State.CHILDREN_CUSTOMERS:
                minigameHandler.ReturnToLab(true);
                currentState = State.ENDED;
                break;
        }
    }

    public void ClickedStart()
    {
        base.ClickedStart();
        bonnetjeObject.SetActive(true);
        gameCanvas.SetActive(true);
        orderText.text = GetOrder();
        //customerObject.SetActive(true);
    }

    public override void ResetMinigame()
    {
        Reset();
        StartMinigame();
        currentState = State.REGULAR_CUSTOMERS;
        hasStarted = false;
        bonnetjeObject.SetActive(false);
        gameCanvas.SetActive(false);
    }

    private State currentState = State.REGULAR_CUSTOMERS;

    public enum State
    {
        REGULAR_CUSTOMERS,
        SWITCH_CUSTOMERS,
        CHILDREN_CUSTOMERS,
        ENDED
    }
}
