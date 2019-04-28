using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLab
{
	public abstract class EventHandler
	{
		public int PriorityOrder = 0;

		protected Delegate callback = null;

		public EventHandler(Delegate callback, int priorityOrder)
		{
			this.callback = callback;

			PriorityOrder = priorityOrder;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(EventHandler handler, Delegate callback)
		{
			return handler.callback == callback;
		}

		public static bool operator !=(EventHandler handler, Delegate callback)
		{
			return !(handler == callback);
		}
	}

	public class EventHandler<TEvent> : EventHandler where TEvent : Event
	{
		public EventHandler(Action<TEvent> callback, int priorityOrder) : base(callback, priorityOrder) {}

		public void Invoke(TEvent eventToRaise)
		{
			(callback as Action<TEvent>)?.Invoke(eventToRaise);
		}
	}
}
