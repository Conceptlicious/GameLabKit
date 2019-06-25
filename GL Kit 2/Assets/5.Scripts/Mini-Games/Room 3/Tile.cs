using System;
using UnityEngine;

namespace Room3
{
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
			//Group is basically a helper class that can be used to compare the colours of the tubes
			public const Group Ungrouped = null;

			[SerializeField] private Color32 groupColor = Color.black;
			public Color32 GroupColor => groupColor;

			public Group() { }
			public Group(Color32 groupColor) => this.groupColor = groupColor;
		}

		[Flags]
		public enum ConnectionDirection
		{
			None = 0,
			All = ~0,
			North = 1 << 0,
			East = 1 << 1,
			West = 1 << 2,
			South = 1 << 3
		}

		public event Action<Tile> Connected;
		public event Action<Tile> Disconnected;

		public int Row { get; private set; } = 0;
		public int Col { get; private set; } = 0;

		public Type TileType { get; set; } = Type.Connection;
		public Group TileGroup { get; set; } = Group.Ungrouped;

		public TileLayer Layer { get; set; } = null;

		public bool CanConnectToOtherTiles => TileType != Type.Obstacle && (TileType != Type.Connection || TileGroup == Group.Ungrouped);

		public Tile NextTile { get; set; } = null;

		public TileSpriteSettings SpriteSettings { get; set; }

		public ConnectionDirection AllowedConnectionDirections { get; set; } = ConnectionDirection.All;

		public Tile(int row, int col, TileLayer layer)
		{
			Row = row;
			Col = col;
			Layer = layer;
		}

		/// <summary>
		/// Tries to connect to the provided tile. This does not consider layers and allows for tiles of different layers to be connected to each other
		/// </summary>
		/// <param name="tile">The tile to try to connect to</param>
		/// <returns>If connected successfully</returns>
		public bool TryConnectTo(Tile tile)
		{
			// If the tile itself doesn't exist just return false as it can't do any of the other checks
			if (tile == null)
			{
				return false;
			}
			//If it cannot connect to other tiles return false as it can't connect
			if (!CanConnectToOtherTiles)
			{
				return false;
			}
			// If the tilegroups aren't part of ungrouped and the tilegroups arent the same
			if (TileGroup != Group.Ungrouped && TileGroup != tile.TileGroup)
			{
				return false;
			}
			// If the connectiondirections of both arent allowed then return false
			if (!IsConnectionDirectionAllowed(tile) || !tile.IsConnectionDirectionAllowed(this))
			{
				return false;
			}
			// If the tile isn't a neighbour of this tile return false
			if (!IsNeighborOf(tile))
			{
				return false;
			}
			//Otherwise connect the tiles and return true
			tile.NextTile = this;
			TileGroup = tile.TileGroup;

			Connected?.Invoke(tile);

			return true;
		}

		public void RemoveTileConnectionsAfterThis()
		{
			//If the next tile is null just return it doesn't have a tile connection
			if (NextTile == null)
			{
				return;
			}
			//set the disconnected tile to next tile
			Tile disconnectedTile = NextTile;
			// set the next tiles group to ungrouped and remove them from this tile
			NextTile.TileGroup = Group.Ungrouped;
			NextTile = null;
			//make the disconnected tile remove the connections after this
			disconnectedTile.RemoveTileConnectionsAfterThis();
			//invoke the disconnected event
			disconnectedTile?.Disconnected(this);
		}

		private bool IsNeighborOf(Tile tile)
		{
			//get the row and collumn difference
			int rowDiffefrence = Mathf.Abs(tile.Row - Row);
			int colDifference = Mathf.Abs(tile.Col - Col);
			//check to make sure that they are indeed neighbours
			return (rowDiffefrence < 2 && colDifference < 2) && Mathf.Abs(rowDiffefrence - colDifference) == 1;
		}

		private bool IsConnectionDirectionAllowed(Tile tile)
		{
			//check the flags of the tiles for allowed connectiondirections and return true or false depending if they are allowed
			return  (Col - tile.Col > 0 && AllowedConnectionDirections.HasFlag(ConnectionDirection.East)) ||
					(Col - tile.Col < 0 && AllowedConnectionDirections.HasFlag(ConnectionDirection.West)) ||
					(Row - tile.Row > 0 && AllowedConnectionDirections.HasFlag(ConnectionDirection.South)) ||
					(Row - tile.Row < 0 && AllowedConnectionDirections.HasFlag(ConnectionDirection.North));
		}

	}
}
