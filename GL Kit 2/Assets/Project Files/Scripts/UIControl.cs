using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using EventType = CustomEventCallbacks.EventType;


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
        NextRoomEventInfo newInfo = new NextRoomEventInfo(EventSystem.DESC_EVENT_UI_NEXT_ROOM);
        EventSystem.ExecuteEvent(EventType.UI_NEXT_ROOM, newInfo);
    }
    
}
