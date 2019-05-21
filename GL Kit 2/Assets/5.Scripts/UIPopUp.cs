using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIPopUp : MonoBehaviour
{
    [SerializeField] private PopupFields popupFields; 
    
    [Serializable]
    public struct PopupFields
    {
        public Text title;
        public Text body;
        public InputField inputField;
        public Button button;
        public Transform buttonAnchorPoint;
        public int buttonPadding;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PopupFields PopupFieldsData
    {
        get { return popupFields; }
    
    }
}
