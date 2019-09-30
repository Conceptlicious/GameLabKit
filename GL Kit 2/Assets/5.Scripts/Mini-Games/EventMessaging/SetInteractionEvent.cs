using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

public class SetInteractionEvent : GameLabEvent
{
    public bool planeState; 

    public SetInteractionEvent(bool allowInteraction)
    {
        planeState = allowInteraction ? false : true;
    }
}
