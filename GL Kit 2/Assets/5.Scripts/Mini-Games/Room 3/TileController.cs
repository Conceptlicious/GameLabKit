using System;
using System.Collections;
using System.Collections.Generic;
using GameLab;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TileController : BetterMonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
	public event Action<TileController> OnMouseHovered;
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
			}

			tileData = value;

			if(tileData != null)
			{
				tileData.OnConnectedToTile += OnConnectedToTile;
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
		OnMouseHovered?.Invoke(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (Input.GetMouseButton(0))
		{
			OnMouseHovered?.Invoke(this);
		}
	}

	private void OnConnectedToTile(Tile tileConnectedTo)
	{
		Image.color = TileData.TileGroup.GroupColor;
	}
}
