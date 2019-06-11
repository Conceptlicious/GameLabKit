 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using EventType = CustomEventCallbacks.EventType;
using GameLab;


public class UIControl : MonoBehaviour
{
    private bool isTransitioning = false;
    // Start is called before the first frame update
    void Start()
    {
        RegisterAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// Registers all event listeners this class needs to care about.
    /// </summary>
    private void RegisterAllListeners()
    {
        EventManager.Instance.AddListener<NextRoomEvent>(OnTransitionStart);
        EventManager.Instance.AddListener<FinishedRoomTransition>(OnTransitionEnd);
    }

    private void OnTransitionStart()
    {
        isTransitioning = true;
    }

    private void OnTransitionEnd()
    {
        isTransitioning = false;
    }

    public void FocusNextRoom()
    {
        NextRoomEvent newInfo = new NextRoomEvent();
        //EventSystem.ExecuteEvent(EventType.UI_NEXT_ROOM, newInfo);
        EventManager.Instance.RaiseEvent(newInfo);
    }
    
    public void S()
    {
        
    }

    public void ProgressDialogue()
    {
        Debug.Log("Touch");
        if (SpeechBubble.Instance.DiagObject != null && isTransitioning == false)
        {
           
            Debug.Log("Field Index: " + SpeechBubble.Instance.DiagObject.Info.fieldIndex);
            
            //If we have looped back to the start after an iteration
            if (SpeechBubble.Instance.DiagObject.Info.fieldIndex == 0)
            {
                FillSpeechBubbleEvent repeatInfo = new FillSpeechBubbleEvent(null, Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, UIAnimator.BlurType.OUT, SpeechBubble.FillTextMethod.NONE, true);
                EventManager.Instance.RaiseEvent(repeatInfo);
            }
            else
            {
                FillSpeechBubbleEvent newInfo = new FillSpeechBubbleEvent(null, Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, UIAnimator.BlurType.NONE, SpeechBubble.FillTextMethod.ITERATE, false);
                EventManager.Instance.RaiseEvent(newInfo);
            
                //Debug.Log("Field Index: " + SpeechBubble.Instance.DiagObject.Info.fieldIndex);
            }
        }
       
    }

    public void PopUpWithParam(DialogueObject pDialogueObject)
    {
        CreateSpesifiedPopUpEvent newInfo = new CreateSpesifiedPopUpEvent(pDialogueObject);     
    }
    
}
