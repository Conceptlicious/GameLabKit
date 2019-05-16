using System;
using UnityEngine;
using GameLab;

public class CameraTargetSelectEvent : GameLabEvent
{   
    public Transform FocalA;
    public Transform FocalB;
    public bool shouldFade = false;
    public bool showTips = false;
    
    public CameraTargetSelectEvent(Transform pFocalA, Transform pFocalB, bool pShouldFade, bool pShowTips)
    {
        FocalA = pFocalA;
        FocalB = pFocalB;

        shouldFade = pShouldFade;
        showTips = pShowTips;       
    } 
}

