//========== Grimm - OnHitEffectEditorWindow.cs - 11/08/2020 ==========//
// Author:  Eric Brkic
// Purpose: To display a custom editor window for the OnHitEffects 
//          script.
//=====================================================================//
using UnityEditor;
using UnityEngine;

public class OnHitEffectEditorWindow : ExtendedEditorWindow
{
    /* @brief Opens the editor window for the On Hit Effects object.
     * @param The script that will be read from for properties (OnHitEffects).
    */
    public static void Open(OnHitEffects _dataObject)
    {
        OnHitEffectEditorWindow window = GetWindow<OnHitEffectEditorWindow>("On Hit Effect Editor");
        window.serializedObject = new SerializedObject(_dataObject);
    }

    /* @brief On the opening of the window, display UI based on the script properties.
    */
    private void OnGUI()
    {
        // Reads and copies the data into an internal structure (SerializedProperty).
        serializedObject.Update();

        // Set the current property to the effect list within the OnHitEffect script.
        currentProperty = serializedObject.FindProperty("hitEffectList");

        EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

                // Draw a sidebar on the GUI.
                DrawSidebar(currentProperty);

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

                // If 
                if (selectedProperty != null)
                    DrawSelectedPropertiesPanel();
                else
                    EditorGUILayout.LabelField("Select/Add an item from the list");

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
