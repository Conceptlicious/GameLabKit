using System.Collections;
using System.Collections.Generic;
using GameLab;
using UnityEngine;

public class CameraSnapEvent : GameLabEvent
{
    public Transform focalPoint;
    public bool considerAsTransition;
    public CameraSnapEvent(Transform pFocalPoint, bool pConsiderAsTranstion)
    {
        focalPoint = pFocalPoint;
        considerAsTransition = pConsiderAsTranstion;
    }
}
