using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLab;

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
	}

	private void OnFinishRoomTransition(FinishedRoomTransition info)
	{
		if (displaySpeechBubbles)
		{
			Debug.Log("Just focused Room " + RoomManager.Instance.GetRoomIDs().z);
			FillSpeechBubbleEvent newInfo = new FillSpeechBubbleEvent(dialogueObjects[RoomManager.Instance.GetRoomIDs().z], Settings.VAL_SPEECH_BUBBLE_TRANSITION_SECONDS, UIAnimator.MoveType.TRANSITION, SpeechBubble.FillTextMethod.ITERATE, true);
			EventManager.Instance.RaiseEvent(newInfo);
		}
		
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
