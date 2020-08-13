//========== Grimm - BossBrainCustomEditor.cs - 12/08/2020 ==========//
// Author:  Eric Brkic
// Purpose: 
//===================================================================//
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(YoinkBossBrain))]
public class BossBrainCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Boss Brain Editor Window"))
        {
            BossBrainEditorWindow.Open((YoinkBossBrain)target);
        }
    }
}