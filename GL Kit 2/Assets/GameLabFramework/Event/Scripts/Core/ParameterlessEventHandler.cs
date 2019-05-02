using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLab
{
	public class ParameterlessEventHandler<TEvent> : EventHandler where TEvent : GameLabEvent
	{
		public ParameterlessEventHandler(Action callback, int priorityOrder) : base(callback, priorityOrder) { }

		public void Invoke()
		{
			(callback as Action)?.Invoke();
		}
	}
}
