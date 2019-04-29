using System;

public class DetectedMarkerEventInfo : EventInfo
{
    public string marker;
    public DetectedMarkerEventInfo(string pDescription, string pMarker)
    {
        eventDescription = pDescription;
        marker = pMarker;
    } 
}

