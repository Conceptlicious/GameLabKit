using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using GameLab;
using System;

[Serializable]
public struct EventMarker
{
    public string Marker;
    [ClassExtends(typeof(GameLabEvent))] public ClassTypeReference EventType;
}

/************************************************************************/
/* TODO
 * -Add the ability to raise events with a type instance in event manager
 * -Make it so that the correct marker raises its associated event type 
 * (could be done through a helper method in the struct ;) )
 * -???
 * -Profit
 */
/************************************************************************/

public class EventMarkers : MonoBehaviour
{
    [SerializeField] private List<EventMarker> eventMarkers = new List<EventMarker>();

    [Tooltip("No protection is currently enabled.")]
    [SerializeField] private bool enableEventMarking = false;
    
    [Space(15)]
    [Header(Settings.STR_WARNING_EVENT_CHAIN_GENERIC_INFO)]
    
    [Tooltip("Char markers found in the dialogue file, used to call events.")]
    [SerializeField] private string[] markers;
    
    [Tooltip("Type of event to be fired when specified marker is detected.")]
    [SerializeField, ClassExtends(typeof(GameLabEvent))] private ClassTypeReference[] events;

  //  private Dictionary<string, EventType> markedEvents = new Dictionary<string, EventType>();

    void Start()
    {
        registerAllListeners();
    }
    
    /// <summary>
    /// Registers all event listeners this class needs to care about.
    /// </summary>
    private void registerAllListeners()
    {
        //EventSystem.RegisterListener(EventType.CALL_EVENT_CHAIN, OnEventMarkerCall);
        EventManager.Instance.AddListener<DetectedMarkerEvent>(OnEventMarkerCall);
        
    }
    
    public void OnEventMarkerCall(DetectedMarkerEvent info)
    {
             
        if (info != null)
        {
            if (enableEventMarking)
            {
                GenericEvent genericInfo = new GenericEvent(EventSystem.DESC_EVENT_GENERIC);
                //EventSystem.ExecuteEvent(markedEvents[info.marker], genericInfo);
            }
        }
        else
        {
            //Safeguard
        }    
        
    }
    /*
    void OnValidate()
    {
        //Cache marker length
        int markerLength = markers.Length;
        
        //If the marker list has been expanded, but no markers have yet been added then fill the 
        //list with the default marker char
        for (int i = 0; i < markerLength; i++)
        {
            if (markers[i] == "" || markers[i] == null)
            {
                markers[i] = Settings.DEFAULT_EVENTMARKER_MARKER;
            }
        }
        
        //Cache the old event length
        int oldEventsLength = events.Length;
        
        //
        if (oldEventsLength != markerLength)
        {
            //Save the event list in a temporary array so we can resize the event list
            ClassTypeReference[] temp = events;
            
            //Set the events size to the size of the marker list
            events = new ClassTypeReference[markerLength];
            
            //Depending on if markers got expanded or contracted
            int loopLength = oldEventsLength > markers.Length ? markers.Length : oldEventsLength;
            for (int i = 0; i < loopLength; i++)
            {
                //Set the events from our cached temp list
                events[i] = temp[i];
            }
            
            //Fill blank spots in the events list with Generic Events
            if(oldEventsLength < markerLength)
            {
                for (int i = oldEventsLength; i < markerLength - oldEventsLength; i++)
                {
                    //events[i] = EventType.GENERIC_EVENT;
                    //events[i] =
                }
            }

            markedEvents.Clear();
            for (int i = 0; i < markerLength; i++)
            {
                markedEvents.Add(markers[i], events[i]);
            }
        }
    }
    */
    
}
