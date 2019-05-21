using System;
using UnityEngine;

[Serializable]
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

		[SerializeField] private Color32 groupColor = Color.black;
		public Color32 GroupColor => groupColor;

		public Group() {}
		public Group(Color32 groupColor) => this.groupColor = groupColor;
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

	public TileLayer TileLayer = null;

	public bool CanConnectToOtherTiles => TileType != Type.Obstacle && (TileType != Type.Connection || TileGroup == Group.Ungrouped);

	public Tile NextTile { get; set; } = null;
	public TileSpriteSettings SpriteSettings { get; set; }

	public ConnectionDirection AllowedConnectionDirections { get; set; } = ConnectionDirection.All;

	public Tile(int row, int col)
	{
		Row = row;
		Col = col;
	}

	public bool TryConnectTo(Tile tile)
	{
		if(tile == null)
		{
			return false;
		}

		if(!CanConnectToOtherTiles)
		{
			return false;
		}

		if(TileGroup != Group.Ungrouped && TileGroup != tile.TileGroup)
		{
			return false;
		}

		if(!IsConnectionDirectionAllowed(tile))
		{
			return false;
		}

		if(!IsNeighborOf(tile))
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

		Tile disconnectedTile = NextTile;

		NextTile.TileGroup = Group.Ungrouped;
		NextTile = null;

		disconnectedTile.OnDisconnectedFromTile(this);

		disconnectedTile.RemoveTileConnectionsAfterThis();
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
