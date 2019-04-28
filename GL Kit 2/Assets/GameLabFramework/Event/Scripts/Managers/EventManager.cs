using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameLab
{
	[DisallowMultipleComponent]
	public class EventManager : Manager<EventManager>
	{
		protected Dictionary<Type, List<EventHandler>> evenTypesToHandlers = new Dictionary<Type, List<EventHandler>>();

		/// <summary>
		/// Raises an event of type <typeparamref name="TEvent"/> with the <paramref name="eventToRaise"/> event information.
		/// </summary>
		/// <typeparam name="TEvent">The type of event to raise</typeparam>
		/// <param name="eventToRaise">The event information</param>
		public void RaiseEvent<TEvent>(TEvent eventToRaise) where TEvent : Event
		{
			List<EventHandler> handlers = GetEventHandlers<TEvent>();

			if(handlers == null || handlers.Count == 0)
			{
				return;
			}

			foreach(EventHandler handler in handlers)
			{
				if(eventToRaise.Consumed)
				{
					break;
				}

				if(handler is EventHandler<TEvent>)
				{
					(handler as EventHandler<TEvent>).Invoke(eventToRaise);
				}
				else if(handler is ParameterlessEventHandler<TEvent>)
				{
					(handler as ParameterlessEventHandler<TEvent>).Invoke();
				}
			}
		}

		/// <summary>
		/// Adds a <typeparamref name="TEvent"/> event listener that cares about the event information.
		/// </summary>
		/// <typeparam name="TEvent">The type of event to listen to</typeparam>
		/// <param name="listener">Delegate function that accepts the event type as a parameter that will be called when the event is raised</param>
		/// <param name="priorityOrder">The priority of the listener when the event is raised. Higher priority listeners get called first</param>
		public void AddListener<TEvent>(Action<TEvent> listener, int priorityOrder = 0) where TEvent : Event
		{
			EventHandler<TEvent> handler = new EventHandler<TEvent>(listener, priorityOrder);
			AddListenerInternal<TEvent>(handler);
		}

		/// <summary>
		/// Adds a <typeparamref name="TEvent"/> event listener that does not care about the event information.
		/// </summary>
		/// <typeparam name="TEvent">The type of event to listen to</typeparam>
		/// <param name="listener">Delegate function that takes no parameters that will be called when the event is raised</param>
		/// <param name="priorityOrder">The priority of the listener when the event is raised. Higher priority listeners get called first</param>
		public void AddListener<TEvent>(Action listener, int priorityOrder = 0) where TEvent : Event
		{
			ParameterlessEventHandler<TEvent> handler = new ParameterlessEventHandler<TEvent>(listener, priorityOrder);
			AddListenerInternal<TEvent>(handler);
		}

		/// <summary>
		/// Removes a <typeparamref name="TEvent"/> event listener.
		/// </summary>
		/// <typeparam name="TEvent">The type of event to stop listening to</typeparam>
		/// <param name="listener">The delegate function to remove</param>
		public void RemoveListener<TEvent>(Action<TEvent> listener) where TEvent : Event
		{
			RemoveListenerInternal<TEvent>(listener);
		}

		/// <summary>
		/// Removes a <typeparamref name="TEvent"/> event listener.
		/// </summary>
		/// <typeparam name="TEvent">The type of event to stop listening to</typeparam>
		/// <param name="listener">The delegate function to remove</param>
		public void RemoveListener<TEvent>(Action listener) where TEvent : Event
		{
			RemoveListenerInternal<TEvent>(listener);
		}

		private void AddListenerInternal<TEvent>(EventHandler handler) where TEvent : Event
		{
			if(!evenTypesToHandlers.ContainsKey(typeof(TEvent)))
			{
				evenTypesToHandlers.Add(typeof(TEvent), new List<EventHandler>());
			}

			List<EventHandler> handlers = evenTypesToHandlers[typeof(TEvent)];
			handlers.Add(handler);

			handlers.Sort((firstHandler, secondHandler) => secondHandler.PriorityOrder.CompareTo(firstHandler.PriorityOrder));
		}

		private void RemoveListenerInternal<TEvent>(Delegate listener) where TEvent : Event
		{
			List<EventHandler> handlers = GetEventHandlers<TEvent>();

			EventHandler handler = handlers.FirstOrDefault(eventHandler => eventHandler == listener);
			handlers.Remove(handler);
		}

		private List<EventHandler> GetEventHandlers<TEvent>() where TEvent : Event
		{
			if(!evenTypesToHandlers.TryGetValue(typeof(TEvent), out List<EventHandler> handlers))
			{
				return null;
			}

			return handlers;
		}
	}
}