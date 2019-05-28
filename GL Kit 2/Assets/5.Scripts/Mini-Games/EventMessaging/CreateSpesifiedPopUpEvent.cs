using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class CreateSpesifiedPopUpEvent : GameLabEvent
{
    public DialogueObject dialogueObject;
    public CreateSpesifiedPopUpEvent(DialogueObject pDialogueObject = null)
    {
        dialogueObject = pDialogueObject;
    }
}
