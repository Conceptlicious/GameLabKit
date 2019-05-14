using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using CustomEventCallbacks;
using EventType = CustomEventCallbacks.EventType;
using GameLab;
using UnityEngine.Rendering.PostProcessing;
using Vector3 = UnityEngine.Vector3;

public class CameraControl : MonoBehaviour
{
    public delegate void Updateables();

    private enum TargetPoints
    {
        ORIGIN,
        CENTRE,
        TARGET,
        
    };
    
    
    private Updateables handler;
    private Camera cam;
    DepthOfField depthOfField = null;

    private Transform[] targetList = new Transform[5];
    private float startTime = 0.0f;

    private int currentTargetIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        registerAllListeners();
        SetUpCamera();
        handler += NullUpdate;     
    }

    private void SetUpCamera()
    {
        cam = Camera.main;
        
        //Grab post processing profile
        PostProcessProfile postProfile = null;
        PostProcessVolume volume = cam.GetComponent<PostProcessVolume>();
        
        
        if (volume != null)
        {
            postProfile = volume.profile;
        }
        //Create if not found
        else
        {
            volume = cam.gameObject.AddComponent<PostProcessVolume>();
            PostProcessProfile newProfile = new PostProcessProfile();
            postProfile = newProfile;
            volume.profile = postProfile;
        }
        
        //Get the DepthOfField "component" and set initial settings
        postProfile.TryGetSettings(out depthOfField);
        if (depthOfField != null)
        {
            depthOfField.focusDistance.value = Settings.VAL_CAMERA_BLUR_FOCALDISTANCE;
            depthOfField.kernelSize.value = KernelSize.VeryLarge;
        }
    }
    
    
    /// <summary>
    /// Registers all event listeners this class needs to care about.
    /// </summary>
    private void registerAllListeners()
    {
        EventManager.Instance.AddListener<CameraTargetSelectEvent>(OnTargetSelect);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        handler();
    }

    private void NullUpdate()
    {
        
    }
     

    private void MoveToTarget()
    {      
        float fracComplete = (Time.time - startTime) / Settings.VAL_CAMERA_TRANSITION_SECONDS;

        
        //Lerp backwards using a sine functioning to create zoom
        //cam.transform.position = Vector3.Lerp(targetList[(int)TargetPoints.ORIGIN].position, targetList[(int)TargetPoints.TARGET].position, fracComplete);
       
        float sine = Mathf.Sin(Mathf.PI * fracComplete);
        cam.transform.position = Vector3.Lerp(targetList[(int)TargetPoints.ORIGIN].position, targetList[(int)TargetPoints.TARGET].position, fracComplete);
        cam.transform.position += targetList[(int)TargetPoints.CENTRE].position * sine;
        
        
        //Modulate depth of field to simulate blur
        if (depthOfField != null)
        {
            depthOfField.focalLength.value = (Settings.VAL_CAMERA_BLUR_FOCALLENGTH_MAX * sine);
        }    
       
        
        if (fracComplete >= 0.999f)
        {
            cam.transform.position = targetList[(int) TargetPoints.TARGET].position;
            handler -= MoveToTarget;

            GameObject.Destroy(targetList[(int) TargetPoints.CENTRE].gameObject);
                    
        }
        
    }

    private void OnTargetSelect(CameraTargetSelectEvent info)
    {
        
        if (info != null)
        {
            //If the next and previous points are identical, keep the next but set the previous to the camera's current position
            //Useful on start since the camera has not yet focused a roomset before
            //info.FocalA.position = info.FocalA.position == info.FocalB.position  ? cam.transform.position : info.FocalA.position;
            
            GameObject __newEmptyCentre = new GameObject("Transition Centre Point");
       
            Vector3 zoomOffset = new Vector3(0.0f, 0.0f, -Settings.VAL_CAMERA_ZOOM_DISTANCE);

            Transform centre = __newEmptyCentre.transform;
            Vector3 centrePosition = (info.FocalB.position + info.FocalA.position) * 0.5f;
            centre.position = centrePosition;   
            centre.Translate(Vector3.back + (zoomOffset));
          
            targetList = new Transform[] { info.FocalA, centre, info.FocalB };      
            
            startTime = Time.time;
            handler += MoveToTarget;
        }
        else
        {
            //Notifies the user that this event is not being acted upon correctly since the EventInfo cannot be cast and lists the current method and class for easy back-searching 
            Debug.Log(
                System.String.Format(EventSystem.STR_INCORRECT_EVENT_TYPE_CAST, System.Reflection.MethodBase.GetCurrentMethod().Name, GetType().FullName));
        }
        
    }
}
