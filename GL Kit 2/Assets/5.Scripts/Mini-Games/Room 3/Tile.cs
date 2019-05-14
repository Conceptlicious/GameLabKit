using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLab;
using UnityEngine;

public class Tile
{
	public enum Type
	{
		StartPoint,
		EndPoint,
		Connection,
		Obstacle
	}

	[Serializable]
	public class Group
	{
		public const Group Ungrouped = null;

		[SerializeField] private Color32 groupColor;
		public Color32 GroupColor => groupColor;
	}

	[Flags]
	public enum ConnectionDirection
	{
		None	= 0,
		All		= ~0,
		North	= 1 << 0,
		East	= 1 << 1,
		West	= 1 << 2,
		South	= 1 << 3
	}

	public event Action<Tile> OnConnectedToTile;
	public event Action<Tile> OnDisconnectedFromTile;

	public int Row { get; private set; } = 0;
	public int Col { get; private set; } = 0;

	public Type TileType { get; set; } = Type.Connection;
	public Group TileGroup { get; set; } = Group.Ungrouped;

	public bool CanConnectToOtherTiles => TileType != Type.Obstacle && (TileType != Type.Connection || TileGroup == Group.Ungrouped);

	public Tile NextTile { get; set; } = null;

	public ConnectionDirection AllowedConnectionDirections { get; set; } = ConnectionDirection.All;

	public Tile(int row, int col)
	{
		Row = row;
		Col = col;
	}

	public bool TryConnectTo(Tile tile)
	{
		if(!CanConnectToOtherTiles)
		{
			return false;
		}

		if(!IsNeighborOf(tile))
		{
			return false;
		}

		if(!IsConnectionDirectionAllowed(tile))
		{
			return false;
		}

		tile.NextTile = this;
		TileGroup = tile.TileGroup;

		OnConnectedToTile?.Invoke(tile);

		return true;
	}

	public void RemoveTileConnectionsAfterThis()
	{
		if(NextTile == null)
		{
			return;
		}

		Tile tileToDisconnect = NextTile;
		Tile lastDisconnectedTile = null;

		while(tileToDisconnect.NextTile != null)
		{
			lastDisconnectedTile = tileToDisconnect;
			tileToDisconnect = tileToDisconnect.RemoveTileConnection();
		}

		// Let listeners know that the last tile is also disconnected. The RemoveTileConnection method does nothing on it because it does not have a next tile, but its previous tile just removed it.
		tileToDisconnect.OnDisconnectedFromTile?.Invoke(lastDisconnectedTile);
	}

	private Tile RemoveTileConnection()
	{
		if(NextTile == null)
		{
			return null;
		}

		Tile tileToReturn = NextTile;

		NextTile.TileGroup = Group.Ungrouped;
		NextTile = null;

		OnDisconnectedFromTile?.Invoke(tileToReturn);

		return tileToReturn;
	}

	private bool IsNeighborOf(Tile tile)
	{
		int rowDiffefrence = Mathf.Abs(tile.Row - Row);
		int colDifference = Mathf.Abs(tile.Col - Col);

		return (rowDiffefrence < 2 && colDifference < 2) && Mathf.Abs(rowDiffefrence - colDifference) == 1;
	}

	private bool IsConnectionDirectionAllowed(Tile tile)
	{
		return  (Col - tile.Col > 0 && AllowedConnectionDirections.HasFlag(ConnectionDirection.East)) ||
				(Col - tile.Col < 0 && AllowedConnectionDirections.HasFlag(ConnectionDirection.West)) ||
				(Row - tile.Row > 0 && AllowedConnectionDirections.HasFlag(ConnectionDirection.South)) ||
				(Row - tile.Row < 0 && AllowedConnectionDirections.HasFlag(ConnectionDirection.North));
	}
}
