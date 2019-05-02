using System;
using GameLab;

public class ObjectFoundEvent : GameLabEvent
{
    public bool correct;
    public Settings.R2_ObjectsToFind objectFound;
    public ObjectFoundEvent(bool pCorrect, Settings.R2_ObjectsToFind pObjectFound)
    {
        correct = pCorrect;
        objectFound = pObjectFound;
    } 
}

