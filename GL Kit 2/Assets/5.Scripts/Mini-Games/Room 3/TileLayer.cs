namespace Room3
{
	public class TileLayer
	{
		public Tile[,] Tiles { get; private set; } = null;

		public TileLayer(int rows, int cols, Tile.Type defaultTileType = Tile.Type.Connection)
		{
			Tiles = new Tile[rows, cols];

			for (int row = 0; row < rows; ++row)
			{
				for (int col = 0; col < cols; ++col)
				{
					Tiles[row, col] = new Tile(row, col, this);
					Tiles[row, col].TileType = defaultTileType;
				}
			}
		}

		public TilePath CalculatePathForGroup(Tile.Group tileGroup)
		{
			TilePath path = new TilePath();
			//Get the startTile
			Tile currentTile = GetStartTileForGroup(tileGroup);
			//return the path if you cant get a start tile
			if (currentTile == null)
			{
				return path;
			}
			//Addtiles to the path aslong as current tile isn't null
			do
			{
				path.AddTileToPath(currentTile);
				currentTile = currentTile.NextTile;
			} while (currentTile != null);

			return path;
		}

		private Tile GetStartTileForGroup(Tile.Group tileGroup)
		{
			//Go through the grid and look for startpoints and return them whenever
			for (int row = 0; row < Tiles.GetLength(0); ++row)
			{
				for (int col = 0; col < Tiles.GetLength(1); ++col)
				{
					Tile tile = Tiles[row, col];

					if (tile.TileGroup != tileGroup)
					{
						continue;
					}

					if (tile.TileType != Tile.Type.StartPoint)
					{
						continue;
					}

					return tile;
				}
			}

			return null;
		}
	}
}
