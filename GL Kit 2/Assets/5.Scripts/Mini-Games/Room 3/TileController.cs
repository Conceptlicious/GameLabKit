using System;
using GameLab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TileController : BetterMonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
	public event Action<TileController> OnInteractedWith;
	public event Action<TileController> OnFinishedInteractingAt;

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

	private Color? originalTileColor = null;

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
		if(!originalTileColor.HasValue)
		{
			originalTileColor = Image.color;
		}

		Image.color = TileData.TileGroup.GroupColor;
	}

	private void OnDisconnectedFromTile(Tile tileDisconnectedFrom)
	{
		Image.color = originalTileColor.GetValueOrDefault(TileGrid.Instance.CurrentLevelSettings.NormalTileColor);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		OnFinishedInteractingAt?.Invoke(this);
	}
}
