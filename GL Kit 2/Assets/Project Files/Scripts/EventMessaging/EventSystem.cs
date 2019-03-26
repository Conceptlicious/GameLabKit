using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomEventCallbacks
{
	public static class EventSystem
	{
		public delegate void EventListener(EventInfo e);
		private static Dictionary<EventType, List<EventListener>> eventListeners = new Dictionary<EventType, List<EventListener>>();

		public static void RegisterListener(EventType eventType, EventListener listener)
		{
			if (eventListeners.ContainsKey(eventType) == false)
			{
				eventListeners.Add(eventType, new List<EventListener>());
			}

			if (eventListeners[eventType] == null)
			{
				eventListeners[eventType] = new List<EventListener>();
			}
	

			eventListeners[eventType].Add(listener);
		}
	
		public static void UnregisterListener(EventType eventType, EventListener pListener)
		{
			//eventListeners[event]
			Debug.Log("Trying to unregister");
			eventListeners[eventType].Remove(pListener);
			Debug.Log("Unregistered");
		}

		public static void ExecuteEvent(EventType eventType, EventInfo eventInfo)
		{
			//Debug.Log("Trying to execute event: " + eventType.ToString() + ", with desc: " + eventInfo.eventDescription);			
	
			if (eventListeners.ContainsKey(eventType) == false || 
			    eventListeners == null ||
			    eventListeners[eventType] == null || 
			    eventListeners[eventType].Count < 1)
			{
				//No one's listening, return.
				Debug.Log("No one is listening to " + eventInfo.eventDescription);
				return;
			}

	
			foreach (EventListener evLi in eventListeners[eventType])
			{
				evLi(eventInfo);
			}
		}
		
		//-----
		//----------	EVENT INFORMATION
		//-----
		public static readonly string DESC_EVENT_GENERIC = "A miscellaneous event has been fired.";
		public static readonly string DESC_EVENT_CAMERA_TARGET = Settings.STR_EVENT_FIRED + "A camera has been set to target a new location";
		public static readonly string DESC_EVENT_CAMERA_DESTINATION_REACH = Settings.STR_EVENT_FIRED + "A camera has reached it's desired destination.";
		public static readonly string DESC_EVENT_UI_NEXT_ROOM = Settings.STR_EVENT_FIRED + "A button has been pressed requesting progress to the following room.";
		

	}
	
	

}
