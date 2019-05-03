using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Room3
{
 [CreateAssetMenu(fileName = "TileSettings", menuName = "ScriptableObjests", order = 1)]
    public class TileSettings : ScriptableObject
    {
        public Sprite StartPointSprite;
        public Sprite EndPointSprite;
        public Sprite AccessibleSprite;
        public Sprite InAccessibleSprite;
        public Sprite HorizontalTubeSprite;
        public Sprite VerticalTubeSprite;
        public Sprite CornerEastToNorthTubeSprite;
        public Sprite CornerEastToSouthTubeSprite;
        public Sprite CornerWestToSouthTubeSprite;
        public Sprite CornerWestToNorthTubeSprite;
    }
}

