using System;

public class DialogueFetchNextEventInfo : EventInfo
{
    //An int used to represent the index in an array of strings
    public string fieldName;
    public DialogueFetchNextEventInfo(string pDescription, string pFieldName)
    {
        eventDescription = pDescription;
        fieldName = pFieldName;
    } 
}

