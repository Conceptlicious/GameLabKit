using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Key data, allowing
/// </summary>
public class DialogueObject : MonoBehaviour
{
    private string dialogueText = Settings.STR_DEFAULT_DIALOGUE; 

    public string DialogueText
    {
        get { return dialogueText; }
        set { dialogueText = value; }
    }

  
}
