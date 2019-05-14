using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GridMapStruct
{
	#region Tooltip Map
	[Tooltip
	("The grid itself, make sure you that the start points and endpoints are the same colour with different alphas " +
	"and that the colours match the colours in the list below")]
	#endregion
	public Texture2D Map;
	#region Tooltip Colours
	[Tooltip("The colours of your tubes make sure that these are the same as the colours in your gridmap")]
	#endregion
	public Tile.Group[] Groups;
	public Color32 Inaccessible;
	public Color32 Accessible;
	public Color32 BridgeColor;
	#region Tooltip Alpha Cut of
	[Tooltip("Maximum Alpha necessary before it becomes an end point"), Range(0, 255)]
	#endregion
	public byte AlphaCutoff;

	public Tile.Group GetGroupFromColor(Color32 color)
	{
		foreach(Tile.Group colorGroup in Groups)
		{
			if(!colorGroup.GroupColor.CompareIgnoreAlpha(color))
			{
				continue;
			}

			return colorGroup;
		}

		return Tile.Group.Ungrouped;
	}

	public Tile.Type GetTypeFromColor(Color32 color)
	{
		if (Accessible.Compare(color))
		{
			return Tile.Type.Connection;
		}
		if (Inaccessible.Compare(color))
		{
			return Tile.Type.Obstacle;
		}
		foreach(Tile.Group colorGroup in Groups)
		{
			if (colorGroup.GroupColor.CompareIgnoreAlpha(color))
			{
				if(color.a < AlphaCutoff)
				{
					return Tile.Type.EndPoint;
				}else
				{
					return Tile.Type.StartPoint;
				}
			}
		}
		return Tile.Type.Obstacle;
	}
}


