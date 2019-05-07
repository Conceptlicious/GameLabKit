using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using GameLab;
using System;

[Serializable]
public struct MarkedEvents
{
    [Tooltip("Char markers found in the dialogue file, used to call events.")]
    public string marker;
    [Tooltip("Type of event to be fired when specified marker is detected.")]
    [ClassExtends(typeof(GameLabEvent))] public ClassTypeReference eventToCall;
}

public class EventMarkers : Singleton<EventMarkers>
{
    [SerializeField] private MarkedEvents[] couplings;

    public void ParseAndCall(string pText)
    {
        for (int i = 0; i < couplings.Length; i++)
        {
            if (pText.Contains(couplings[i].marker))
            {
                EventManager.Instance.RaiseEvent(  Activator.CreateInstance(couplings[i].eventToCall) as GameLabEvent );
            }
        }
    }
   
}
