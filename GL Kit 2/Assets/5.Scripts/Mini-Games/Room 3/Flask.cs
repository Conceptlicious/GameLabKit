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
	private const string GENRE_VARIABLE = "genre";
	private const string KNOT_NAME = "Part9";

	private Image flask;
	private Animator animator = null;
	[SerializeField, Tooltip("Distance is equivalent to what X axis point you want it to move to")] private float travelDistance = 0f;
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
		LeanTween.moveLocalX(gameObject, travelDistance, travelSpeed);
	}

	public void OnFlaskSelected()
	{
		FlaskSprite = GetComponent<Image>().sprite;

		DialogueManager.Instance.CurrentDialogue.Reset(KNOT_NAME);
		DialogueManager.Instance.CurrentDialogue.SetStringVariable(GENRE_VARIABLE, $"\"{name}\"");
		MenuManager.Instance.OpenMenu<DialogueMenu>();
	}
}