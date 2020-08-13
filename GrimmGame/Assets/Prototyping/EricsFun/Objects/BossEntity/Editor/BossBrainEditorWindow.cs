//========== Grimm - BossBrainEditorWindow.cs - 12/08/2020 ==========//
// Author:  Eric Brkic
// Purpose: 
//===================================================================//
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

public class BossBrainEditorWindow : ExtendedEditorWindow
{
    private bool inBasePanel = true;
    private bool inPhasePanel = false;

    public static void Open(YoinkBossBrain _dataObject)
    {
        BossBrainEditorWindow window = CreateInstance<BossBrainEditorWindow>();
        Texture windowIcon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Prototyping/EricsFun/Objects/BossEntity/Editor/bwain.png");
        GUIContent windowTitleContent = new GUIContent("Boss Brain Editor", windowIcon);

        window.titleContent = windowTitleContent;
        window.serializedObject = new SerializedObject(_dataObject);
        window.Show();

    }

    // Base Panel
    private void OnGUI()
    {
        serializedObject.Update();

        currentProperty = serializedObject.FindProperty("YoinkBossBrain");

        EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

                if (inBasePanel)
                    DrawBaseGUIPanel();

                if (inPhasePanel)
                    DrawPhaseGUIPanel();

            EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        ApplyChanges();
    }
    bool showDebugVariables = false;
    private void DrawBaseGUIPanel()
    {
        inBasePanel = true;
        inPhasePanel = false;

        currentProperty = serializedObject.FindProperty("YoinkBossBrain");

        EditorGUILayout.BeginVertical("box");

            DrawField("m_rotationSpeed", true);

            showDebugVariables = EditorGUILayout.ToggleLeft("Show Debug Variables. **NOT TO BE MODIFIED!**", showDebugVariables);
            if (showDebugVariables)
            { 
                DrawField("m_desiredRange", true);
                DrawField("m_target", true);
                DrawField("m_currentAttackVariables", true);
                DrawField("m_currentAction", true);
                DrawField("m_currentPhase", true);
                DrawField("m_revengeValue", true);
            }


            EditorGUILayout.BeginHorizontal("box");

                if (GUILayout.Button("Display Phase List", EditorStyles.toolbarButton))
                {
                    inBasePanel = false;
                    inPhasePanel = true;
                }

            EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private bool displayAttackList = false;

    private void DrawPhaseGUIPanel()
    {
        inBasePanel = false;
        inPhasePanel = true;

        currentProperty = serializedObject.FindProperty("m_bossPhaseList");

        EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

                DrawSidebar(currentProperty);

                EditorGUILayout.BeginHorizontal("box");

                    if (GUILayout.Button("Return to the Base Panel", EditorStyles.toolbarButton))
                    {
                        inBasePanel = true;
                        inPhasePanel = false;
                    }

                EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

                if (selectedProperty != null)
                {
                    currentProperty = selectedProperty;

                    DrawField("m_name", true);
                    DrawField("m_increase", true);
                    DrawField("m_decrease", true);
                    DrawField("m_threshold", true);

                    EditorGUILayout.BeginHorizontal("box");

                        if (GUILayout.Button("Display Attack List for this Phase", EditorStyles.toolbarButton))
                        {
                            displayAttackList = !displayAttackList;
                        }

                    EditorGUILayout.EndHorizontal();

                     if (displayAttackList)
                     {
                         EditorGUILayout.BeginVertical("box");
                            DrawField("m_phaseAttacks", true);
                         EditorGUILayout.EndVertical();
                     }
                 }
                else
                    EditorGUILayout.LabelField("Select/Add a phase from the list.");

            EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    //private void DrawAttackGUIPanel()
    //{
    //    inBasePanel = false;
    //    inPhasePanel = false;
    //    inAttackPanel = true;

    //    currentProperty = serializedObject.FindProperty("m_phaseAttacks");

    //    EditorGUILayout.BeginHorizontal();

    //        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

    //            DrawSidebar(currentProperty);

    //            EditorGUILayout.BeginHorizontal("box");

    //                if (GUILayout.Button("Return to the Phase Panel", EditorStyles.toolbarButton))
    //                {
    //                    inBasePanel = false;
    //                    inPhasePanel = true;
    //                    inAttackPanel = false;
    //                }

    //            EditorGUILayout.EndHorizontal();

    //        EditorGUILayout.EndVertical();

    //        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

    //            if (selectedProperty != null)
    //            {
    //                currentProperty = selectedProperty;

    //                DrawField("attackName", true);
    //                DrawField("animationNumber", true);
    //                DrawField("attackDamage", true);
    //                DrawField("attackRange", true);
    //            }
    //            else
    //                EditorGUILayout.LabelField("Select/Add an attack from the list for this phase.");

    //        EditorGUILayout.EndVertical();

    //    EditorGUILayout.EndHorizontal();
    //}
}
