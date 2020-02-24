using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if(DrawDefaultInspector())
        {
            if (mapGen.autoUpdateMap)
            {
                mapGen.DrawMapInEditor();
            }
        }

        if(GUILayout.Button("Generate Map"))
        {
            mapGen.DrawMapInEditor();
        }
    }
}
