using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;

public class UIPopUpManager : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private Canvas parentCanvas;

    public void Start()
    {
        registerAllListeners();
    }

    /// <summary>
    /// Registers all event listeners this class needs to care about.
    /// </summary>
    private void registerAllListeners()
    {
        //EventSystem.RegisterListener(EventType.UI_NEXT_ROOM, OnNextRoomCommand);
        EventManager.Instance.AddListener<CreatePopUpEvent>(OnEventMarkerCall);
    }
    
    public void CreatePopUp()
    {
        Debug.Log("DETECT");
        //layout visible
        string path = Settings.PATH_PREFABS + Settings.OBJ_NAME_UI_POPUP;
        Debug.Log(path);
        Canvas UICanvas = GameObject.Instantiate(Resources.Load<Canvas>(path));
        //GameObject.Instantiate(UICanvas);
        if (UICanvas == null)
        {
            Debug.Log("UICanvas is null");
        }
        UICanvas.name = "hello";
        
        UIPopUp popup = UICanvas.GetComponent<UIPopUp>();
        Debug.Assert(popup != null, Settings.ERR_ASSERT_UI_POPUP_MISSING_COMPONENT);
        UIPopUp.PopupFields data = popup.PopupFieldsData;
        
        data.title.text = dialogueObject.GetTextAndIterate();
        data.body.text = dialogueObject.GetTextAndIterate();

        data.inputField.enabled = !dialogueObject.GetTextAndIterate().IsNullOrEmpty();
         
       /* string[] buttonNames = dialogueObject.GetTextAndIterate().Split(' ');
        for (int i = 0; i < buttonNames.Length; i++)
        {
            GameObject newButton = GameObject.Instantiate(data.button.gameObject, UICanvas.transform);
            Vector3 newPosition = data.buttonAnchorPoint.position;
            newPosition.x += (newButton.GetComponent<RectTransform>().rect.width + data.buttonPadding * i);
            newButton.transform.position = newPosition;
            newButton.GetComponentInChildren<Text>().text = buttonNames[i];
        }*/
    }

    public void OnEventMarkerCall()
    {    //Don't uncomment this until a bug is fixed
        CreatePopUp();
    }
}
