using System.Collections;
using UnityEngine;

namespace GameLab
{
	/// <summary>
	/// Singleton manager behavior class, used for components that are managers and should only have one instance.
	/// <remarks>Manager classes live on through scene transitions and will mark their parent root GameObject with <see cref="Object.DontDestroyOnLoad"/></remarks>
	/// </summary>
	/// <typeparam name="T">The Manager Type</typeparam>
	public abstract partial class Manager<T> : Singleton<T>, IManager where T : Singleton<T> {}
}