using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OnHitEffectEditorWindow : ExtendedEditorWindow
{
    public static void Open(OnHitEffects _dataObject)
    {
        OnHitEffectEditorWindow window = GetWindow<OnHitEffectEditorWindow>("On Hit Effect Editor");
        window.serializedObject = new SerializedObject(_dataObject);
    }

    private void OnGUI()
    {
        serializedObject.Update();

        currentProperty = serializedObject.FindProperty("hitEffectList");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        DrawSidebar(currentProperty);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

            //DrawProperties(selectedProperty, true);
        if (selectedProperty != null)
            DrawSelectedPropertiesPanel();
        else
            EditorGUILayout.LabelField("Select an item from the list");

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        ApplyChanges();
    }


    bool displayAnimationList = false;
    bool displayParticleList = false;

    private void DrawSelectedPropertiesPanel()
    {
        currentProperty = selectedProperty;

        EditorGUILayout.BeginVertical("box");

        DrawField("name", true);
        DrawField("hitEffect", true);

        EditorGUILayout.BeginHorizontal("box");

        if (GUILayout.Button("Display Animation List", EditorStyles.toolbarButton))
        {
            displayAnimationList = !displayAnimationList;
            displayParticleList = false;
        }

        if (GUILayout.Button("Display Particle List", EditorStyles.toolbarButton))
        {
            displayAnimationList = false;
            displayParticleList = !displayParticleList;
        }

        EditorGUILayout.EndHorizontal();

        if (displayAnimationList)
        {
            EditorGUILayout.BeginVertical("box");
            DrawField("animationsToPlay", true);
            EditorGUILayout.EndVertical();
        }

        if (displayParticleList)
        {
            EditorGUILayout.BeginVertical("box");
            DrawField("particlesToPlay", true);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }
}
