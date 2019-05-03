using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore;
using System.Linq;

/*   -------------- To Do -----------

- Should listen to tiles and tell them to change
*/
namespace Room3 { 
public class Grid : MonoBehaviour
{   
    private Tile[,] coordinates;
    private Color32[] pixels;
    private int pixelCount = 0;
    private List<GameObject> currentGrid = new List<GameObject>();
    private List<DotPath> paths = new List<DotPath>();
    private int currentPathIndex = 0;
    private int gridIndex = 0;
    [SerializeField, Range(0,255)] private int alphaCutoffPoint = 125;
    [SerializeField] private GridMapStruct[] gridMaps;
    [SerializeField] private GameObject tilePrefab;

    private void Start()
    {     
        CreateGrid();  
    }

    public void NextGrid()
    {
        foreach(GameObject tile in currentGrid)
        {
            Destroy(tile);
        }
        paths = new List<DotPath>();
        gridIndex = (gridIndex + 1) % gridMaps.Length;
        CreateGrid();
    }

    private void CreateGrid()
    {
        pixels = gridMaps[gridIndex].GridMap.GetPixels32();
        coordinates = new Tile[gridMaps[gridIndex].GridMap.height, gridMaps[gridIndex].GridMap.width];
        pixelCount = pixels.Length;
        
        for (int y = 0; y < gridMaps[gridIndex].GridMap.height; y++)
        {
            for (int x = 0; x < gridMaps[gridIndex].GridMap.width; x++)
            {
              
                int index = (y * gridMaps[gridIndex].GridMap.width + x);
                SpawnTile(new Vector2Int(x,y), pixels[index], tilePrefab);
            }
        }
    }

    private void SpawnTile(Vector2Int gridPosition, Color32 color, GameObject tile)
    {
        GameObject tileObject = Instantiate(tile, new Vector2(0,0), Quaternion.identity, this.transform);
        Tile tileReference = tileObject.GetComponent<Tile>();
        currentGrid.Add(tileObject);
        RectTransform rectTransformInstance = tileObject.GetComponent<RectTransform>();
        rectTransformInstance.anchorMin = 
            new Vector2((gridPosition.x * (1.0f / gridMaps[gridIndex].GridMap.width)), gridPosition.y * (1.0f / gridMaps[gridIndex].GridMap.height));
        rectTransformInstance.anchorMax = 
            new Vector2(((gridPosition.x + 1) * (1.0f / gridMaps[gridIndex].GridMap.width)), (gridPosition.y + 1) * (1.0f / gridMaps[gridIndex].GridMap.height));
        rectTransformInstance.offsetMin = rectTransformInstance.offsetMax = Vector2.zero;
       tileReference.ChangeState(color, true, false);

        if(color.Compare(gridMaps[0].AccessibleColour))
        {
            tileReference.CurrentTileType = TypeOfTile.Accessible;
        }

        for (int i = 0; i < gridMaps[gridIndex].TubeColours.Length; i++)
        {
            if (color.CompareIgnoreAlpha(gridMaps[gridIndex].TubeColours[i]))
            {
                if (color.a <= alphaCutoffPoint)
                {
                    tileReference.CurrentTileType = TypeOfTile.EndTube;
                }
                else
                {
                    tileReference.CurrentTileType = TypeOfTile.StartTube;
                    DotPath path = new DotPath(color);
                    path.AddToList(tileReference);
                    paths.Add(path);
                    path.OnCompleted += OnTubeCompleted;
                }
            }
        }

        tileReference.CheckState();
        coordinates[gridPosition.x, gridPosition.y] = tileReference;
        
        if (color.Compare(gridMaps[gridIndex].InAccessibleColour))
        {
            tileReference.CurrentTileType = TypeOfTile.Inaccessible;
            tileReference.CheckState();
            return;
        }
        tileReference.OnMouseHoover += OnTubePlaced;
    }

    private void OnTubeCompleted()
    {
        bool allPathsComplete = paths.All(path => path.isCompleted);
        if (allPathsComplete)
        {
            NextGrid();
        }
        ////foreach ( Path path in from paths where !path.isCompleted)
        ////{

        ////}
    }

    private void OnTubePlaced(Tile tile)
    {
        foreach (DotPath path in paths)
        {
            if(path == paths[currentPathIndex])
            {
                continue;
            }

            if (path.ContainsTile(tile))
            {
                currentPathIndex = paths.IndexOf(path);
                return;
            }
        }

        if (CheckAdjacency(tile, paths[currentPathIndex].lastTile) || tile.CurrentTileType == TypeOfTile.StartTube)
        {         
            paths[currentPathIndex].lastTile.CheckState();
            paths[currentPathIndex].AddToList(tile);
            tile.CheckState();
        }
       
        
    }
    private bool CheckAdjacency(Tile tile, Tile other)
    {
        
        int xCoordinate = 0;
        int yCoordinate = 0;
        for (int x = 0; x < coordinates.GetLength(0); x++)
        {
            for (int y = 0; y < coordinates.GetLength(1); y++)
            {
                if(tile == coordinates[x, y])
                {
                    xCoordinate = x;
                    yCoordinate = y;
                    break;
                }
            }
        }
        if(!(xCoordinate + 1 >= coordinates.GetLength(0)))
        {
           if(other == coordinates[xCoordinate +1, yCoordinate])
           {
                tile.EastTile = other;
                other.WestTile = tile;
                return true;
           }
        }
        if (!(yCoordinate + 1 >= coordinates.GetLength(1)))
        {
            if (other == coordinates[xCoordinate, yCoordinate + 1])
            {
                tile.NorthTile = other;
                other.SouthTile = tile;
                // other is above
                return true;
            }
        }
        if (!(xCoordinate - 1 < 0))
        {
            if (other == coordinates[xCoordinate - 1, yCoordinate])
            {
                // other is to the left
                tile.WestTile = other;
                other.EastTile = tile;
                return true;
            }
        }
        if (!(yCoordinate - 1 < 0))
        {
            if (other == coordinates[xCoordinate, yCoordinate - 1])
            {
                // other is below
                tile.SouthTile = other;
                other.NorthTile = tile;
                return true;
            }
        }
        return false;
    }

   

}

public static class Color32Extensions
{
    public static bool Compare(this Color32 color, Color32 other)
    {
        return color.r == other.r && color.g == other.g && color.b == other.b && color.a == other.a;
    }
    public static bool CompareIgnoreAlpha(this Color32 color, Color32 other)
    {
        return color.r == other.r && color.g == other.g && color.b == other.b;
    }
}

    public static class Vector3Extensions
    {
        public static Vector3 V3Multiplied(this Vector3 vector3, Vector3 other)
        {
            return (new Vector3(vector3.x * other.x, vector3.y * other.y, vector3.z * other.z));
        }
        public static Vector3 V3Plus(this Vector3 vector3, Vector3 other)
        {
            return (new Vector3(vector3.x + other.x, vector3.y + other.y, vector3.z + other.z));
        }
    }
}