using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace GameLab
{
	[DisallowMultipleComponent]
	public class GameManager : Manager<GameManager>
	{
		[SerializeField] private Camera mainCamera = null;

		public Player Player { get; private set; }

		public Camera MainCamera { get; private set; }
		public Transform MainCameraCachedTransform { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			Player = FindObjectOfType<Player>();

			MainCamera = mainCamera != null ? mainCamera : Camera.main;
			MainCameraCachedTransform = MainCamera?.transform;
		}
	}
}