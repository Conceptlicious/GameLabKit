using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLab;
using UnityEngine;

public class TileLayer
{
	public Tile[,] Tiles { get; private set; } = null;

	public TileLayer(int rows, int cols, Tile.Type defaultTileType = Tile.Type.Connection)
	{
		Tiles = new Tile[rows, cols];

		for(int row = 0; row < rows; ++row)
		{
			for(int col = 0; col < cols; ++col)
			{
				Tiles[row, col] = new Tile(row, col);
				Tiles[row, col].TileType = defaultTileType;
			}
		}
	}

	public void RemoveConnectionsAfterTile(Tile tile)
	{
		tile = tile.NextTile;

		while (tile != null)
		{
			if (tile.TileType != Tile.Type.StartPoint || tile.TileType != Tile.Type.EndPoint)
			{
				tile = tile.RemoveTileConnection();
			}
		}
	}

	public TilePath CalculatePathForGroup(Tile.Group tileGroup)
	{
		TilePath path = new TilePath();

		Tile currentTile = GetStartTileForGroup(tileGroup);

		if(currentTile == null)
		{
			return path;
		}

		do
		{
			path.AddTileToPath(currentTile);
			currentTile = currentTile.NextTile;
		} while(currentTile != null);

		return path;
	}

	private Tile GetStartTileForGroup(Tile.Group tileGroup)
	{
		for(int row = 0; row < Tiles.GetLength(0); ++row)
		{
			for(int col = 0; col < Tiles.GetLength(1); ++col)
			{
				Tile tile = Tiles[row, col];

				if(tile.TileGroup != tileGroup)
				{
					continue;
				}

				if(tile.TileType != Tile.Type.StartPoint)
				{
					continue;
				}

				return tile;
			}
		}

		return null;
	}
}
