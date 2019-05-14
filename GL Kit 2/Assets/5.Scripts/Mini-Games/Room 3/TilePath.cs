using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct TilePath
{
	public bool IsComplete =>	Tiles != null &&
								Tiles.Count > 1 &&
								Tiles[0].TileType == Tile.Type.StartPoint &&
								Tiles[Tiles.Count - 1].TileType == Tile.Type.EndPoint;

	public List<Tile> Tiles { get; private set; }

	public void AddTileToPath(Tile tile)
	{
		if(Tiles == null)
		{
			Tiles = new List<Tile>();
		}
		Tiles.Add(tile);
	}

}
