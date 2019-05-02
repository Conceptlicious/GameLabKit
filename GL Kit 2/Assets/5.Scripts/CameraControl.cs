using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using EventType = CustomEventCallbacks.EventType;
using GameLab;

public class CameraControl : MonoBehaviour
{
    public delegate void Updateables();

    private Updateables handler;
    private Camera cam;

    private Vector3[] targetList = new Vector3[3];

    private int currentTargetIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        registerAllListeners();
        cam = Camera.main;
        handler += NullUpdate;
       
    }
    
    
    /// <summary>
    /// Registers all event listeners this class needs to care about.
    /// </summary>
    private void registerAllListeners()
    {
        EventManager.Instance.AddListener<CameraTargetSelectEvent>(OnTargetSelect);

    }

    // Update is called once per frame
    void Update()
    {
        handler();
    }

    private void NullUpdate()
    {
        
    }

    private void MoveToTarget()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetList[currentTargetIndex], Settings.VAL_CAMERA_MOVEMENT_SPEED);
        if (cam.transform.position == targetList[currentTargetIndex])
        {
            if ( currentTargetIndex < targetList.Length - 1)
            {
                Debug.Log("reached target " + currentTargetIndex + " and incrementing to " + (currentTargetIndex + 1));
                currentTargetIndex++;
            }
            else
            {
                currentTargetIndex = 0;
                handler -= MoveToTarget;
            }


        }
    }

    private void OnTargetSelect(CameraTargetSelectEvent info)
    {
        
        if (info != null)
        {
            //If the next and previous points are identical, keep the next but set the previous to the camera's current position
            //Useful on start since the camera has not yet focused a roomset before
            info.FocalA.position = info.FocalA.position == info.FocalB.position  ? cam.transform.position : info.FocalA.position;
            Vector3[] list = new Vector3[] { info.FocalA.position, info.FocalB.position, info.FocalB.position };
        
            for (int i = 0; i < targetList.Length; i++)
            {
                float zoom = i < targetList.Length - 1 ? Settings.VAL_CAMERA_ZOOM_DISTANCE : 0.0f;
                Debug.Log("Setting zoom for pos " + i + " to: " + zoom);
                SetTargetList(i, list[i], zoom);
            }

            handler += MoveToTarget;
        }
        else
        {
            //Notifies the user that this event is not being acted upon correctly since the EventInfo cannot be cast and lists the current method and class for easy back-searching 
            Debug.Log(
                System.String.Format(EventSystem.STR_INCORRECT_EVENT_TYPE_CAST, System.Reflection.MethodBase.GetCurrentMethod().Name, GetType().FullName));
        }
        
    }

    private void SetTargetList(int pIndex, Vector3 pFocal, float pZoomDistance)
    {
        targetList[pIndex] = new Vector3(pFocal.x, pFocal.y, pFocal.z - pZoomDistance);       
    }
}
