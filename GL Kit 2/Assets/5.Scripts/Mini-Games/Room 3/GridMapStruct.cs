using System;
using UnityEngine;

[Serializable]
public struct GridMapStruct
{
	[Tooltip("The grid itself, make sure you that the start points and endpoints are the same colour with different alphas and that the colours match the colours in the list below")]
	public Texture2D Map;
	[Tooltip("The colours of your tubes make sure that these are the same as the colours in your gridmap")]
	public Tile.Group[] Groups;
	public Color32 Inaccessible;
	public Color32 Accessible;
	public Color32 BridgeColor;
	[Tooltip("Maximum Alpha necessary before it becomes an end point"), Range(0, 255)]
	public byte AlphaCutoff;
}


