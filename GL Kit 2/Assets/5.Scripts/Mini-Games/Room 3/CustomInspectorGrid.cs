using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Room3
{
    [CustomEditor(typeof(Grid))]
    public class CustomInspectorGrid : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button(("Next Grid")))
            {
                Grid gridReference = (Grid)target;
                gridReference.NextGrid();
            }

        }
    }
}
