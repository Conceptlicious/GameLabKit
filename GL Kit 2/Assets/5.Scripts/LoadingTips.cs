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
    [SerializeField] private UISlidingObject[] slidingObject;
    [SerializeField] private Text textField;    
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private bool tipsAreRandom;

    private int tipsIndex = 0;
    
    private delegate void Updateables();
    private Updateables handler;
    
    // Start is called before the first frame update
    void Start()
    {
        registerAllListeners();     
    }
    
    /// <summary>
    /// Registers all event listeners this class needs to care about.
    /// </summary>
    private void registerAllListeners()
    {
        EventManager.Instance.AddListener<CameraTargetSelectEvent>(OnTransition);
    }

    private void OnTransition(CameraTargetSelectEvent info)
    {
        if (info != null & info.showTips == true)
        {
            textField.text = tipsAreRandom == true
                ? Dialogue.GetRandomText(dialogueObject.Info.fileIndex, dialogueObject.Info.containerIndex)
                : Dialogue.GetNextText(dialogueObject.Info.fileIndex, dialogueObject.Info.containerIndex, tipsIndex, out tipsIndex);
        
            UIAnimator.Instance.AnimateObjects(slidingObject, Settings.VAL_CAMERA_TRANSITION_SECONDS);
        }            
    }
}
