using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room3 {
	public struct TilePath
	{
		// If tiles isn't null and contains both a start point as the first entry and an end point as the last entry then it will be true otherwise it'll be false
		public bool IsComplete => Tiles != null &&
									Tiles.Count > 1 &&
									Tiles[0].TileType == Tile.Type.StartPoint &&
									Tiles[Tiles.Count - 1].TileType == Tile.Type.EndPoint;

		public List<Tile> Tiles { get; private set; }

		public void AddTileToPath(Tile tile)
		{
			//if tiles = null make a new list
			if (Tiles == null)
			{
				Tiles = new List<Tile>();
			}
			// add the parameter tile to tiles
			Tiles.Add(tile);
		}
	}
}
