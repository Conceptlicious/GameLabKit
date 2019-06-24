using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script can be used to modulate the post processing profile and volumes. It can be greatly expanded and 
//currently only is used to control the DepthOfField in order to simulate blur.
//Usage: Used currently as part of the scene transitioning.
//--------------------------------------------------

public class PostProcessControl : Singleton<PostProcessControl>
{
	private Camera cam;
	DepthOfField depthOfField = null;

	void Awake()
	{
		SetUpCamera();
	}

	public void SetDepthOfFieldFocal(float pAmount)
	{
		depthOfField.focalLength.value = pAmount;
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

}
