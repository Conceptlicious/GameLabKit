using System;
using GameLab;

public class DialogueProgressEvent : GameLabEvent
{
    //An int used to represent the index in an array of strings
    public int progress;
    public DialogueProgressEvent(int pProgress)
    {
        progress = pProgress;
    } 
}

