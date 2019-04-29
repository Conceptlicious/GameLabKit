using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using UnityEditorInternal;
using EventType = CustomEventCallbacks.EventType;

public class RoomManager : MonoBehaviour
{
    [Tooltip("The available number of focal points is dictated by the Settings.cs file.")]
    [SerializeField] private Transform[] roomFocalPoints; 
    

    private Vector2Int currentRoom = new Vector2Int(0, 0);

    void Start()
    {
        registerAllListeners();
        FillFocalsWithBlanks();
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

    private void FillFocalsWithBlanks()
    {
        for (int i = 0; i < roomFocalPoints.Length; i++)
        {
            //Debug.Log("Room focal " + i + ": " + roomFocalPoints[i].position);
            if (roomFocalPoints[i] == null)
            {
                string path =
                     Settings.PATH_PREFABS + Settings.OBJ_NAME_BLANK_GAMEOBJECT;
                Debug.Log(path);
                GameObject blankGameObjectTransform = Resources.Load<GameObject>(path);
                if (blankGameObjectTransform == null)
                {
                    Debug.Log("Cannot instantiate " + Settings.OBJ_NAME_BLANK_GAMEOBJECT);
                }
                roomFocalPoints[i] = blankGameObjectTransform.transform;
            }
               
        }
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

           FillFocalsWithBlanks();
            
        }
    }
}
