using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Room3
{
    [Serializable]
    public class GridMapStruct
    {
        public Texture2D GridMap;
        public Color32[] TubeColours;
        public Color32 AccessibleColour;
        public Color32 InAccessibleColour;
    }
}
