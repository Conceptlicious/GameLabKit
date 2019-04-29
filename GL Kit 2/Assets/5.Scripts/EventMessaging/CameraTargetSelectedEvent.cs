using System;
using UnityEngine;

public class CameraTargetSelectEventInfo : EventInfo
{   
    public Transform FocalA;
    public Transform FocalB;
    public CameraTargetSelectEventInfo(string pDescription, Transform pFocalA, Transform pFocalB)
    {
        eventDescription = pDescription;
        FocalA = pFocalA;
        FocalB = pFocalB;
    } 
}

