using System;

public class DialogueProgressEventInfo : EventInfo
{
    //An int used to represent the index in an array of strings
    public int progress;
    public DialogueProgressEventInfo(string pDescription, int pProgress)
    {
        eventDescription = pDescription;
        progress = pProgress;
    } 
}

