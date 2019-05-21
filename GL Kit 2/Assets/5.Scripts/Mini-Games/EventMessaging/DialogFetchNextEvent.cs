using System;
using GameLab;

public class DialogueFetchNextEvent : GameLabEvent
{
    public string fieldName = string.Empty;
    public DialogueFetchNextEvent(string fieldName)
    {
        this.fieldName = fieldName;
    } 
}

