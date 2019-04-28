using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
	/// <summary>
	/// Returns the <b>world space</b> axis-aligned bounds encompassing the entire object, including all of its children.
	/// </summary>
	/// <param name="component">The component whose gameobject to encapsulate</param>
	/// <param name="includeInactiveObjects">Whether inactive children are included in the bounds calculation</param>
	/// <param name="renderersToExclude">List of renderers to exclude from the bounds calculations</param>
	/// <returns>Axis-Aligned world space bounds encompassing the object and its children</returns>
	public static Bounds CalculateBounds(this Component component, bool includeInactiveObjects, params Renderer[] renderersToExclude)
	{
		return component.gameObject.CalculateBounds(includeInactiveObjects, renderersToExclude);
	}

	/// <summary>
	/// Returns the <b>world space</b> axis-aligned bounds encompassing the entire object, including all of its children.
	/// </summary>
	/// <param name="gameObject">The gameobject to encapsulate</param>
	/// <param name="includeInactiveObjects">Whether inactive children are included in the bounds calculation</param>
	/// <param name="renderersToExclude">List of renderers to exclude from the bounds calculations</param>
	/// <returns>Axis-Aligned world space bounds encompassing the object and its children</returns>
	public static Bounds CalculateBounds (this GameObject gameObject, bool includeInactiveObjects, params Renderer[] renderersToExclude)
	{
		Bounds bounds = new Bounds();
		Renderer[] objectRenderers = gameObject.GetComponentsInChildren<Renderer>();

		bool initializedBounds = false;

		for(int i = 0; i < objectRenderers.Length; ++i)
		{
			Renderer renderer = objectRenderers[i];
			
			if(!includeInactiveObjects && renderer.gameObject.activeInHierarchy == false)
			{
				continue;
			}

			if(renderersToExclude != null && renderersToExclude.Contains(renderer))
			{
				continue;
			}

			if(!initializedBounds)
			{
				bounds = renderer.bounds;
				initializedBounds = true;
			}
			else
			{
				bounds.Encapsulate(renderer.bounds);
			}
		}

		return bounds;
	}

	/// <summary>
	/// Ensure that a component of type <typeparamref name="T"/> exists on the game object.
	/// If it doesn't exist, the method creates it.
	/// </summary>
	/// <typeparam name="T">Type of the component.</typeparam>
	/// <param name="gameObject">Game object on which component should be.</param>
	/// <returns>The component that was retrieved or created.</returns>
	public static T EnsureComponent<T>(this GameObject gameObject) where T : Component
	{
		T foundComponent = gameObject.GetComponent<T>();

		if(foundComponent != null)
		{
			return foundComponent;
		}

		return gameObject.AddComponent<T>();
	}

	/// <summary>
	/// Ensure that a component of type <typeparamref name="T"/> exists on the game object.
	/// If it doesn't exist, the method creates it.
	/// </summary>
	/// <typeparam name="T">Type of the component.</typeparam>
	/// <param name="component">A component on the game object for which a component of type <typeparamref name="T"/> should exist.</param>
	/// <returns>The component that was retrieved or created.</returns>
	public static T EnsureComponent<T>(this Component component) where T : Component
	{
		return EnsureComponent<T>(component.gameObject);
	}
}
