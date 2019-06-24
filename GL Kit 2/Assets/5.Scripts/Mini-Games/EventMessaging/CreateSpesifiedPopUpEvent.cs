using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This event is used to create a popup from a spesified dialogue object, and not one intended to be read in sequence.
//Usage: Used as part of the pop up UI display.
//--------------------------------------------------

public class CreateSpesifiedPopUpEvent : GameLabEvent
{
    public DialogueObject dialogueObject;
    public CreateSpesifiedPopUpEvent(DialogueObject pDialogueObject = null)
    {
        dialogueObject = pDialogueObject;
    }
}
