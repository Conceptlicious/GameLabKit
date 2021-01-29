using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferenceHandler : MonoBehaviour
{

    public EmailFactory emailFactory => EmailFactory.instance;

    /// <summary>
    /// Array of all the question pages
    /// </summary>
    [Header("All pages of questions for the player")]
    [SerializeField] GameObject[] questionObjects;
    /// <summary>
    /// The current question page the player is on
    /// </summary>
    [Header("The current page the player is on")]
    [SerializeField] int questionIndex = 0;
    /// <summary>
    /// A serializable 2d array of the Toggle unity ui
    /// </summary>
    [Header("2D Array of toggles")]
    [SerializeField] ToggleOutput togglesOutput;
    /// <summary>
    /// The list of outputs of all toggles the player had selected on the pages
    /// that will be send to the send email method within EmailFactory
    /// </summary>
    [Header("List of all outcomes of the players choices")]
    [SerializeField] List<string> outcomeList = new List<string>();

    public void OpenPreference()
    {
        questionObjects[questionIndex].SetActive(true);
    }

    public void NextQuestion()
    {
        string outcome = "";
        Toggle[] toggles = togglesOutput.toggles[questionIndex].toggleArray;

        //Handles adding all the toggled buttons to the outcome string
        foreach (Toggle toggle in toggles)
        {
            Debug.Log($"{toggle.name} is {(toggle.isOn ? "on" : "off")}");
            if (toggle.isOn)
            {
                outcome += $"{toggle.name}, ";
                Debug.Log("outcome: " + outcome);
            }
        }

        //Will remove the last 2 characters(, ) if there are more then 1 character
        if (outcome.Length >= 2)
            outcome = outcome.Substring(0, outcome.Length - 2);
        Debug.Log("last outcome: " + outcome);

        //Adds the outcome to the list
        outcomeList.Add(outcome);

        //Handles deactivating the current panel and increasing the question index
        questionObjects[questionIndex++].SetActive(false);

        //Handles opening the next panel
        questionObjects[questionIndex].SetActive(true);
    }

    public void Finished(bool sendEmail)
    {
        if (sendEmail)
            emailFactory.SendEmail(outcomeList);
        questionObjects[questionIndex].SetActive(false);
    }
}
