using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventCallbacks;
using EventType = CustomEventCallbacks.EventType;
using GameLab;

public class HiddenObject : BetterMonoBehaviour
{
	[SerializeField]
	private Settings.R2_ObjectsToFind objectsToFind;

	public void CheckObject(ObjectFoundEvent objectFoundEventInfo)
	{
		bool correctObject = objectsToFind != Settings.R2_ObjectsToFind.INCORRECT ? true : false;
		ObjectFoundEvent info = new ObjectFoundEvent( correctObject, objectsToFind);
		//EventSystem.ExecuteEvent(EventType.R2_OBJECT_FOUND, info);
		EventManager.Instance.RaiseEvent(info);
	}
}
