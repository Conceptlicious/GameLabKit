using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLab;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
public class Flask : BetterMonoBehaviour
{
	public static Sprite FlaskSprite { get; private set; }
	[SerializeField] private Image completeButtons;
	private Image flask;
	private Animator animator = null;
	[SerializeField, Tooltip("Distance is equivelant to what X axis point you want it to move to")] private float travelDistance = 0f;
	[SerializeField, Tooltip("Duration of the flask moving in seconds")] private float travelSpeed = 0f;
	public Action AnimationStart;

	private void Start()
	{
		animator = GetComponent<Animator>();
		animator.SetBool("PlayAnimation", false);
		flask = GetComponent<Image>();
		AnimationStart += StartAnimations;
	}

	private void StartAnimations()
	{
		flask.enabled = true;
		animator.SetBool("PlayAnimation", true);
	}

	public void OnAnimationFinish()
	{
		Debug.Log("Animation Finished");
		LeanTween.moveLocalX(gameObject, travelDistance, travelSpeed);
	}

	public void OnFlaskSelected()
	{
		//Set the trophy to this
		FlaskSprite = GetComponent<Image>().sprite;

		completeButtons.gameObject.SetActive(true);
		//EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
		//EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Genre));
		//EventManager.Instance.RaiseEvent(new NextRoomEvent());
	}

	public void YesButton()
	{
		EventManager.Instance.RaiseEvent(new SaveItemEvent(RoomType.Genre));
		NextRoomEvent nextRoomEvent = new NextRoomEvent();
		completeButtons.gameObject.SetActive(false);
		EventManager.Instance.RaiseEvent(nextRoomEvent);
	}

	public void NoButton()
	{
		completeButtons.gameObject.SetActive(false);
	}
}
