using System;
using UnityEngine;

namespace GameLab
{
	/// <summary>
	/// Attribute to select a single layer from a dropdown menu to set for game objects instead of typing a number manually.
	/// The attribute can only be used with int values.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LayerAttribute : PropertyAttribute { }
}
