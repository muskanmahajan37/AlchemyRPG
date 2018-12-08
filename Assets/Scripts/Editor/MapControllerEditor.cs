using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapController))]
public class LevelScriptEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        MapController mc = (MapController)target;
        
        mc.width = EditorGUILayout.IntField("Width", mc.width);
        mc.height = EditorGUILayout.IntField("Height", mc.height);

        mc.debugMode = EditorGUILayout.Toggle("Dev Mode", mc.debugMode);
    }
}