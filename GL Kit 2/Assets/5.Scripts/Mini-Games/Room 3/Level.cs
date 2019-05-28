using System;
using System.Linq;
using GameLab;
using Room3;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room 3 Level", menuName = "Room 3/Level")]
public class Level : ScriptableObject
{
	[Serializable]
	public struct ColorSettings
	{
		public static readonly ColorSettings Default = new ColorSettings()
		{
			NormalTileColor = new Color32(155,155,155,255),
			ObstacleTileColor = Color.black,
			BridgeTileColor = Color.yellow,
			StartPointAlphaThreshold = 126,
			TileGroups = new Tile.Group[3]
			{
				new Tile.Group(Color.red),
				new Tile.Group(Color.green),
				new Tile.Group(Color.blue)
			}
		};

		public Color32 NormalTileColor;
		public Color32 ObstacleTileColor;
		public Color32 BridgeTileColor;

		[Tooltip("The alpha value at or after which a tile is considered a start point and before which a tile is considered an end point")]
		[Range(0, 255)] public byte StartPointAlphaThreshold;

		public Tile.Group[] TileGroups;

		public Tile.Group GetTileGroupFromColor(Color32 color)
		{
			foreach(Tile.Group tubeGroup in TileGroups)
			{
				if(!tubeGroup.GroupColor.CompareRGB(color))
				{
					continue;
				}

				return tubeGroup;
			}

			return Tile.Group.Ungrouped;
		}

		public Tile.Type GetTileTypeFromColor(Color32 color)
		{
			if(color.CompareRGBA(NormalTileColor) || color.CompareRGBA(BridgeTileColor))
			{
				return Tile.Type.Connection;
			}

			if(color.CompareRGBA(ObstacleTileColor))
			{
				return Tile.Type.Obstacle;
			}

			if(TileGroups.Any(tubeGroup => tubeGroup.GroupColor.CompareRGB(color)))
			{
				if(color.a >= StartPointAlphaThreshold)
				{
					return Tile.Type.StartPoint;
				}
				else
				{
					return Tile.Type.EndPoint;
				}
			}

			return Tile.Type.Obstacle;
		}

		public override bool Equals(object obj) => base.Equals(obj);
		public override int GetHashCode() => base.GetHashCode();

		public static bool operator ==(ColorSettings one, ColorSettings other) => one.Equals(other);
		public static bool operator !=(ColorSettings one, ColorSettings other) => !(one == other);
	}

	[SerializeField] private Texture2D levelTexture = null;
	public Texture2D LevelTexture => levelTexture;

	[SerializeField] private bool overrideDefaultColorSettings = false;
	public bool HasCustomColorSettings => overrideDefaultColorSettings;

	[SerializeField] private ColorSettings customColorSettings = ColorSettings.Default;
	public ColorSettings CustomColorSettings => customColorSettings;

	public int Rows => LevelTexture.height;
	public int Cols => LevelTexture.width;
}
