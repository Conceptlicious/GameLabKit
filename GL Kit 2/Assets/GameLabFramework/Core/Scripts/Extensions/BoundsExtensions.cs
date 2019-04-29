using System;
using UnityEngine;

public static class BoundsExtensions
{
	public enum VertexToSnap
	{
		Center,
		TopRightFront,
		TopRightBack,
		BottomRightFront,
		BottomRightBack,
		TopLeftFront,
		TopLeftBack,
		BottomLeftFront,
		BottomLeftBack
	}

	/// <summary>
	/// Expands the size of the bounds to the next integer.
	/// </summary>
	/// <param name="bounds">The bounds to round up</param>
	public static void RoundUpSize(this ref Bounds bounds)
	{
		Vector3 size = bounds.size;
		size.x = CeilToInt(size.x);
		size.y = CeilToInt(size.y);
		size.z = CeilToInt(size.z);
		bounds.size = size;
	}

	/// <summary>
	/// Expands the size of the bounds to the next integer.
	/// </summary>
	/// <param name="bounds">The bounds to round up</param>
	public static void RoundUpSizeXZ(this ref Bounds bounds)
	{
		Vector3 size = bounds.size;
		size.x = CeilToInt(size.x);
		size.z = CeilToInt(size.z);
		bounds.size = size;
	}

	public static void SnapToPosition(this ref Bounds bounds, Vector3 position, VertexToSnap vertexToSnap = VertexToSnap.Center)
	{
		Vector3 vertexToSnapPosition = VertexToSnapToPosition(ref bounds, vertexToSnap);
		Vector3 snapMovementDelta = position - vertexToSnapPosition;
		bounds.center += snapMovementDelta;
	}

	public static void SnapToBounds(this ref Bounds bounds, VertexToSnap vertexToSnap, ref Bounds boundsToSnapTo, VertexToSnap vertexToSnapTo)
	{
		Vector3 vertexToSnapToPosition = VertexToSnapToPosition(ref boundsToSnapTo, vertexToSnapTo);
		bounds.SnapToPosition(vertexToSnapToPosition, vertexToSnap);
	}

	private static Vector3 VertexToSnapToPosition(ref Bounds bounds, VertexToSnap vertexToSnap)
	{
		switch(vertexToSnap)
		{
			case VertexToSnap.Center:			return bounds.center;
			case VertexToSnap.TopRightFront:	return bounds.center + bounds.extents;
			case VertexToSnap.TopRightBack:		return bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
			case VertexToSnap.BottomRightFront: return bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
			case VertexToSnap.BottomRightBack:	return bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
			case VertexToSnap.TopLeftFront:		return bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
			case VertexToSnap.TopLeftBack:		return bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
			case VertexToSnap.BottomLeftFront:	return bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
			case VertexToSnap.BottomLeftBack:	return bounds.center - bounds.extents;
			default:							return bounds.center;
		}
	}

	private static int CeilToInt(float number, float threshold = 0.2f)
	{
		if(number - (int)number < threshold)
		{
			return (int)number;
		}

		return Mathf.CeilToInt(number);
	}
}