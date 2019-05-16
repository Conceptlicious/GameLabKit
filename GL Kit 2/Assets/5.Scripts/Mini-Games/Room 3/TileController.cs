using System;
using GameLab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TileController : BetterMonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
	public event Action<TileController> OnInteractedWith;

	public Image Image { get; private set; }

	private Tile tileData = null;
	public Tile TileData
	{
		get => tileData;
		set
		{
			if(tileData != null)
			{
				tileData.OnConnectedToTile -= OnConnectedToTile;
				tileData.OnDisconnectedFromTile -= OnDisconnectedFromTile;
			}

			tileData = value;

			if(tileData != null)
			{
				tileData.OnConnectedToTile += OnConnectedToTile;
				tileData.OnDisconnectedFromTile += OnDisconnectedFromTile;
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		
		Image = GetComponent<Image>();
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
		Image.color = TileData.TileGroup.GroupColor;
	}

	private void OnDisconnectedFromTile(Tile tileDisconnectedFrom)
	{
		Image.color = TileGrid.Instance.CurrentLevelSettings.NormalTileColor;
	}
}
