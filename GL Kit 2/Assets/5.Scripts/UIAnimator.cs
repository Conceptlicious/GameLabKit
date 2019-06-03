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

    public enum MoveType
    {
        ARC,
        TRANSITION,
        TOTAL
    };

    private MoveType moveType;
   

    // Update is called once per frame
    void FixedUpdate()
    {
        handler?.Invoke();
    }
    
    private void MoveObjects()
    {
        if (slidingObject != null && slidingObject.Length > 0)
        {
            float fracComplete = (Time.time - startTime) / lengthOfAnimation;
            //On the starting and ending quarters of the wave, use the ramp of the curve  

            for (int i = 0; i < slidingObject.Length; i++)
            {
                switch (moveType)
                {
                       case MoveType.ARC:
                           MoveOverArc(i, fracComplete);
                           break;
                       
                       case MoveType.TRANSITION:
                           MoveBetweenPoints(i, fracComplete);
                           break;
                }
            }
        
            //textBox.transform.position = 
        
            if(fracComplete >= 0.999f)
            {       
                
                if (moveType == MoveType.TRANSITION)
                {
                   FlipPositions();
                }
                handler -= MoveObjects;
            }
        }
        
    }

    private void FlipPositions()
    {
        for (int i = 0; i < slidingObject.Length; i++)
        {
            Vector3 temporary = new Vector3();

            //flip the locations
            temporary = slidingObject[i].HiddenPosition.position;
            slidingObject[i].HiddenPosition.position = slidingObject[i].ShownPosition.position;
            slidingObject[i].ShownPosition.position = temporary;
        }
    }

    private void MoveOverArc(int pIndex, float pFracComplete)
    {
        float sineCapped = pFracComplete >=  slidingObject[pIndex].InOutPercentages.x && pFracComplete <= slidingObject[pIndex].InOutPercentages.y ? 1.0f : Mathf.Abs(Mathf.Sin(FREQUENCY_CHANGE * (Mathf.PI * pFracComplete)));
        slidingObject[pIndex].MainObject.transform.position = Vector3.Lerp(slidingObject[pIndex].HiddenPosition.position, slidingObject[pIndex].ShownPosition.position, sineCapped);
    }

    private void MoveBetweenPoints(int pIndex, float pFracComplete)
    {
        slidingObject[pIndex].MainObject.transform.position = Vector3.Lerp(slidingObject[pIndex].HiddenPosition.position, slidingObject[pIndex].ShownPosition.position, pFracComplete);
       
    }

    public void AnimateObjects(UISlidingObject[] pSlidingObject, float pLengthOfAnimation, MoveType pMoveType)
    {
        Debug.Log("Begin");
        startTime = Time.time;
        lengthOfAnimation = pLengthOfAnimation;
        slidingObject = pSlidingObject;
       // ResetPositions();
        handler += MoveObjects;
        moveType = pMoveType;
    }

    private void ResetPositions()
    {
        for (int i = 0; i < slidingObject.Length; i++)
        {
            slidingObject[i].MainObject.transform.position = slidingObject[i].HiddenPosition.position;
        }
    }
    
 }
 

