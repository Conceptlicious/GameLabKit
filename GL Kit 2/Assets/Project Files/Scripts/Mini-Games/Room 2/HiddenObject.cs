using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using EventType = CustomEventCallbacks.EventType;

public class HiddenObject : MonoBehaviour
{
    [SerializeField]
    private Settings.R2_ObjectsToFind objectsToFind;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckObject()
    {
        bool correctObject = objectsToFind != Settings.R2_ObjectsToFind.INCORRECT ? true : false;
        EventInfo info = new ObjectFoundEventInfo(EventSystem.DESC_EVENT_OBJECT_FOUND + objectsToFind.ToString(), correctObject, objectsToFind);
        EventSystem.ExecuteEvent(EventType.R2_OBJECT_FOUND, info);      
    }
}
