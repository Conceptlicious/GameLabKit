namespace CustomEventCallbacks
{
    public enum EventType
    {
        GENERIC_EVENT,
        SYSTEM_LOAD_ROOM,        
        BEGIN_MINI_GAME,
        CAMERA_ZOOM_ROOM,
        CAMERA_TARGET_SELECT,
        
        UI_PROGRESS_DIALOGUE,
	    UI_MORE_INFO,
        UI_NEXT_ROOM,
        
        CALL_EVENT_CHAIN
    }


    public enum ChainSafeEventTypes
    {
        CALL_EVENT_CHAIN
    }

}