using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;

public class UIPopUpManager : MonoBehaviour
{
    [SerializeField] private DialogueObject[] levelPopupDialogueObjects;
    [SerializeField] private Canvas parentCanvas;
    private int levelIndex = 0;

    public void Start()
    {
        registerAllListeners();
        if (GameData.Initialised == false)
        {
            Dialogue.LoadAllText();
        }
    }

    /// <summary>
    /// Registers all event listeners this class needs to care about.
    /// </summary>
    private void registerAllListeners()
    {
        //EventSystem.RegisterListener(EventType.UI_NEXT_ROOM, OnNextRoomCommand);
        EventManager.Instance.AddListener<CreatePopUpEvent>(OnEventMarkerCall);
        EventManager.Instance.AddListener<CreateSpesifiedPopUpEvent>(OnCreateSpesifiedPopup);
        EventManager.Instance.AddListener<LevelProgressEvent>(OnLevelProgess);
    }

    public void Thing()
    {
        CreatePopUp(levelPopupDialogueObjects[levelIndex]);
    }
    

    public void CreatePopUp(DialogueObject pDiagObj)
    {
        //Make sure the info inside the container is actually popup-styled info
        if (pDiagObj.Info.fieldName != Settings.JSON_POPUP_IDENTIFIER_KEY &&
            pDiagObj.GetTextAndIterate() != Settings.JSON_POPUP_IDENTIFIER_VALUE)
        {
            Debug.Log(Settings.ERR_DIAG_OBJ_POPUP_INCORRECT);
            return;
        }
        
        //Need to iterate once to 
        pDiagObj.GetTextAndIterate();

        string path = Settings.PATH_PREFABS + Settings.OBJ_NAME_UI_POPUP;
        Debug.Log(path);
        Canvas UICanvas = GameObject.Instantiate(Resources.Load<Canvas>(path));
     
        if (UICanvas == null)
        {
            Debug.Log("UICanvas is null");
        }

        UICanvas.name = "Popup: " + pDiagObj.GetContainerName();

        UIPopUp popup = UICanvas.GetComponent<UIPopUp>();
        Debug.Assert(popup != null, Settings.ERR_ASSERT_UI_POPUP_MISSING_COMPONENT);
        UIPopUp.PopupFields data = popup.PopupFieldsData;      

        data.title.text = pDiagObj.GetTextAndIterate();
        data.body.text = pDiagObj.GetTextAndIterate();

        string textFieldEnabled = pDiagObj.GetTextAndIterate();
        bool t = (System.String.IsNullOrEmpty(textFieldEnabled) || textFieldEnabled.ToLower() == "false") == true ? false : true;
        Debug.Log("Input Field is: " + t);
        data.inputField.gameObject.SetActive(t);

        string[] buttonNames = pDiagObj.GetTextAndIterate().Split(' ');
        for (int i = 0; i < buttonNames.Length; i++)
        {
            GameObject newButton = GameObject.Instantiate(data.button.gameObject, UICanvas.transform);
            Vector3 newPosition = data.buttonAnchorPoint.position;
            newPosition.x += ((newButton.GetComponent<RectTransform>().rect.width * i) + data.buttonPadding);
            newButton.transform.position = newPosition;
            newButton.GetComponentInChildren<Text>().text = buttonNames[i];
        }
        
        //Disable original button to clone
        data.button.gameObject.SetActive(false); 
    }

    public void OnLevelProgess(LevelProgressEvent info)
    {
        levelIndex = Mathf.Clamp(info.levelID, 0, levelPopupDialogueObjects.Length - 1);
    }

    public void OnEventMarkerCall()
    {          
        //Don't uncomment this until a bug is fixed
        Debug.Log("level index: " + levelIndex);
        CreatePopUp(levelPopupDialogueObjects[levelIndex]);
    }

    public void OnCreateSpesifiedPopup(CreateSpesifiedPopUpEvent info)
    {
        CreatePopUp(info.dialogueObject);
    }
}
