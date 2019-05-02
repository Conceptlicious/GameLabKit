using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab
{
	public abstract class GameLabEvent
	{
		public bool Consumed { get; private set; }

		public void Consume()
		{
			Consumed = true;
		}
	}
}