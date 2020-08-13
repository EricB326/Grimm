//========== Grimm - OnHitEffectCustomEditor.cs - 11/08/2020 ==========//
// Author:  Eric Brkic
// Purpose: 
//=====================================================================//
using UnityEngine;
using UnityEditor;

// Might use for single editors, allows for double-clicking to open the window.
//public class AssetHandler
//{
//    [OnOpenAsset()]
//    public static bool OpenEditor(int _instanceID, int _line)
//    {
//        OnHitEffects obj = EditorUtility.InstanceIDToObject(_instanceID) as OnHitEffects;
//
//        if (obj != null)
//        {
//            OnHitEffectEditorWindow.Open(obj);
//            return true;
//        }
//
//        return false;
//    }
//}

[CustomEditor(typeof(OnHitEffects))]
public class OnHitEffectCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor Window"))
        {
            OnHitEffectEditorWindow.Open((OnHitEffects)target);
        }
    }
}
