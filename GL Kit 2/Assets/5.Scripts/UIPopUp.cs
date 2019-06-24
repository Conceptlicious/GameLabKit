using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script holds data about the fields and values used to create our procedural popups.
//Usage: Used to hold data from dialogue object for UIPopUpManager
//--------------------------------------------------

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
