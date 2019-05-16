using System.Collections;
using System.Collections.Generic;
using UnityEngine;using CustomEventCallbacks;
using GameLab;
using System;
using UnityEngine.UI;

public class UIAnimator : Singleton<UIAnimator>
{
    private const float FREQUENCY_CHANGE = 2.0f;
    
    private delegate void Updateables();
    private Updateables handler;
    private float startTime = 0.0f;

    private UISlidingObject[] slidingObject = null;
    private float lengthOfAnimation = 0.0f;
   

    // Start is called before the first frame update
    void Start()
    {
        handler += NullUpdate; 
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        handler();
    }

    private void NullUpdate()
    {
        
    }
    
    private void MoveObjects()
    {
        if (slidingObject != null && slidingObject.Length > 0)
        {
            float fracComplete = (Time.time - startTime) / lengthOfAnimation;
            //On the starting and ending quarters of the wave, use the ramp of the curve  

            for (int i = 0; i < slidingObject.Length; i++)
            {
                float sineCapped = fracComplete >=  slidingObject[i].InOutPercentages.x && fracComplete <= slidingObject[i].InOutPercentages.y ? 1.0f : Mathf.Abs(Mathf.Sin(FREQUENCY_CHANGE * (Mathf.PI * fracComplete)));
                slidingObject[i].MainObject.transform.position = Vector3.Lerp(slidingObject[i].HiddenPosition.position, slidingObject[i].ShownPosition.position, sineCapped);
            }
        
            //textBox.transform.position = 
        
            if(fracComplete >= 0.999f)
            {       
                handler -= MoveObjects;
            }
        }
        
    }

    public void AnimateObjects(UISlidingObject[] pSlidingObject, float pLengthOfAnimation)
    {
        startTime = Time.time;
        lengthOfAnimation = pLengthOfAnimation;
        slidingObject = pSlidingObject;
        handler += MoveObjects;
    }
    
 }
 

