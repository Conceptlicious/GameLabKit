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
				if(controlledTiles.Contains(tile))
				{
					continue;
				}

				if(MainTile == null)
				{
					MainTile = tile;

					if(MainTile.TileType == Tile.Type.StartPoint || MainTile.TileType == Tile.Type.EndPoint)
					{
						ConnectedControlledTile = MainTile;
					}
				}

				if(tile.Row != Row || tile.Col != Col)
				{
					continue;
				}

				tile.Connected += OnConnectedToTile;
				tile.Disconnected += OnDisconnectedFromTile;

				controlledTiles.Add(tile);
			}
		}

		public void RemoveTilesToControl(params Tile[] tiles)
		{
			foreach(Tile tile in tiles)
			{
				if(!controlledTiles.Contains(tile))
				{
					continue;
				}

				tile.Connected -= OnConnectedToTile;
				tile.Disconnected -= OnDisconnectedFromTile;

				if(tile == MainTile)
				{
					MainTile = null;
				}

				controlledTiles.Remove(tile);
			}

			if(HasControlledTiles)
			{
				MainTile = controlledTiles[0];
			}
		}

		public bool TryConnectTo(TileController tileController)
		{
			foreach(Tile controlledTile in controlledTiles)
			{
				foreach(Tile otherControlledTile in tileController.controlledTiles)
				{
					if(controlledTile.TryConnectTo(otherControlledTile))
					{
						ConnectedControlledTile = controlledTile;
						return true;
					}
				}
			}

			ConnectedControlledTile = null;
			return false;
		}

		public void ChangeSprite(Sprite spritetoChange)
		{
			if(!HasControlledTiles)
			{
				return;
			}

			if(controlledTiles[0].TileType != Tile.Type.Connection)
			{
				return;
			}

			Image.sprite = spritetoChange;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Interacted?.Invoke(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (Input.GetMouseButton(0))
			{
				Interacted?.Invoke(this);
			}
		}

		private void OnConnectedToTile(Tile tileConnectedTo)
		{
			if (!originalTileColor.HasValue)
			{
				originalTileColor = Image.color;
			}

			Image.color = tileConnectedTo.TileGroup.GroupColor;
		}

		private void OnDisconnectedFromTile(Tile tileDisconnectedFrom)
		{
			Image.color = originalTileColor.GetValueOrDefault(TileGrid.Instance.CurrentLevelSettings.NormalTileColor);
			Image.sprite = null;
		}
		
		public void OnPointerUp(PointerEventData eventData)
		{
			OnFinishedInteractingAt?.Invoke(this);
		}
	}
}
