using System;
using System.Collections.Generic;
using GameLab;
using UnityEngine;

public class TileGrid : Singleton<TileGrid>
{
	public bool HasInteractedWithTile => lastInteractedWithTile != null;

	// TODO: CLeanup current level management
	public LevelData CurrentLevel
	{
		get => levels[currentLevelIndex];
		set => currentLevelIndex = value != null ? Array.IndexOf(levels, value) : -1;
	}

	public LevelData.ColorSettings CurrentLevelSettings => CurrentLevel != null && CurrentLevel.HasCustomColorSettings ? CurrentLevel.CustomColorSettings : defaultLevelSettings;

	[SerializeField] private TileController tileControllerPrefab = null;

	[SerializeField] private LevelData.ColorSettings defaultLevelSettings = LevelData.ColorSettings.Default;
	[SerializeField] private LevelData[] levels = new LevelData[0];

	private int currentLevelIndex = -1;

	private TileLayer mainLayer		= null;
	private TileLayer bridgeLayer	= null;

	private TileController lastInteractedWithTile = null;

	private HashSet<Tile.Group> finishedGroups = new HashSet<Tile.Group>();

	protected override void Awake()
	{
		base.Awake();

		if(levels.Length == 0)
		{
			Debug.LogWarning("There are no levels set up in Room 3!");
			return;
		}

		//SpawnLevel(levels[0]);
		NextLevel();
	}

	private void NextLevel()
	{
		if(currentLevelIndex == levels.Length - 1)
		{
			print("No more levels");
			return;
		}

		++currentLevelIndex;
		print(currentLevelIndex);
		SpawnLevel(levels[currentLevelIndex]);
	}

	private void PreviousLevel()
	{
		if(currentLevelIndex == 0)
		{
			print("No previous levels");
		}

		--currentLevelIndex;

		SpawnLevel(levels[currentLevelIndex]);
	}

	private void SpawnLevel(LevelData level)
	{
		DestroySpawnedLevel();

		mainLayer = new TileLayer(level.Rows, level.Cols);
		bridgeLayer = new TileLayer(level.Rows, level.Cols, Tile.Type.Obstacle);

		LevelData.ColorSettings levelColorSettings = CurrentLevelSettings;
		Color32[] levelPixelData = level.LevelTexture.GetPixels32();

		float anchorStepPerColumn = 1.0f / level.Cols;
		float anchorStepPerRow = 1.0f / level.Rows;

		for(int i = 0; i < levelPixelData.Length; ++i)
		{
			Color32 pixel = levelPixelData[i];
			int row = i / level.Rows;
			int col = i % level.Cols;

			TileLayer tileLayer = pixel.CompareRGB(levelColorSettings.BridgeTileColor) ? bridgeLayer : mainLayer;

			Tile tileData = tileLayer.Tiles[row, col];

			tileData.TileGroup = levelColorSettings.GetTileGroupFromColor(pixel);
			tileData.TileType = levelColorSettings.GetTileTypeFromColor(pixel);

			TileController tileController = SpawnTileController(tileData, anchorStepPerColumn, anchorStepPerRow);
			tileController.Image.color = pixel;
		}
	}

	private TileController SpawnTileController(Tile tileData, float anchorStepPerColumn, float anchorStepPerRow)
	{
		TileController tileController = Instantiate(tileControllerPrefab, Vector3.zero, Quaternion.identity, CachedTransform);
		tileController.name = $"Tile {tileData.Row}, {tileData.Col}";

		tileController.TileData = tileData;
		tileController.OnInteractedWith += OnTileInteractedWith;
		tileController.OnFinishedInteractingAt += OnFinishedInteractingAtTile;

		tileController.CachedRectTransform.anchorMin = new Vector2(tileData.Col * anchorStepPerColumn, tileData.Row * anchorStepPerRow);
		tileController.CachedRectTransform.anchorMax = new Vector2((tileData.Col + 1) * anchorStepPerColumn, (tileData.Row + 1) * anchorStepPerRow);

		tileController.CachedRectTransform.offsetMin = tileController.CachedRectTransform.offsetMax = Vector2.zero;

		return tileController;
	}

	private void DestroySpawnedLevel()
	{
		foreach(Transform spawnedTile in CachedTransform)
		{
			Destroy(spawnedTile.gameObject);
		}

		//CurrentLevel = null;
		mainLayer = null;
		bridgeLayer = null;

		lastInteractedWithTile = null;
		finishedGroups.Clear();
	}

	private void OnTileInteractedWith(TileController tile)
	{
		if(tile == lastInteractedWithTile)
		{
			return;
		}

		Tile tileData = tile.TileData;

		if(!HasInteractedWithTile)
		{
			TryResumePathFrom(tile);
			return;
		}

		if(TryRemoveTileConnectionsAfter(tile))
		{
			return;
		}

		if(finishedGroups.Contains(lastInteractedWithTile.TileData.TileGroup))
		{
			return;
		}

		if(!tileData.TryConnectTo(lastInteractedWithTile.TileData))
		{
			return;
		}

		lastInteractedWithTile = tile;

		UpdateWinStatus();
	}

	private void OnFinishedInteractingAtTile(TileController tile)
	{
		lastInteractedWithTile = null;
	}

	private bool TryResumePathFrom(TileController tile)
	{
		if(HasInteractedWithTile)
		{
			return false;
		}

		if(tile.TileData.TileGroup == Tile.Group.Ungrouped)
		{
			return false;
		}

		if(tile.TileData.TileType == Tile.Type.EndPoint)
		{
			return false;
		}

		RemoveTileConnectionsAfter(tile);

		return true;
	}

	private bool TryRemoveTileConnectionsAfter(TileController tile)
	{
		if(tile.TileData.TileGroup == Tile.Group.Ungrouped)
		{
			return false;
		}

		TilePath interactedTileGroupPath = mainLayer.CalculatePathForGroup(tile.TileData.TileGroup);

		int interactedTilePathIndex = interactedTileGroupPath.Tiles.IndexOf(tile.TileData);
		int lastInteractedWithTilePathIndex = interactedTileGroupPath.Tiles.IndexOf(lastInteractedWithTile.TileData);

		if(interactedTilePathIndex < 0 || interactedTilePathIndex >= lastInteractedWithTilePathIndex)
		{
			return false;
		}

		RemoveTileConnectionsAfter(tile);

		return true;
	}

	private void RemoveTileConnectionsAfter(TileController tile)
	{
		tile.TileData.RemoveTileConnectionsAfterThis();
		finishedGroups.Remove(tile.TileData.TileGroup);

		lastInteractedWithTile = tile;
	}

	private void UpdateWinStatus()
	{
		Tile tileData = lastInteractedWithTile.TileData;

		if(tileData.TileType != Tile.Type.EndPoint)
		{
			return;
		}

		finishedGroups.Add(tileData.TileGroup);

		foreach(Tile.Group tileGroup in CurrentLevelSettings.TileGroups)
		{
			if(!finishedGroups.Contains(tileGroup))
			{
				return;
			}
		}

		NextLevel();
		Debug.Log("Level complete!");
	}
}
