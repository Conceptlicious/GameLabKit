using System;
using GameLab;

public class ObjectFoundEvent : GameLabEvent
{
    public bool correct;
    public Settings.R2_ObjectsToFind objectFound;
    public ObjectFoundEvent(bool correct, Settings.R2_ObjectsToFind objectFound)
    {
        this.correct = correct;
        this.objectFound = objectFound;
    } 
}

