﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

// Might use for single editors, allows for double-clicking to open the window.
//public class AssetHandler
//{
//    [OnOpenAsset()]
//    public static bool OpenEditor(int _instanceID, int _line)
//    {
//        OnHitEffects obj = EditorUtility.InstanceIDToObject(_instanceID) as OnHitEffects;

//        if (obj != null)
//        {
//            OnHitEffectEditorWindow.Open(obj);
//            return true;
//        }

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
