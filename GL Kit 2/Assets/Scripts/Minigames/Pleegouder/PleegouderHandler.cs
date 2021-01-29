using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PleegouderHandler : Minigame
{
    public static PleegouderHandler instance;

    private OptionHandler optionHandler => OptionHandler.instance;
    private Charlie charlieHandler => Charlie.instance;

    [Header("Gameobjects")]
    [SerializeField] GameObject playerChair;
    [SerializeField] Vector3 originalChairRotation;
    [SerializeField] GameObject colaObject;

    [Header("Infromation buttons")]
    [SerializeField] GameObject[] infoButtons;

    [Header("Pleegouder Data")]
    [SerializeField] int currentIndex = 0;
    public int[] puntenTelling = new int[3];

    [Header("Opvoedings style")]
    [SerializeField] GameObject opvoedStyle;
    [SerializeField] TMP_Text opvoedStyleText;

    public int CurrentIndex
    {
        get => currentIndex;
        set => currentIndex = value;
    }

    [SerializeField] Option[] pleegOuderOptions;

    [Header("State of the game")]
    [SerializeField] State currentState = State.IDLE;

    public State CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    public Option GetCurrentOption()
    {
        return pleegOuderOptions[currentIndex];
    }

    public void Start()
    {
        base.Start();
        instance = this;
        originalChairRotation = playerChair.transform.localEulerAngles;
    }

    public override void StartMinigame()
    {

    }

    public override void FinishMinigame()
    {
        CurrentState = State.MINIGAME_ENDED;
        player.AddTrophy(trophy);
        StartCoroutine(NextDialogue("Pleegouders", "Ending", 0));
    }

    public override void OnDialogueFinished()
    {
        if(!hasStarted)
        {
            switch(labHandler.CurrentState)
            {
                case LabHandler.State.JEUGDZORG:
                    instructionScreen[0].SetActive(true);
                    break;
            }
        } else
        {
            switch(CurrentState)
            {
                case State.IDLE:
                    charlieHandler.CurrentState = Charlie.State.WALKING;
                    currentState = State.STARTED;
                    break;
                case State.SITUATIE_0_B:
                    RotateChair();
                    charlieHandler.SetCharlieOnChair();
                    break;
                case State.SITUATIE_0_A:
                case State.SITUATIE_1:
                case State.SITUATIE_2:
                case State.SITUATIE_3:
                case State.SITUATIE_4:
                case State.SITUATIE_5:
                    optionHandler.SetOptions(GetCurrentOption());
                    break;
                case State.SITUATIE_1_OPTION:
                    StartCoroutine(NextDialogue("Pleegouders", "Situatie 2 Start", 1));
                    currentState = State.SITUATIE_2;
                    break;
                case State.SITUATIE_2_OPTION:
                    StartCoroutine(NextDialogue("Pleegouders", "Situatie 3 Start", 1));
                    currentState = State.SITUATIE_3;
                    break;
                case State.SITUATIE_3_OPTION:
                    colaObject.SetActive(true);
                    StartCoroutine(NextDialogue("Pleegouders", "Situatie 4 Start", 1));
                    currentState = State.SITUATIE_4;
                    break;
                case State.SITUATIE_4_OPTION:
                    opvoedStyleText.text = $"{GetParentingStyle()}";
                    opvoedStyle.SetActive(true);
                    CurrentState = State.MINIGAME_END;
                    break;
                case State.MINIGAME_END:
                    DisplayTrophy();
                    break;
                case State.MINIGAME_ENDED:
                    CurrentState = State.IDLE;
                    minigameHandler.ReturnToLab(true);
                    break;

            }
        }
    }

    public void CloseStyle()
    {
        opvoedStyle.SetActive(false);
        StartCoroutine(NextDialogue("Pleegouders", "Ticket", 0));
    }

    /// <summary>
    /// Handles getting the opvoedstijl
    /// </summary>
    /// <returns></returns>
    public string GetParentingStyle()
    {
        int? maxVal = null; //nullable so this works even if you have all super-low negatives
        int index = -1;
        for (int i = 0; i < puntenTelling.Length; i++)
        {
            int thisNum = puntenTelling[i];
            if (!maxVal.HasValue || thisNum > maxVal.Value)
            {
                maxVal = thisNum;
                index = i;
            }
        }
        switch(index)
        {
            case 0:
                return "<b>Je hebt in deze situaties de Verwaarlozende opvoedstijl</b>\n\nBij deze stijl worden er weinig regels gesteld en bieden de ouders weinig steun, veiligheid en betrokkenheid. Het kind wordt aan zijn lot overgelaten en mag het zelf allemaal uitzoeken.\n\n<b>Voordelen:</b>\nWeinig tot geen aandacht geven aan een kind is gemakkelijk. Een ouder heeft zo alle tijd voor zichzelf en/of het werk.\n\n<b>Nadelen:</b>\nVan deze stijl leert een kind niets. Een kind leert niet wat verstandig of eerlijk is. Daardoor loopt een kind meer risico op het in aanraking komen met “verkeerde” vriendjes. Een kind wat in deze stijl wordt opgevoed voelt zich ook vaak eenzaam, in de steek gelaten en niet geliefd.";
            case 1:
                return "<b>Je hebt in deze situaties de Autoritaire opvoedstijl</b>\n\nBij deze stijl worden er veel regels gesteld. De ouder is in dit geval de baas en het kind moet gehoorzamen. Als je regels niet worden opgevolgd, krijgt het kind straf. De regels van de ouders worden vaak niet uitgelegd en er is geen ruimte voor discussie.\n\n<b>Voordelen:</b>\nDe ouders zijn de baas en bepalen wat er gebeurt. Zo weet het kind altijd wat hij kan verwachten. Er zijn kinderen die baat kunnen hebben bij een autoritaire opvoeding.\n\n<b>Nadelen:</b>\nHet kind kan door deze opvoedstijl angstig en volgzaam of agressief of opstandig worden. Het kind ontwikkelt weinig zelfvertrouwen en zelfstandigheid.Het beredeneren van keuzes en het maken van moeilijke beslissingen zijn dan lastig. Er kan door deze manier ook conflict ontstaan. Als het kind groter wordt zal hij zich meer onttrekken aan zijn ouders.";
            case 2:
                return "<b>Je hebt in deze situaties de Democratische opvoedstijl</b>\n\nBij deze stijl stellen de ouders regels op en letten op de wensen en behoefte van hun kind. De leiding wordt gegeven met liefde en wordt rekening gehouden met de ontwikkeling van het kind. De gemaakte regels worden onderbouwd met argumenten. Het kind voelt zich aangemoedigd en gesteund.\n\n<b>Voordelen:</b>\nDoor deze opvoeding wordt het zelfvertrouwen en de zelfstandigheid van het kind gestimuleerd. Het kind wordt gerespecteerd en zijn ontwikkeling wordt gevolgd. Er is openheid tussen kind en opvoeder. Vaak doen kinderen iets beter hun best op school en zijn ze meestal optimistisch. Ook hebben ze minder gedragsproblemen en kunnen beter omgaan met negatieve invloed van leeftijdsgenoten.\n\n<b>Nadelen:</b>\nDeze stijl van opvoeden kost erg veel energie. Ook zal het kind niet altijd blindelings gehoorzamen en wil graag argumenten horen. Ouder en kind moeten aanvaarden dat hun meningen niet altijd hetzelfde zijn en gaan hier vaak over in gesprek. Het gevaar kan zijn dat het kind te mondig en te vrij wordt. Daarnaast kan het zo zijn dat de ouders geen grenzen meer kunnen stellen en altijd blijven praten. ";
            default: return "";
        }
    }

    public void ClickedStart()
    {
        if (HasLoaded) ResetMinigame();
        base.ClickedStart();
        foreach (GameObject o in infoButtons)
            o.SetActive(true);
        dialogueHandler.SetDialogue("Pleegouders", "Charlie start");
    }

    public void RotateChair()
    {
        playerChair.transform.localEulerAngles = new Vector3(0, 90f, 0);
        currentState = State.SITUATIE_1;
        StartCoroutine(NextDialogue("Pleegouders", $"Situatie {CurrentIndex} Start", 0));
    }

    public void HandleSituationFive()
    {
        //TODO: Make charlie go upstairs
        optionHandler.SetOptions(GetCurrentOption());
        CurrentState = State.SITUATIE_5;
    }

    public override void ResetMinigame()
    {
        hasStarted = false;
        puntenTelling = new int[3];
        colaObject.SetActive(false);
        CurrentIndex = 0;
        playerChair.transform.localEulerAngles = originalChairRotation;
        CurrentState = State.IDLE;
        optionHandler.Reset();
        charlieHandler.Reset();
        foreach (GameObject o in infoButtons)
            o.SetActive(false);
        instructionScreen[0].SetActive(false);
    }

    public enum State
    {
        IDLE,
        STARTED,
        SITUATIE_0_A,
        SITUATIE_0_B,
        SITUATIE_1,
        SITUATIE_1_OPTION,
        SITUATIE_2,
        SITUATIE_2_OPTION,
        SITUATIE_3,
        SITUATIE_3_OPTION,
        SITUATIE_4,
        SITUATIE_4_OPTION,
        SITUATIE_5,
        MINIGAME_END,
        MINIGAME_ENDED
    }
}
