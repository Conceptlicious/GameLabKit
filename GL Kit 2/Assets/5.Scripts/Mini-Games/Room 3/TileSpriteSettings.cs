using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpriteSettings", menuName = "Room 3/SpriteSettings")]
public class TileSpriteSettings : ScriptableObject
{
	public Sprite TubeWestToEast;
	public Sprite TubeNorthToSouth;
	public Sprite TubeWestToSouth;
	public Sprite TubeEastToSouth;
	public Sprite TubeWestToNorth;
	public Sprite TubeEastToNorth;
}
