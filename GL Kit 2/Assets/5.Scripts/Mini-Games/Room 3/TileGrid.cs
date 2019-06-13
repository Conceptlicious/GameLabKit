using System;
using System.Collections.Generic;
using GameLab;
using UnityEngine;

namespace Room3
{
	public class TileGrid : Singleton<TileGrid>
	{
		public bool HasInteractedWithTile => lastInteractedWithTileController != null;

		// TODO: CLeanup current level management
		public Level CurrentLevel
		{
			get => levels[currentLevelIndex];
			set => currentLevelIndex = value != null ? Array.IndexOf(levels, value) : -1;
		}

		public Level.ColorSettings CurrentLevelSettings => CurrentLevel != null && CurrentLevel.HasCustomColorSettings ? CurrentLevel.CustomColorSettings : defaultLevelSettings;

		[SerializeField] private TileSpriteSettings tileSpriteSettings;

		[SerializeField] private TileController tileControllerPrefab = null;

		[SerializeField] private Level.ColorSettings defaultLevelSettings = Level.ColorSettings.Default;
		[SerializeField] private Level[] levels = new Level[0];

		private int currentLevelIndex = -1;

		private bool canBeInteractedWith = false;

		private TileLayer mainLayer = null;
		private TileLayer bridgeLayer = null;

		private TileController lastInteractedWithTileController = null;

		private HashSet<Tile.Group> finishedGroups = new HashSet<Tile.Group>();

		protected override void Awake()
		{
			base.Awake();

			if (levels.Length == 0)
			{
				Debug.LogWarning("There are no levels set up in Room 3!");
				return;
			}

			//SpawnLevel(levels[0]);
			NextLevel();
		}

		public void SetGridInteractable(bool canInteractedWith)
		{
			canBeInteractedWith = canInteractedWith;
		}

		private void NextLevel()
		{
			if (currentLevelIndex == levels.Length - 1)
			{
				print("No more levels");

				SaveItemEvent saveItemEvent = new SaveItemEvent(RoomType.Genre, this);
				EventManager.Instance.RaiseEvent(saveItemEvent);

				NextRoomEvent newInfo = new NextRoomEvent();
				EventManager.Instance.RaiseEvent(newInfo);
				return;
			}

			++currentLevelIndex;
			print(currentLevelIndex);
			SetGridInteractable(false);
			SpawnLevel(levels[currentLevelIndex]);
		}

		private void PreviousLevel()
		{
			if (currentLevelIndex == 0)
			{
				print("No previous levels");
			}

			--currentLevelIndex;

			SpawnLevel(levels[currentLevelIndex]);
		}

		private void SpawnLevel(Level level)
		{
			DestroySpawnedLevel();

			mainLayer = new TileLayer(level.Rows, level.Cols);
			bridgeLayer = new TileLayer(level.Rows, level.Cols, Tile.Type.Obstacle);

			Level.ColorSettings levelColorSettings = CurrentLevelSettings;
			Color32[] levelPixelData = level.LevelTexture.GetPixels32();

			float anchorStepPerColumn = 1.0f / level.Cols;
			float anchorStepPerRow = 1.0f / level.Rows;

			for (int i = 0; i < levelPixelData.Length; ++i)
			{
				Color32 pixel = levelPixelData[i];
				int row = i / level.Rows;
				int col = i % level.Cols;

				if (pixel.CompareRGB(levelColorSettings.BridgeTileColor))
				{
					mainLayer.Tiles[row, col].AllowedConnectionDirections = Tile.ConnectionDirection.East | Tile.ConnectionDirection.West;
					bridgeLayer.Tiles[row, col].AllowedConnectionDirections = Tile.ConnectionDirection.North | Tile.ConnectionDirection.South;
					bridgeLayer.Tiles[row, col].TileType = Tile.Type.Connection;
				}

				Tile tileData = mainLayer.Tiles[row, col];

				tileData.TileGroup = levelColorSettings.GetTileGroupFromColor(pixel);
				tileData.TileType = levelColorSettings.GetTileTypeFromColor(pixel);

				TileController tileController = SpawnTileController(row, col, anchorStepPerColumn, anchorStepPerRow);

				tileController.AddTilesToControl(mainLayer.Tiles[row, col], bridgeLayer.Tiles[row, col]);
				tileController.Image.color = pixel;

				tileController.Interacted += OnTileInteractedWith;
				tileController.OnFinishedInteractingAt += OnFinishedInteractingAtTile;
			}
		}

