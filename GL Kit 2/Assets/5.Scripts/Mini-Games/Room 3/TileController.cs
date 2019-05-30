using System;
using System.Collections.Generic;
using GameLab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Room3
{
	[RequireComponent(typeof(Image))]
	public class TileController : BetterMonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
	{
		public event Action<TileController> OnInteractedWith;
		public event Action<TileController> OnFinishedInteractingAt;

		public Image Image { get; private set; }

		private Dictionary<TileLayer, Tile> tileData = null;
		public Dictionary<TileLayer, Tile> TileData
		{
			get => tileData;
			set
			{
				foreach (KeyValuePair<TileLayer, Tile> tileData in TileData)
				{
					if(tileData.Value != null)
					{
						tileData.Value.OnConnectedToTile -= OnConnectedToTile;
						tileData.Value.OnDisconnectedFromTile -= OnDisconnectedFromTile;
					}
				}

				tileData = value;

				foreach (KeyValuePair<TileLayer, Tile> tileData in TileData)
				{
					if (tileData.Value != null)
					{
						tileData.Value.OnConnectedToTile += OnConnectedToTile;
						tileData.Value.OnDisconnectedFromTile += OnDisconnectedFromTile;
					}
				}
			}
		}


		private Color? originalTileColor = null;

		private void Awake()
		{
			Image = GetComponent<Image>();
		}

		public void ChangeSprite(Sprite spritetoChange)
		{
			if (tileData.TileType != Tile.Type.EndPoint)
			{
				Image.sprite = spritetoChange;
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			OnInteractedWith?.Invoke(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (Input.GetMouseButton(0))
			{
				OnInteractedWith?.Invoke(this);
			}
		}

		private void OnConnectedToTile(Tile tileConnectedTo)
		{
			if (!originalTileColor.HasValue)
			{
				originalTileColor = Image.color;
			}

			Image.color = TileData.TileGroup.GroupColor;
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
