using System;
using GameLab;

public class DetectedMarkerEvent : GameLabEvent
{
    public string marker = string.Empty;
    public DetectedMarkerEvent(string marker)
    {
        this.marker = marker;
    } 
}

