using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpManager : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CreatePopUp()
    {
        //layout visible
        Canvas UICanvas = Resources.Load<Canvas>(Settings.PATH_ASSETS_RESOURCES + Settings.PATH_PREFABS + Settings.OBJ_NAME_UI_POPUP);
        UIPopUp popup = UICanvas.GetComponent<UIPopUp>();
        Debug.Assert(popup != null, Settings.ERR_ASSERT_UI_POPUP_MISSING_COMPONENT);
        UIPopUp.PopupFields data = popup.PopupFieldsData;
        
        data.title.text = dialogueObject.GetTextAndIterate();
        data.body.text = dialogueObject.GetTextAndIterate();

        data.inputField.enabled = dialogueObject.GetTextAndIterate().IsNullOrEmpty() ? true : false;
         
        string[] buttonNames = dialogueObject.GetTextAndIterate().Split(' ');
        for (int i = 0; i < buttonNames.Length; i++)
        {
            GameObject newButton = GameObject.Instantiate(data.button.gameObject, UICanvas.transform);
            Vector3 newPosition = data.buttonAnchorPoint.position;
            newPosition.x += (newButton.GetComponent<RectTransform>().rect.width + data.buttonPadding * i);
            newButton.transform.position = newPosition;
            newButton.GetComponentInChildren<Text>().text = buttonNames[i];
        }
    }

    public void OnEventMarkerCall()
    {
        //iterate container index
        CreatePopUp();
    }
}
