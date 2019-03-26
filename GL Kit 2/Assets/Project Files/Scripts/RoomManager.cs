using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using UnityEditorInternal;
using EventType = CustomEventCallbacks.EventType;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Transform[] roomFocalPoints = new Transform[Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS];
    

    private Vector2Int currentRoom = new Vector2Int(0, 0);

    void Start()
    {
        registerAllListeners();
    }
   
    /// <summary>
    /// Registers all event listeners this class needs to care about.
    /// </summary>
    private void registerAllListeners()
    {
        EventSystem.RegisterListener(EventType.UI_NEXT_ROOM, OnNextRoomCommand);
    }


    private void OnNextRoomCommand(EventInfo pInfo)
    {
        currentRoom.y = currentRoom.x;
        currentRoom.x++;
        Mathf.Clamp(currentRoom.x, 0, Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS);   
        //Focal A is current. Focal B is next. Current[New, Old]
        CameraTargetSelectEventInfo newInfo = new CameraTargetSelectEventInfo(EventSystem.DESC_EVENT_CAMERA_TARGET, roomFocalPoints[currentRoom.y], roomFocalPoints[currentRoom.x]);
        EventSystem.ExecuteEvent(EventType.CAMERA_TARGET_SELECT, newInfo);
    }

    void OnValidate()
    {
        int oldLength = roomFocalPoints.Length;
        if (oldLength != Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS)
        {
            
            Transform[] temp = roomFocalPoints;
            roomFocalPoints = new Transform[Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS];
            for (int i = 0; i < oldLength; i++)
            {
                roomFocalPoints[i] = temp[i];
            }

            for (int i = oldLength; i < Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS - oldLength; i++)
            {
                roomFocalPoints[i] = Resources.Load<GameObject>(
                    Settings.PATH_ASSETS_RESOURCES + Settings.PATH_PREFABS + Settings.OBJ_NAME_BLANK_GAMEOBJECT).transform;
            }
        }
    }
}
