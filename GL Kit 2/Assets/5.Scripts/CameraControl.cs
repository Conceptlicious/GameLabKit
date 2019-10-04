﻿using CustomEventCallbacks;
using GameLab;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Vector3 = UnityEngine.Vector3;

public class CameraControl : MonoBehaviour
{
    public delegate void Updateables();

    [SerializeField] private bool fade;
    [SerializeField] private GameObject transitionPlane;

    private enum TargetPoints
    {
        ORIGIN,
        CENTRE,
        TARGET,
        TOTAL
    };

    private Updateables handler;
    private Camera cam;
    DepthOfField depthOfField = null;
    private bool shouldFade = false;

    private Transform[] targetList = new Transform[(int)TargetPoints.TOTAL];
    private float startTime = 0.0f;

    private int currentTargetIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        registerAllListeners();
        SetUpCamera();
    }

    private void SetUpCamera()
    {
        cam = Camera.main;
        EventManager.Instance.RaiseEvent(new SetInteractionEvent(true));

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
        EventManager.Instance.AddListener<CameraSnapEvent>(OnCameraSnap);
        EventManager.Instance.AddListener<SetInteractionEvent>(OnPlaneActivity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        handler?.Invoke();
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
        if (shouldFade == true && depthOfField != null && fade == true)
        {
            PostProcessControl.Instance.SetDepthOfFieldFocal(Settings.VAL_CAMERA_BLUR_FOCALLENGTH_MAX * sine);
            //depthOfField.focalLength.value = (Settings.VAL_CAMERA_BLUR_FOCALLENGTH_MAX * sine);
        }

        if (fracComplete >= 0.999f)
        {
            cam.transform.position = targetList[(int)TargetPoints.TARGET].position;
            handler -= MoveToTarget;

            Destroy(targetList[(int)TargetPoints.CENTRE].gameObject);

            if (RoomManager.Instance.GetCurrentRoomID().y != RoomManager.Instance.GetCurrentRoomID().z)
            {
                EventManager.Instance.RaiseEvent(new SetInteractionEvent(true));

                FinishedRoomTransition newInfo = new FinishedRoomTransition();
                EventManager.Instance.RaiseEvent(newInfo);
            }
        }

    }

    private void OnCameraSnap(CameraSnapEvent info)
    {
        cam.transform.position = info.focalPoint.position;
        if (info.considerAsTransition == true)
        {
            FinishedRoomTransition newInfo = new FinishedRoomTransition();
            EventManager.Instance.RaiseEvent(newInfo);
        }
    }


    private void OnTargetSelect(CameraTargetSelectEvent info)
    {
        EventManager.Instance.RaiseEvent(new SetInteractionEvent(false));

        if (info != null)
        {
            //If the next and previous points are identical, keep the next but set the previous to the camera's current position
            //Useful on start since the camera has not yet focused a roomset before
            //info.FocalA.position = info.FocalA.position == info.FocalB.position  ? cam.transform.position : info.FocalA.position;

            GameObject __newEmptyCentre = new GameObject("Transition Centre Point");

            Vector3 zoomOffset = new Vector3(0.0f, 0.0f, -info.zoomDistance);

            Transform centre = __newEmptyCentre.transform;
            Vector3 centrePosition = (info.FocalB.position + info.FocalA.position) * 0.5f;
            centre.position = centrePosition;
            centre.Translate(Vector3.back + (zoomOffset));

            targetList = new Transform[] { info.FocalA, centre, info.FocalB };
            shouldFade = info.shouldFade;

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

    private void OnPlaneActivity(SetInteractionEvent eventData)
    {
        transitionPlane.SetActive(eventData.planeState);
    }
}