		private TileController SpawnTileController(int row, int col, float anchorStepPerColumn, float anchorStepPerRow)
		{
			TileController tileController = Instantiate(tileControllerPrefab, Vector3.zero, Quaternion.identity, CachedTransform);

			tileController.name = $"Tile {row}, {col}";

			tileController.CachedRectTransform.anchorMin = new Vector2(col * anchorStepPerColumn, row * anchorStepPerRow);
			tileController.CachedRectTransform.anchorMax = new Vector2((col + 1) * anchorStepPerColumn, (row + 1) * anchorStepPerRow);

			tileController.CachedRectTransform.offsetMin = tileController.CachedRectTransform.offsetMax = Vector3.zero;
			tileController.CachedRectTransform.localPosition = new Vector3(tileController.CachedRectTransform.localPosition.x, tileController.CachedRectTransform.localPosition.y, 0.0f);

			return tileController;
		}

		private void DestroySpawnedLevel()
		{
			foreach (Transform spawnedTile in CachedTransform)
			{
				Destroy(spawnedTile.gameObject);
			}

			//CurrentLevel = null;
			mainLayer = null;
			bridgeLayer = null;

			lastInteractedWithTileController = null;
			finishedGroups.Clear();
		}

		private void OnTileInteractedWith(TileController tileController)
		{
			if (!canBeInteractedWith)
			{
				return;
			}

			if (tileController == lastInteractedWithTileController)
			{
				return;
			}

			if (!HasInteractedWithTile)
			{
				TryResumePathFrom(tileController);
				return;
			}

			if (TryRemoveTileConnectionsAfter(tileController))
			{
				return;
			}

			if(IsTilePartOfFinishedGroups(tileController))
			{
				return;
			}

			if(!tileController.TryConnectTo(lastInteractedWithTileController))
			{
				return;
			}

			ValidatePath(tileController, lastInteractedWithTileController);

			if (tileController.Row == lastInteractedWithTileController.Row)
			{
				tileController.ChangeSprite(tileSpriteSettings.TubeWestToEast);
			}
			else
			{
				tileController.ChangeSprite(tileSpriteSettings.TubeNorthToSouth);
			}

			lastInteractedWithTileController = tileController;

			UpdateWinStatus();
		}
		
		private bool IsTilePartOfFinishedGroups(TileController tileController)
		{
			foreach(Tile controlledTile in tileController.ControlledTiles)
			{
				if(controlledTile.TileType == Tile.Type.Obstacle)
				{
					continue;
				}

				if(!finishedGroups.Contains(controlledTile.TileGroup))
				{
					return false;
				}
			}

			return true;
		}

		private void ValidatePath(TileController currentTileController, TileController lastTileController)
		{
			if(lastTileController.TileGroup == Tile.Group.Ungrouped)
			{
				return;
			}

			TilePath path = mainLayer.CalculatePathForGroup(lastTileController.TileGroup);
			
			if (!path.Tiles.Contains(currentTileController.ConnectedControlledTile))
			{
				return;
			}

			int lastTileControllerConnectedTileIndexInPath = path.Tiles.IndexOf(lastTileController.ConnectedControlledTile);

			// If the tile was not found or is the very first tile in the path, there is nothing before that, so we cannot do corner logic.
			if(lastTileControllerConnectedTileIndexInPath <= 0)
			{
				return;
			}

			Tile previousToLastTile = path.Tiles[lastTileControllerConnectedTileIndexInPath - 1];

			CheckForCorners(currentTileController, lastTileController, previousToLastTile);
		}

