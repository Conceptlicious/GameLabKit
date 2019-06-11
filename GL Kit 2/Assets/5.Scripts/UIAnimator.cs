using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using GameLab;
using System;
using UnityEngine.UI;

public class UIAnimator 
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

    public enum BlurType
    {
        IN,
        OUT,
        NONE
    }

    private MoveType moveType;
    private BlurType blurType;

    public UIAnimator(UISlidingObject[] pSlidingObject, float pLengthOfAnimation, MoveType pMoveType, BlurType pBlurType)
    {
        startTime = Time.time;
        lengthOfAnimation = pLengthOfAnimation;
        slidingObject = pSlidingObject;
        moveType = pMoveType;
        blurType = pBlurType;
        handler += MoveObjects;
    }
    
    // Update is called once per frame
    public void FixedUpdateAnimator()
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

            switch (blurType)
            {
                    case BlurType.IN:
                        PostProcessControl.Instance.SetDepthOfFieldFocal(Settings.VAL_CAMERA_BLUR_FOCALLENGTH_MAX * fracComplete);
                        break;
                    case BlurType.OUT:
                        PostProcessControl.Instance.SetDepthOfFieldFocal(Settings.VAL_CAMERA_BLUR_FOCALLENGTH_MAX * (1.0f - fracComplete));
                        break;
                    case BlurType.NONE:
                        break;
            }
        
            //textBox.transform.position = 
        
            if(fracComplete >= 0.999f)
            {       
                handler -= MoveObjects;
                if (moveType == MoveType.TRANSITION)
                {
                   FlipPositions();
                }
                FinishedAnimatingUIEvent newInfo = new FinishedAnimatingUIEvent(this);
                EventManager.Instance.RaiseEvent(newInfo);
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

    /*public void AnimateObjects(UISlidingObject[] pSlidingObject, float pLengthOfAnimation, MoveType pMoveType)
    {
        Debug.Log("Begin");
        startTime = Time.time;
        lengthOfAnimation = pLengthOfAnimation;
        
       // AddNewAnimateables(pSlidingObject);
        
        slidingObject = pSlidingObject;
        //ResetPositions();
        moveType = pMoveType;
        handler += MoveObjects;
    }*/

   /* private void AddNewAnimateables(UISlidingObject[] pSlidingObject)
    {
        bool[] duplicates = new bool[pSlidingObject.Length];
        for (int i = 0; i < slidingObject.Count; i++)
        {
            for (int j = 0; j < pSlidingObject.Length; j++)
            {
                if (slidingObject[i] == pSlidingObject[j])
                {
                    duplicates[j] = true;
                    
                }
            }   
            
        }

        for (int i = 0; i < pSlidingObject.Length; i++)
        {
            if (duplicates[i] == false)
            {
                slidingObject.Add(pSlidingObject[i]);
            }
        }
       
    }*/
    
    private void ResetPositions()
    {
        for (int i = 0; i < slidingObject.Length; i++)
        {
            slidingObject[i].MainObject.transform.position = slidingObject[i].HiddenPosition.position;
        }
    }
    
 }
 

