using System;

public class ObjectFoundEventInfo : EventInfo
{
    public bool correct;
    public Settings.R2_ObjectsToFind objectFound;
    public ObjectFoundEventInfo(string pDescription, bool pCorrect, Settings.R2_ObjectsToFind pObjectFound)
    {
        eventDescription = pDescription;
        correct = pCorrect;
        objectFound = pObjectFound;
    } 
}

