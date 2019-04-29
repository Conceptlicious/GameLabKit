using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using EventType = CustomEventCallbacks.EventType;
using GameLab;


public class UIControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void FocusNextRoom()
    {
        NextRoomEvent newInfo = new NextRoomEvent();
        //EventSystem.ExecuteEvent(EventType.UI_NEXT_ROOM, newInfo);
        EventManager.Instance.RaiseEvent(newInfo);
    }
    
}
