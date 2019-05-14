using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using GameLab;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoadingTips : MonoBehaviour
{
    [SerializeField] private SlidingObject[] slidingObjects;
    [SerializeField] private Text textField;    
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private bool tipsAreRandom;
    private const float FREQUENCY_CHANGE = 2.0f;

    private int tipsIndex = 0;
    
    private delegate void Updateables();
    private Updateables handler;
    private float startTime = 0.0f;

    [Serializable]
    private struct SlidingObject
    {
        public GameObject mainObject;
        public Transform hiddenPosition;
        public Transform shownPosition;
        public Vector2 inOutPercentages;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        registerAllListeners();
        handler += NullUpdate;       
    }
    
    /// <summary>
    /// Registers all event listeners this class needs to care about.
    /// </summary>
    private void registerAllListeners()
    {
        EventManager.Instance.AddListener<CameraTargetSelectEvent>(OnTransition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        handler();
    }

    private void NullUpdate()
    {
        
    }

    private void MoveNPC()
    {
        float fracComplete = (Time.time - startTime) / Settings.VAL_CAMERA_TRANSITION_SECONDS;
        //On the starting and ending quarters of the wave, use the ramp of the curve  

        for (int i = 0; i < slidingObjects.Length; i++)
        {
            float sineCapped = fracComplete >=  slidingObjects[i].inOutPercentages.x && fracComplete <= slidingObjects[i].inOutPercentages.y ? 1.0f : Mathf.Abs(Mathf.Sin(FREQUENCY_CHANGE * (Mathf.PI * fracComplete)));
            slidingObjects[i].mainObject.transform.position = Vector3.Lerp(slidingObjects[i].hiddenPosition.position, slidingObjects[i].shownPosition.position, sineCapped);
        }
        
        //textBox.transform.position = 
        
        if(fracComplete >= 0.999f)
        {       
            handler -= MoveNPC;
        }
    }

    void OnValidate()
    {
        for (int i = 0; i < slidingObjects.Length; i++)
        {
            float x = slidingObjects[i].inOutPercentages.x;
            float y = slidingObjects[i].inOutPercentages.y;
            
            x = Mathf.Clamp01(x);
            y = Mathf.Clamp01(y);

            float tempX = x;
            float tempY = y;
            
            y = Mathf.Max(tempX, tempY);
            x = Mathf.Min(tempX, tempY);

            slidingObjects[i].inOutPercentages = new Vector2(x, y);
        }
    }

    private void OnTransition()
    {
        startTime = Time.time;
        textField.text = tipsAreRandom == true
            ? Dialogue.GetRandomText(dialogueObject.Info.fileIndex, dialogueObject.Info.containerIndex)
            : Dialogue.GetNextText(dialogueObject.Info.fileIndex, dialogueObject.Info.containerIndex, tipsIndex, out tipsIndex);

        handler += MoveNPC;
    }
}
