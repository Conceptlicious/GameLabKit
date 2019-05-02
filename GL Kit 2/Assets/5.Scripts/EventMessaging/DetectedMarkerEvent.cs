using System;
using GameLab;

public class DetectedMarkerEvent : GameLabEvent
{
    public string marker;
    public DetectedMarkerEvent(string pMarker)
    {
        marker = pMarker;
    } 
}

