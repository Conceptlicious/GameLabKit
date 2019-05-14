using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameLab;
using System.IO;

public class TileGrid : BetterMonoBehaviour
{

	[SerializeField] private GridMapStruct[] grids;
	[SerializeField] private GameObject tilePrefab;
	private Tile lastTile = null;
	private List<TilePath> paths = new List<TilePath>();
	private TileLayer bridgeLayer = null;
	private TileLayer mainLayer = null;

	protected override void Awake()
	{
		base.Awake();
		CreateGrid(0);
	}

	private void CreateGrid(int gridIndex)
	{
		Texture2D map = grids[gridIndex].Map;
		Color32[] pixels = map.GetPixels32();

		mainLayer = new TileLayer(map.height, map.width);
		bridgeLayer = new TileLayer(map.height, map.width, Tile.Type.Obstacle);


		for(int pixelIndex = 0; pixelIndex < pixels.Length; ++pixelIndex)
		{
			TileLayer tileLayer = null;

			if (pixels[pixelIndex].Compare(grids[gridIndex].BridgeColor))
			{
				tileLayer = bridgeLayer;
			}
			else
			{
				tileLayer = mainLayer;
			}
			int row = pixelIndex / map.width;
			int col = pixelIndex % map.width;

			tileLayer.Tiles[row, col].TileGroup = grids[gridIndex].GetGroupFromColor(pixels[pixelIndex]);
			tileLayer.Tiles[row, col].TileType = grids[gridIndex].GetTypeFromColor(pixels[pixelIndex]);

			GameObject tileInstance = Instantiate(tilePrefab, new Vector3(0,0,0), Quaternion.identity, this.transform);

			TileController tileControllerInstance = tileInstance.GetComponent<TileController>();
			tileControllerInstance.TileData = tileLayer.Tiles[row, col];
			tileControllerInstance.OnMouseHovered += OnTileHovered;
			RectTransform rectTransformInstance = tileInstance.GetComponent<RectTransform>();

			rectTransformInstance.anchorMin =
				new Vector2((col * (1.0f / map.width)), 
				row * (1.0f / map.height));
			rectTransformInstance.anchorMax =
				new Vector2(((col + 1) * (1.0f / map.width)), 
				(row + 1) * (1.0f / map.height));
			rectTransformInstance.offsetMin = rectTransformInstance.offsetMax = Vector2.zero;

			Image tileImageInstance = tileInstance.GetComponent<Image>();
			tileImageInstance.color = pixels[pixelIndex];

			if (tileControllerInstance.TileData.TileGroup != null)
			{
				//tileControllerInstance.TileData.TileGroup.UngrouppedColor = grids[gridIndex].Accessible;
			}
		}
	}

	private void OnTileHovered(TileController tile)
	{
		if(tile.TileData == null)
		{
			return;
		}

		if(lastTile == null && tile.TileData.TileType == Tile.Type.StartPoint)
		{
			lastTile = tile.TileData;
			return;
		}

		if (tile.TileData.TileGroup != null && tile.TileData.TileGroup == lastTile.TileGroup)
		{
			tile.TileData.RemoveTileConnectionsAfterThis();
			return;
		}

		if(!tile.TileData.TryConnectTo(lastTile))
		{
			return;
		}
		
		lastTile = tile.TileData;
	}
}

public static class Color32Extensions
{
	public static bool Compare(this Color32 thisColor, Color32 other)
	{
		return thisColor.r == other.r && thisColor.g == other.g && thisColor.b == other.b && thisColor.a == other.a;
	}

	public static bool CompareIgnoreAlpha(this Color32 thisColor, Color32 other)
	{
		return thisColor.r == other.r && thisColor.g == other.g && thisColor.b == other.b;
	}
}
