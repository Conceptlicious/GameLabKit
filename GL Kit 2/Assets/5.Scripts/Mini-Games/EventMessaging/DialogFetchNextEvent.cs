using System;
using GameLab;

public class DialogueFetchNextEvent : GameLabEvent
{
    //An int used to represent the index in an array of strings
    public string fieldName;
    public DialogueFetchNextEvent(string pFieldName)
    {
        fieldName = pFieldName;
    } 
}

