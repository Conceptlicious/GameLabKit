using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ------------- TO DO --------------
 - Should keep track of all the paths based on the amount of colours in the grid
 - Should update itself and tell the grid to updtate the tiles based on how the path progresses
 - */

namespace Room3
{
    public class DotPath
    {
        private List<Tile> tiles = new List<Tile>();
        public Tile lastTile => tiles.Count > 0 ? tiles[tiles.Count - 1] : null;
        private Color32 pathColor;
        public event Action OnCompleted;
        public bool isCompleted => lastTile?.CurrentTileType == TypeOfTile.EndTube;

        public DotPath(Color32 color)
        {
            pathColor = color;
        }

        public void AddToList(Tile tile)
        {

            if (tiles.Contains(tile))
            {
                RemoveToTile(tile);
            }
            else
            {
                if (!isCompleted)
                {
                    tiles.Add(tile);
                    if (tile.CurrentTileType == TypeOfTile.EndTube)
                    {
                        if (!Color32Extensions.CompareIgnoreAlpha(tile.image.color, pathColor))
                        {
                            RemoveToTile(tile);
                            return;
                        }
                        OnCompleted?.Invoke();
                    }
                    tile.ChangeState(pathColor);
                }
            }

        }

        private void RemoveToTile(Tile tile)
        {
            for (int i = tiles.Count - 1; i > tiles.IndexOf(tile); i--)
            {
                tiles[i].ResetTube();
                tiles.RemoveAt(i);
            }
        }

        public bool ContainsTile(Tile tile)
        {
            return tiles.Contains(tile);
        }
    }
}