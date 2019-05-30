using System;
using UnityEngine;
using GameLab;

public class CameraTargetSelectEvent : GameLabEvent
{
	//FocalA and FocalB have to be focalA and FocalB
	public Transform FocalA = null;
	public Transform FocalB = null;
	public bool shouldFade = false;
	public bool showTips = false;
	public float zoomDistance;
	
	public CameraTargetSelectEvent(Transform focalA, Transform focalB, float pZoomDistance, bool shouldFade, bool showTips)
	{
		this.FocalA = focalA;
		this.FocalB = focalB;

		zoomDistance = pZoomDistance;

		this.shouldFade = shouldFade;
		this.showTips = showTips;       
	} 
}

