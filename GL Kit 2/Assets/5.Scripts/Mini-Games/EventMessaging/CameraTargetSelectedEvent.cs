using System;
using UnityEngine;
using GameLab;

public class CameraTargetSelectEvent : GameLabEvent
{   
    public Transform FocalA;
    public Transform FocalB;
    public CameraTargetSelectEvent(Transform pFocalA, Transform pFocalB)
    {

        FocalA = pFocalA;
        FocalB = pFocalB;
    } 
}

