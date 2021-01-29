using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionHandler : MonoBehaviour
{
    private Player player => Player.instance;
    private PleegouderHandler pleegouderHandler => PleegouderHandler.instance;

    public static OptionHandler instance;

    [SerializeField] GameObject[] options;

    [SerializeField] TMP_Text situatieText;

    [Header("Borden objects")]
    [SerializeField] GameObject packedBord;
    [SerializeField] GameObject unpackedBord;

    public void Start()
    {
        instance = this;
    }

    public void SetOptions(Option option)
    {
        for(int index = 0; index < option.options.Length; index++)
        {
            options[index].SetActive(true);
            options[index].GetComponentInChildren<TMP_Text>().text = $"{option.options[index].text.Replace("[NAAM SPELER]", player.PlayerName)}";
        }
        situatieText.text = $"{option.situatie.Replace("[NAAM SPELER]", player.PlayerName).Replace("\\n", "\n")}";
    }

    public void PickOption(string option)
    {
        for (int index = 0; index < options.Length; index++)
            options[index].SetActive(false);
        situatieText.text = "";

        int answerIndex = 0;
        if (option.Equals("B")) answerIndex = 1;
        else if (option.Equals("C")) answerIndex = 2;

        for (int index = 0; index < pleegouderHandler.GetCurrentOption().options[answerIndex].style.Length; index++)
            if (pleegouderHandler.GetCurrentOption().options[answerIndex].style[index])
                pleegouderHandler.puntenTelling[index]++;

        HandleAction($"Situatie {pleegouderHandler.CurrentIndex} {option}");
    }

    public void HandleAction(string dialogueOption)
    {
        pleegouderHandler.CurrentIndex++;
        switch (dialogueOption)
        {
            case "Situatie 0 A": //Default no extra dialogue stuff
                pleegouderHandler.RotateChair();
                pleegouderHandler.CurrentState = PleegouderHandler.State.SITUATIE_0_A;
                break;
            case "Situatie 4 A":
                pleegouderHandler.HandleSituationFive();
                break;
            case "Situatie 5 A":
            case "Situatie 5 B":
                StartCoroutine(pleegouderHandler.NextDialogue("Pleegouders", "End minigame", 1));
                pleegouderHandler.CurrentState = PleegouderHandler.State.MINIGAME_END;
                break;
            default:
                pleegouderHandler.dialogueHandler.SetDialogue("Pleegouders", dialogueOption);
                switch (dialogueOption)
                {
                    case "Situatie 0 B":
                        pleegouderHandler.CurrentState = PleegouderHandler.State.SITUATIE_0_B;
                        break;
                    case "Situatie 1 A":
                    case "Situatie 1 B":
                        packedBord.SetActive(false);
                        unpackedBord.SetActive(true);
                        pleegouderHandler.CurrentState = PleegouderHandler.State.SITUATIE_1_OPTION;
                        break;
                    case "Situatie 2 A":
                    case "Situatie 2 B":
                    case "Situatie 2 C":
                        pleegouderHandler.CurrentState = PleegouderHandler.State.SITUATIE_2_OPTION;
                        break;
                    case "Situatie 3 A":
                    case "Situatie 3 B":
                    case "Situatie 3 C":
                        pleegouderHandler.CurrentState = PleegouderHandler.State.SITUATIE_3_OPTION;
                        break;
                    case "Situatie 4 B":
                    case "Situatie 4 C":
                        pleegouderHandler.CurrentState = PleegouderHandler.State.SITUATIE_4_OPTION;
                        break;
                }
                break;
        }
    }

    public void Reset()
    {
        packedBord.SetActive(true);
        unpackedBord.SetActive(false);
        situatieText.text = "";
        for (int index = 0; index < options.Length; index++)
            options[index].SetActive(false);
    }
}
