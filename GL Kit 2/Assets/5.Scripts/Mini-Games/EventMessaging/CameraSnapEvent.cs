using System.Collections;
using System.Collections.Generic;
using GameLab;
using UnityEngine;

public class CameraSnapEvent : GameLabEvent
{
    public Transform focalPoint;
    public CameraSnapEvent(Transform pFocalPoint)
    {
        focalPoint = pFocalPoint;
    }
}
