using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

//--------------------------------------------------
//Produced by: Josh van Asten
//Overview: This script handles the progression of dialogue displayed by Dr GameLab. It mostly listens to and broadcasts
//events from other scripts.
//Usage: Used with UI and dialogue systems.
//--------------------------------------------------


public class SpeechManager : BetterMonoBehaviour
{
	[SerializeField] private DialogueObject[] dialogueObjects;
	[SerializeField] private GameObject dialogueObjectParent;
	[SerializeField] private bool displaySpeechBubbles = true;
	
	void Awake()
	{
		RegisterAllListeners();
	}
	
	/// <summary>
	/// Registers all event listeners this class needs to care about.
	/// </summary>
	private void RegisterAllListeners()
	{
		EventManager.Instance.AddListener<FinishedRoomTransition>(OnFinishRoomTransition);
		EventManager.Instance.AddListener<DismissSpeechBubbleEvent>(OnDismissCall);
	}

	
	//Display after transition
	private void OnFinishRoomTransition(FinishedRoomTransition info)
	{
		if (displaySpeechBubbles)
		{
			Debug.Log("Just focused Room " + RoomManager.Instance.GetRoomIDs().z);
			
			FillSpeechBubbleEvent newInfo = new FillSpeechBubbleEvent(dialogueObjects[RoomManager.Instance.GetRoomIDs().z], Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, UIAnimator.BlurType.IN, SpeechBubble.FillTextMethod.ITERATE, true);
			EventManager.Instance.RaiseEvent(newInfo);
		}
		
	}

	//Dismiss the UI
	private void OnDismissCall()
	{
		FillSpeechBubbleEvent info = new FillSpeechBubbleEvent(null, Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, UIAnimator.BlurType.OUT, SpeechBubble.FillTextMethod.NONE, true);
		EventManager.Instance.RaiseEvent(info);
	}

	private void FillWithBlanks()
	{
		GameObject[] oldBlanks = GameObject.FindGameObjectsWithTag("Dialogue: " + Settings.OBJ_NAME_BLANK_GAMEOBJECT);

		for (int i = 0; i < oldBlanks.Length; i++)
		{
			//GameObject.Destroy(oldBlanks[i]);
		}
		for (int i = 0; i < dialogueObjects.Length; i++)
		{        
			//Debug.Log("Room focal " + i + ": " + roomFocalPoints[i].position);
			if (dialogueObjects[i] == null)
			{
               
				Debug.Log("Filling " + i + " with a blank GO.");
				string path =
					Settings.PATH_PREFABS + Settings.OBJ_NAME_BLANK_GAMEOBJECT;
				Debug.Log(path);
				GameObject blankGameObject = GameObject.Instantiate(Resources.Load<GameObject>(path), dialogueObjectParent.transform);
				blankGameObject.tag = "Dialogue: " + Settings.OBJ_NAME_BLANK_GAMEOBJECT;				
				DialogueObject diagObj = blankGameObject.AddComponent<DialogueObject>();
				dialogueObjects[i] = diagObj;
			}
               
		}
	}

	
	void OnValidate()
	{
		int oldLength = dialogueObjects.Length;
		if (oldLength != Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS)
		{
            
			DialogueObject[] temp = dialogueObjects;
			dialogueObjects = new DialogueObject[Settings.SYS_VAL_MAX_NUMBER_ROOM_FOCALS];
			for (int i = 0; i < oldLength; i++)
			{
				dialogueObjects[i] = temp[i];
			}

			FillWithBlanks();
            
		}
	}
}
