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

		[SerializeField] private TileSpriteSettings[] tileSpriteSettings;

		[SerializeField] private TileController tileControllerPrefab = null;

		[SerializeField] private Level.ColorSettings defaultLevelSettings = Level.ColorSettings.Default;
		[SerializeField] private Level[] levels = new Level[0];

		[SerializeField] private Lever lever;

		[SerializeField] private GameObject flasks;

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

			EventManager.Instance.AddListener<FinishedRoomTransition>(OnFinishedTransition);

			//SpawnLevel(levels[0]);
			NextLevel();
		}
		
		private void OnFinishedTransition(FinishedRoomTransition eventData)
		{
			int roomId = RoomManager.Instance.GetCurrentRoomID().z;

			if (roomId == 3)
			{
				DialogueManager.Instance.SetCurrentDialogue(RoomType.Genre);
				MenuManager.Instance.OpenMenu<DialogueMenu>();
			}

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
				DestroySpawnedLevel();
				//EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
				flasks.SetActive(true);
				return;
			}
		  //	EventManager.Instance.RaiseEvent(new ProgressDialogueEvent());
			++currentLevelIndex;
			//print(currentLevelIndex);
			SetGridInteractable(false);
			lever.SwitchSprites();
			SpawnLevel(levels[currentLevelIndex]);
		}

		private void PreviousLevel()
		{
			//set the level to the previous level
			if (currentLevelIndex == 0)
			{
				print("No previous levels");
			}

			--currentLevelIndex;

			SpawnLevel(levels[currentLevelIndex]);
		}

		private void SpawnLevel(Level level)
		{
			//first remove the currentlevel
			DestroySpawnedLevel();
			//Set the layers
			mainLayer = new TileLayer(level.Rows, level.Cols);
			bridgeLayer = new TileLayer(level.Rows, level.Cols, Tile.Type.Obstacle);

			Level.ColorSettings levelColorSettings = CurrentLevelSettings;
			Color32[] levelPixelData = level.LevelTexture.GetPixels32();

			float anchorStepPerColumn = 1.0f / level.Cols;
			float anchorStepPerRow = 1.0f / level.Rows;
			//Go through every pixel of the levelData
			for (int i = 0; i < levelPixelData.Length; ++i)
			{
				Color32 pixel = levelPixelData[i];
				int row = i / level.Cols;
				int col = i % level.Cols;
				bool overlayOn = false;

				//If the pixel has the bridge tile colour make an extra tile  with different allowed connection directions
				if (pixel.CompareRGB(levelColorSettings.BridgeTileColor))
				{
					mainLayer.Tiles[row, col].AllowedConnectionDirections = Tile.ConnectionDirection.East | Tile.ConnectionDirection.West;
					bridgeLayer.Tiles[row, col].AllowedConnectionDirections = Tile.ConnectionDirection.North | Tile.ConnectionDirection.South;
					bridgeLayer.Tiles[row, col].TileType = Tile.Type.Connection;
					overlayOn = true;
				}
				// Get the tileData at the right coordinates
				Tile tileData = mainLayer.Tiles[row, col];
				//Get the groups and types of the data based on the colour
				tileData.TileGroup = levelColorSettings.GetTileGroupFromColor(pixel);
				tileData.TileType = levelColorSettings.GetTileTypeFromColor(pixel);
				// Spawn in a game object for the tile
				TileController tileController = SpawnTileController(row, col, anchorStepPerColumn, anchorStepPerRow, overlayOn);
				
				tileController.AddTilesToControl(mainLayer.Tiles[row, col], bridgeLayer.Tiles[row, col]);
				tileController.Image.color = pixel;
				// add listeners 
				tileController.Interacted += OnTileInteractedWith;
				tileController.OnFinishedInteractingAt += OnFinishedInteractingAtTile;
			}
		}

		private TileController SpawnTileController(int row, int col, float anchorStepPerColumn, float anchorStepPerRow, bool overlayOn)
		{
			//Instantiate a tile
			TileController tileController = Instantiate(tileControllerPrefab, Vector3.zero, Quaternion.identity, CachedTransform);
			// Give him a name with the coordinates (for clarities)
			tileController.name = $"Tile {row}, {col}";
			//Set the anchors
			tileController.CachedRectTransform.anchorMin = new Vector2(col * anchorStepPerColumn, row * anchorStepPerRow);
			tileController.CachedRectTransform.anchorMax = new Vector2((col + 1) * anchorStepPerColumn, (row + 1) * anchorStepPerRow);
			//Set the position on the position
			tileController.CachedRectTransform.offsetMin = tileController.CachedRectTransform.offsetMax = Vector3.zero;
			tileController.CachedRectTransform.localPosition = new Vector3(tileController.CachedRectTransform.localPosition.x, tileController.CachedRectTransform.localPosition.y, 0.0f);

			tileController.BridgeOverlay.enabled = overlayOn;

			//Return the tileController
			return tileController;
		}

		private void DestroySpawnedLevel()
		{
			//Remove each game object in the cachedtransform
			foreach (Transform spawnedTile in CachedTransform)
			{
				Destroy(spawnedTile.gameObject);
			}
			//Remove the Layers
			mainLayer = null;
			bridgeLayer = null;
			//Set the lastineractedtilecontroller to null and remove the finishedGroups
			lastInteractedWithTileController = null;
			finishedGroups.Clear();
		}

		public bool IsOnBridgeLayer(Tile tile)
		{
			return bridgeLayer.Tiles[tile.Row, tile.Col].TileType == Tile.Type.Connection;
		}

		public Tile GetBridgeTile(Tile tile)
		{
			Tile bridgeTile = bridgeLayer.Tiles[tile.Row, tile.Col];
			return bridgeTile;
		}

		private void OnTileInteractedWith(TileController tileController)
		{
			//If it cant be interacted with then just return
			if (!canBeInteractedWith)
			{
				return;
			}
			//If the tilecontroller is the last tile that has been interacted with return
			if (tileController == lastInteractedWithTileController)
			{
				return;
			}
			//If it has already been interacted with try to resume the path from that tile and return
			if (!HasInteractedWithTile)
			{
				TryResumePathFrom(tileController);
				return;
			}
			//If you can removetileconnections after the tile then do so and return
			if (TryRemoveTileConnectionsAfter(tileController))
			{
				return;
			}
			//If the tile is already part of a finished group return
			if(IsTilePartOfFinishedGroups(tileController))
			{
				return;
			}
			// Return if it cant connect to the lastinteracted with tile
			if(!tileController.TryConnectTo(lastInteractedWithTileController))
			{
				return;
			}
			//Validate the path between tilecontroller and the tilecontroller and set the spite correctly
			ValidatePath(tileController, lastInteractedWithTileController);

			if (tileController.Row == lastInteractedWithTileController.Row)
			{
				
				tileController.ChangeSprite(tileSpriteSettings[tileController.TileGroup.SpriteIndex].TubeWestToEast);
			}
			else
			{
				tileController.ChangeSprite(tileSpriteSettings[tileController.TileGroup.SpriteIndex].TubeNorthToSouth);
			}
			//Set the lastinteractedwithtilecontroller to the current tilecontroller
			lastInteractedWithTileController = tileController;

			UpdateWinStatus();
		}
		
		private bool IsTilePartOfFinishedGroups(TileController tileController)
		{
			foreach(Tile controlledTile in tileController.ControlledTiles)
			{
				//if the tiletype is an obstacle just continue
				if(controlledTile.TileType == Tile.Type.Obstacle)
				{
					continue;
				}
				//If the finished groups dont contain the controlledtiles their group just return false
				if(!finishedGroups.Contains(controlledTile.TileGroup))
				{
					return false;
				}
			}

			return true;
		}

		private void ValidatePath(TileController currentTileController, TileController lastTileController)
		{
			//If the last tile isnt ungrouped
			if(lastTileController.TileGroup == Tile.Group.Ungrouped)
			{
				return;
			}
			//calculate the patch of lasttile on the mainlayer
			TilePath path = mainLayer.CalculatePathForGroup(lastTileController.TileGroup);
			// if the path doesnt contain the currenttilecontroller
			if (!path.Tiles.Contains(currentTileController.ConnectedControlledTile))
			{
				return;
			}
			// Get the index of the lasttilecontroller
			int lastTileControllerConnectedTileIndexInPath = path.Tiles.IndexOf(lastTileController.ConnectedControlledTile);

			// If the tile was not found or is the very first tile in the path, there is nothing before that, so we cannot do corner logic.
			if(lastTileControllerConnectedTileIndexInPath <= 0)
			{
				return;
			}
			// get the tile before the last tile
			Tile previousToLastTile = path.Tiles[lastTileControllerConnectedTileIndexInPath - 1];
			//see if they have corners
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
				lastTileController.ChangeSprite(tileSpriteSettings[lastTileController.TileGroup.SpriteIndex].TubeWestToEast);
				currentTileController.ChangeSprite(tileSpriteSettings[currentTileController.TileGroup.SpriteIndex].TubeWestToEast);
				return;
			}
			if (lastTileTileData.Col == currentTileTileData.Col && lastTileTileData.Col == previousToLastTile.Col && lastTileTileData.Row != currentTileTileData.Row)
			{
				// there is an X difference but no Y difference
				lastTileController.ChangeSprite(tileSpriteSettings[lastTileController.TileGroup.SpriteIndex].TubeNorthToSouth);
				currentTileController.ChangeSprite(tileSpriteSettings[currentTileController.TileGroup.SpriteIndex].TubeNorthToSouth);
				return;
			}

			if (currentTileTileData.Col != previousToLastTile.Col && currentTileTileData.Row != previousToLastTile.Row)
			{
				if (currentXSmallerThanPTL && currentYSmallerThanPTL)
				{
					if (currentOnSameRowAsL)
					{
						lastTileController.ChangeSprite(tileSpriteSettings[lastTileController.TileGroup.SpriteIndex].TubeWestToNorth);
					}
					else
					{
						lastTileController.ChangeSprite(tileSpriteSettings[lastTileController.TileGroup.SpriteIndex].TubeEastToSouth);
					}
					return;
				}

				if (currentXSmallerThanPTL && !currentYSmallerThanPTL)
				{
					if (currentOnSameRowAsL)
					{
						lastTileController.ChangeSprite(tileSpriteSettings[lastTileController.TileGroup.SpriteIndex].TubeWestToSouth);
					}
					else
					{
						lastTileController.ChangeSprite(tileSpriteSettings[lastTileController.TileGroup.SpriteIndex].TubeEastToNorth);
					}
					return;
				}

				if (!currentXSmallerThanPTL && currentYSmallerThanPTL)
				{
					if (currentOnSameRowAsL)
					{
						lastTileController.ChangeSprite(tileSpriteSettings[lastTileController.TileGroup.SpriteIndex].TubeEastToNorth);
					}
					else
					{
						lastTileController.ChangeSprite(tileSpriteSettings[lastTileController.TileGroup.SpriteIndex].TubeWestToSouth);
					}
					return;
				}

				if (!currentXSmallerThanPTL && !currentYSmallerThanPTL)
				{
					if (currentOnSameRowAsL)
					{
						lastTileController.ChangeSprite(tileSpriteSettings[lastTileController.TileGroup.SpriteIndex].TubeEastToSouth);
					}
					else
					{
						lastTileController.ChangeSprite(tileSpriteSettings[lastTileController.TileGroup.SpriteIndex].TubeWestToNorth);
					}
					return;
				}
			}
		}

		private void OnFinishedInteractingAtTile(TileController tile)
		{
			//reset the lastinteractedwithtile
			lastInteractedWithTileController = null;
		}

		private bool TryResumePathFrom(TileController tile)
		{
			//if the tile has been interacted with return false
			if (HasInteractedWithTile)
			{
				return false;
			}
			//if the tile is ungroupped return false
			if (tile.TileGroup == Tile.Group.Ungrouped)
			{
				return false;
			}
			//if the tile doesnt have a main tile set or the maintiletype is endpoint return false
			if (!tile.IsMainTileSet || tile.MainTile.TileType == Tile.Type.EndPoint)
			{
				return false;
			}
			//otherwise remove tile connections after the tile and return true
			RemoveTileConnectionsAfter(tile);
			return true;
		}

		private bool TryRemoveTileConnectionsAfter(TileController tile)
		{
			// if the tileGroup to ungrouped return false
			if (tile.TileGroup == Tile.Group.Ungrouped)
			{
				return false;
			}
			// calculate the path for the tiles tilegroup
			TilePath interactedTileGroupPath = mainLayer.CalculatePathForGroup(tile.TileGroup);
			//Get the index of both the tile and the last tile
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
			// call removetileconnecitonsafter this on the tile and remove the tilegroup form finished groups
			tile.ConnectedControlledTile.RemoveTileConnectionsAfterThis();
			finishedGroups.Remove(tile.TileGroup);
			//set lastinteractedwithtilecontroller to the tile
			lastInteractedWithTileController = tile;
		}

		private void UpdateWinStatus()
		{
			//get the tiledata with the lastineractedwithtilecontroller
			Tile tileData = lastInteractedWithTileController.ConnectedControlledTile;
			// if isnt an endpoint just return
			if (tileData.TileType != Tile.Type.EndPoint)
			{
				return;
			}
			// otherwise add the tilegroup
			finishedGroups.Add(tileData.TileGroup);
			// go through all tilegroups
			foreach (Tile.Group tileGroup in CurrentLevelSettings.TileGroups)
			{
				//return if the group isnt in finishedgroups
				if (!finishedGroups.Contains(tileGroup))
				{
					return;
				}
			}
			//otherwise go to the next level as this one is complete
			NextLevel();
			Debug.Log("Level complete!");
		}
	}
}