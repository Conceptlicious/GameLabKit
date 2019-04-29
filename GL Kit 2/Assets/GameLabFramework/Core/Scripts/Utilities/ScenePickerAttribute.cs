using System;
using UnityEngine;

namespace GameLab
{
	/// <summary>
	/// Attribute to select a scene by dragging it into an object field.
	/// The attribute can only be used on string variables.
	/// This does not hold a reference to a scene, so if a scene gets renamed, it will need to be reselected.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class ScenePickerAttribute : PropertyAttribute { }
}
