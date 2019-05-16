using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIPopUp : MonoBehaviour
{
    [SerializeField] private GameObject window;
    [SerializeField] private FieldDialogueCoupling title;
    [SerializeField] private FieldDialogueCoupling[] buttons;
    [SerializeField] private FieldDialogueCoupling mainText;

    [Serializable]
    private struct FieldDialogueCoupling
    {
        public Text textField;
        public DialogueObject dialogueObject;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
