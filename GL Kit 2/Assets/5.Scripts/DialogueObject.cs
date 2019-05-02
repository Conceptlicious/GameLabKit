using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object to be edited through the DialogueEditor script.
public class DialogueObject : MonoBehaviour
{
    private string dialogueText = Settings.STR_DEFAULT_DIALOGUE; 

    public string DialogueText
    {
        get { return dialogueText; }
        set { dialogueText = value; }
    }

  
}
