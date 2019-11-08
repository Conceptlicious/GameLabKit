﻿using CustomEventCallbacks;
using GameLab;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraControl : MonoBehaviour
{
	public delegate void Updateables();
	
	private enum TargetPoints
	{
		ORIGIN,
		CENTRE,
		TARGET,
		TOTAL
	};

	private Updateables handler;
	private Camera cam;

	private Transform[] targetList = new Transform[(int)TargetPoints.TOTAL];
	private float startTime = 0.0f;

	private int currentTargetIndex = 0;
	// Start is called before the first frame update

	private void OnEnable()
	{
		SetUpCamera();
		RegisterAllListeners();
	}

	private void SetUpCamera()
	{
		cam = Camera.main;
	}


	/// <summary>
	/// Registers all event listeners this class needs to care about.
	/// </summary>
	private void RegisterAllListeners()
	{
		Debug.LogError("registering camera control events");
		EventManager.Instance.AddListener<CameraTargetSelectEvent>(OnTargetSelect);
		EventManager.Instance.AddListener<CameraSnapEvent>(OnCameraSnap);
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

		if (fracComplete >= 0.999f)
		{
			cam.transform.position = targetList[(int)TargetPoints.TARGET].position;
			handler -= MoveToTarget;

			Destroy(targetList[(int)TargetPoints.CENTRE].gameObject);

			if (RoomManager.Instance.GetCurrentRoomID().y != RoomManager.Instance.GetCurrentRoomID().z)
			{
				FinishedRoomTransition newInfo = new FinishedRoomTransition();
				EventManager.Instance.RaiseEvent(newInfo);
			}
		}

	}

	private void OnCameraSnap(CameraSnapEvent info)
	{
		Debug.LogError("On camera snap");
		Debug.LogError("Will move camera to " + info.focalPoint.position);
		cam.transform.position = info.focalPoint.position;
		Debug.LogError("Moved camera to " + info.focalPoint.position);
		if (info.considerAsTransition == true)
		{
			Debug.LogError("Consider as transition");
			FinishedRoomTransition newInfo = new FinishedRoomTransition();
			EventManager.Instance.RaiseEvent(newInfo);
		}
	}


	private void OnTargetSelect(CameraTargetSelectEvent info)
	{
		if (info != null)
		{
			//If the next and previous points are identical, keep the next but set the previous to the camera's current position
			//Useful on start since the camera has not yet focused a roomset before
			//info.FocalA.position = info.FocalA.position == info.FocalB.position  ? cam.transform.position : info.FocalA.position;
			Debug.LogError("Shit is happening");
			GameObject __newEmptyCentre = new GameObject("Transition Centre Point");

			Vector3 zoomOffset = new Vector3(0.0f, 0.0f, -info.zoomDistance);

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