		private void CheckForCorners(TileController currentTileController, TileController lastTileController, Tile previousToLastTile)
		{
			Tile currentTileTileData = currentTileController.ConnectedControlledTile;
			Tile lastTileTileData = lastTileController.ConnectedControlledTile;

			// current ptl have both different x and y values 
			bool currentXSmallerThanPTL = (currentTileTileData.Col < previousToLastTile.Col);
			bool currentYSmallerThanPTL = (currentTileTileData.Row < previousToLastTile.Row);

			bool currentOnSameRowAsL = (currentTileTileData.Row == lastTileTileData.Row);

			if (lastTileTileData.Row == currentTileTileData.Row && lastTileTileData.Row == previousToLastTile.Row && lastTileTileData.Col != currentTileTileData.Row)
			{
				// there is an X difference but no Y difference
				lastTileController.ChangeSprite(tileSpriteSettings.TubeWestToEast);
				currentTileController.ChangeSprite(tileSpriteSettings.TubeWestToEast);
				return;
			}
			if (lastTileTileData.Col == currentTileTileData.Col && lastTileTileData.Col == previousToLastTile.Col && lastTileTileData.Row != currentTileTileData.Row)
			{
				// there is an X difference but no Y difference
				lastTileController.ChangeSprite(tileSpriteSettings.TubeNorthToSouth);
				currentTileController.ChangeSprite(tileSpriteSettings.TubeNorthToSouth);
				return;
			}

			if (currentTileTileData.Col != previousToLastTile.Col && currentTileTileData.Row != previousToLastTile.Row)
			{
				if (currentXSmallerThanPTL && currentYSmallerThanPTL)
				{
					if (currentOnSameRowAsL)
					{
						lastTileController.ChangeSprite(tileSpriteSettings.TubeWestToNorth);
					}
					else
					{
						lastTileController.ChangeSprite(tileSpriteSettings.TubeEastToSouth);
					}
					return;
				}

				if (currentXSmallerThanPTL && !currentYSmallerThanPTL)
				{
					if (currentOnSameRowAsL)
					{
						lastTileController.ChangeSprite(tileSpriteSettings.TubeWestToSouth);
					}
					else
					{
						lastTileController.ChangeSprite(tileSpriteSettings.TubeEastToNorth);
					}
					return;
				}

				if (!currentXSmallerThanPTL && currentYSmallerThanPTL)
				{
					if (currentOnSameRowAsL)
					{
						lastTileController.ChangeSprite(tileSpriteSettings.TubeEastToNorth);
					}
					else
					{
						lastTileController.ChangeSprite(tileSpriteSettings.TubeWestToSouth);
					}
					return;
				}

				if (!currentXSmallerThanPTL && !currentYSmallerThanPTL)
				{
					if (currentOnSameRowAsL)
					{
						lastTileController.ChangeSprite(tileSpriteSettings.TubeEastToSouth);
					}
					else
					{
						lastTileController.ChangeSprite(tileSpriteSettings.TubeWestToNorth);
					}
					return;
				}
			}
		}

		private void OnFinishedInteractingAtTile(TileController tile)
		{
			lastInteractedWithTileController = null;
		}

		private bool TryResumePathFrom(TileController tile)
		{
			if (HasInteractedWithTile)
			{
				return false;
			}

			if (tile.TileGroup == Tile.Group.Ungrouped)
			{
				return false;
			}

			if (!tile.IsMainTileSet || tile.MainTile.TileType == Tile.Type.EndPoint)
			{
				return false;
			}

			RemoveTileConnectionsAfter(tile);
			return true;
		}

		private bool TryRemoveTileConnectionsAfter(TileController tile)
		{
			if (tile.TileGroup == Tile.Group.Ungrouped)
			{
				return false;
			}

			TilePath interactedTileGroupPath = mainLayer.CalculatePathForGroup(tile.TileGroup);

			int interactedTilePathIndex = interactedTileGroupPath.Tiles.IndexOf(tile.ConnectedControlledTile);
			int lastInteractedWithTilePathIndex = interactedTileGroupPath.Tiles.IndexOf(lastInteractedWithTileController.ConnectedControlledTile);

			if (interactedTilePathIndex < 0 || interactedTilePathIndex >= lastInteractedWithTilePathIndex)
			{
				return false;
			}

			RemoveTileConnectionsAfter(tile);

			return true;
		}

		private void RemoveTileConnectionsAfter(TileController tile)
		{
			tile.ConnectedControlledTile.RemoveTileConnectionsAfterThis();
			finishedGroups.Remove(tile.TileGroup);

			lastInteractedWithTileController = tile;
		}

		private void UpdateWinStatus()
		{
			Tile tileData = lastInteractedWithTileController.ConnectedControlledTile;

			if (tileData.TileType != Tile.Type.EndPoint)
			{
				return;
			}

			finishedGroups.Add(tileData.TileGroup);

			foreach (Tile.Group tileGroup in CurrentLevelSettings.TileGroups)
			{
				if (!finishedGroups.Contains(tileGroup))
				{
					return;
				}
			}

			NextLevel();
			Debug.Log("Level complete!");
		}
	}
}