using System;
using System.Collections.Generic;
using GameLab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Room3
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	public class TileController : BetterMonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
	{
		//The events for when the tile gets interacted with
		public event Action<TileController> Interacted;
		public event Action<TileController> OnFinishedInteractingAt;

		public Tile MainTile { get; private set; } = null;
		public Tile ConnectedControlledTile { get; private set; } = null;

		public int Row => IsMainTileSet ? MainTile.Row : -1;
		public int Col => IsMainTileSet ? MainTile.Col : -1;

		public Tile.Group TileGroup => IsConnectedToAnotherTile ? ConnectedControlledTile.TileGroup : Tile.Group.Ungrouped;

		public Image Image { get; private set; }

		private List<Tile> controlledTiles = new List<Tile>();
		public IReadOnlyCollection<Tile> ControlledTiles => controlledTiles.AsReadOnly();

		public bool HasControlledTiles => controlledTiles.Count > 0;
		public bool IsMainTileSet => MainTile != null;
		public bool IsConnectedToAnotherTile => ConnectedControlledTile != null;

		private Color? originalTileColor = null;

		private void Awake()
		{
			Image = GetComponent<Image>();
		}

		public void AddTilesToControl(params Tile[] tiles)
		{
			foreach(Tile tile in tiles)
			{
				//the already controlled tiles contain this tile continue to the next tile
				if(controlledTiles.Contains(tile))
				{
					continue;
				}
				//if the maintile is null set the tile to the maintile
				if(MainTile == null)
				{
					MainTile = tile;
					//if the maintile is a start or end point the the connected controllertile to main tile
					if(MainTile.TileType == Tile.Type.StartPoint || MainTile.TileType == Tile.Type.EndPoint)
					{
						ConnectedControlledTile = MainTile;
					}
				}
				// if the tile is neither on the same row nor the same collumn continue to the next tile
				if(tile.Row != Row || tile.Col != Col)
				{
					continue;
				}
				//otherwist add onconnectedtotile and ondisconnectedfromtile to the tiles events
				tile.Connected += OnConnectedToTile;
				tile.Disconnected += OnDisconnectedFromTile;
				//and finally add the tile to the controlled tiles
				controlledTiles.Add(tile);
			}
		}

		public void RemoveTilesToControl(params Tile[] tiles)
		{
			foreach(Tile tile in tiles)
			{
				//if the tile isnt in controlledtiles go to the next tile
				if(!controlledTiles.Contains(tile))
				{
					continue;
				}
				//remove the methods to the events
				tile.Connected -= OnConnectedToTile;
				tile.Disconnected -= OnDisconnectedFromTile;
				//if the tile is the maintile set the maintile to null
				if(tile == MainTile)
				{
					MainTile = null;
				}
				//remove the tile from controlledtiles
				controlledTiles.Remove(tile);
			}
			//if controlledtiles is bigger than 0, set the maintile to the first in controlledTiles
			if(HasControlledTiles)
			{
				MainTile = controlledTiles[0];
			}
		}

		public bool TryConnectTo(TileController tileController)
		{
			foreach(Tile controlledTile in controlledTiles)
			{
				// a double for each loop in the controlled tiles of this controlled tiles
				foreach(Tile otherControlledTile in tileController.controlledTiles)
				{
					// if it can connect set the connectedcontrolledtile to controlledtile
					if(controlledTile.TryConnectTo(otherControlledTile))
					{
						ConnectedControlledTile = controlledTile;
						return true;
					}
				}
			}
			// set the connectedcontrolled tile to false and returned false
			ConnectedControlledTile = null;
			return false;
		}

		public void ChangeSprite(Sprite spritetoChange)
		{
			//if the sprite doesnt have hascontrolledtiles as true just return
			if(!HasControlledTiles)
			{
				return;
			}
			//if it isnt a connection just return it shouldnt have a sprite
			if(controlledTiles[0].TileType != Tile.Type.Connection)
			{
				return;
			}
			// change the sprite
			Image.sprite = spritetoChange;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			//when it gets clicked on invoke the interacted
			Interacted?.Invoke(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//if the LMB is being held down and you enter their image invoke the interacted event
			if (Input.GetMouseButton(0) && controlledTiles[0].TileType != Tile.Type.StartPoint) 
			{
				Interacted?.Invoke(this);
			}
		}

		private void OnConnectedToTile(Tile tileConnectedTo)
		{
			// If the original colour doesn't have a value
			if (!originalTileColor.HasValue)
			{
				// change the originalTileColor to the current color
				originalTileColor = Image.color;
			}
			// change the colour to the group color
			//Image.color = tileConnectedTo.TileGroup.GroupColor;
			Image.color = Color.white;
		}

		private void OnDisconnectedFromTile(Tile tileDisconnectedFrom)
		{
			//Change the color back to the original tile color and reset the sprite
			Image.color = originalTileColor.GetValueOrDefault(TileGrid.Instance.CurrentLevelSettings.NormalTileColor);
			Image.sprite = null;
		}
		
		public void OnPointerUp(PointerEventData eventData)
		{
			// if you let go of LMB then invoke OnfinishedinteractingAt
			OnFinishedInteractingAt?.Invoke(this);
		}
	}
}
