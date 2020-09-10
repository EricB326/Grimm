//========== Grimm - BossBrainEditorWindow.cs - 12/08/2020 ==========//
// Author:  Eric Brkic
// Purpose: 
//===================================================================//
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

// Sorry to sully your script Eric.
// Script needs to be updated whenver "BossPhase" or "BossBrain" is edited otherwise breakage.


public class BossBrainEditorWindow : ExtendedEditorWindow
{
    private bool inBasePanel = true;
    private bool inPhasePanel = false;
    private static BossBrain script;

    public static void Open(BossBrain _dataObject)
    {
        script = _dataObject;
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

        currentProperty = serializedObject.FindProperty("BossBrain");

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
    bool displayFirstDiagnostic = false;
    bool displaySecondDiagnostic = false;
    bool displayThirdDiagnostic = false;
    private void DrawBaseGUIPanel()
    {
        inBasePanel = true;
        inPhasePanel = false;

        currentProperty = serializedObject.FindProperty("BossBrain");

        EditorGUILayout.BeginVertical("box");

            DrawField("m_rotationSpeed", true);

            showDebugVariables = EditorGUILayout.ToggleLeft("Show Debug Variables. **NOT TO BE MODIFIED!**", showDebugVariables);
            if (showDebugVariables)
            {
                script.m_diagnosticMode = EditorGUILayout.Toggle("Diagnostic Mode", script.m_diagnosticMode);
                if (script.m_diagnosticMode)
                {
                    EditorGUILayout.BeginHorizontal("box");

                        if (GUILayout.Button("Diagnostic One", EditorStyles.toolbarButton))
                        {
                            displayFirstDiagnostic = !displayFirstDiagnostic;
                        }

                        if (GUILayout.Button("Diagnostic Two", EditorStyles.toolbarButton))
                        {
                            displaySecondDiagnostic = !displaySecondDiagnostic;
                        }

                        if (GUILayout.Button("Diagnostic Three", EditorStyles.toolbarButton))
                        {
                            displayThirdDiagnostic = !displayThirdDiagnostic;
                        }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal("box");

                        if (displayFirstDiagnostic)
                        {
                            GUILayout.TextArea("test");
                        }

                        if (displaySecondDiagnostic)
                        {
                            GUILayout.TextArea("test2");
                        }

                        if (displayThirdDiagnostic)
                        {
                            GUILayout.TextArea("test3");
                        }

                    EditorGUILayout.EndHorizontal();
                }

                /****************************************************************************************************************************
                // THIS IS WHERE YOU PLACE THE DEBUG VARIABLES.
                // DrawField("variableName", true); "variableName" MUST be ONE to ONE with the name of the variable in the BossBrain script!
                // ONLY PUBLIC & [SerializeField] PRIVATE VARAIBLES CAN BE DISPLAYED! YOU WILL GET AN ERROR TRYING TO DISPLAY A PRIVATE VAR!
                ***************************************************************************************************************************/
                DrawField("m_desiredRange", true);
                DrawField("m_target", true);
                DrawField("m_currentPhase", true);
                DrawField("m_revengeValue", true);
                //DrawField("m_actionQue", true);
                DrawField("m_baseActions", true);
                DrawField("m_timeOutSeek", true);
                DrawField("m_timeOutAttack", true);
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
                    //DrawField("m_increase", true);
                    //DrawField("m_decrease", true);
                    //DrawField("m_threshold", true);
                    //DrawField("m_wanderTime", true);
                    //DrawField("m_timeBetweenPreDefinedAndRandom", true);
                    //DrawField("m_preDefinedActions", true);
                    //DrawField("m_counterMoves", true);
                    DrawField("m_timeBetweenMovementAction", true);


                    EditorGUILayout.BeginHorizontal("box");

                    if (GUILayout.Button("Display Action List for this Phase", EditorStyles.toolbarButton))
                    {
                        displayAttackList = !displayAttackList;
                    }

                    EditorGUILayout.EndHorizontal();

                    if (displayAttackList)
                    {
                        EditorGUILayout.BeginVertical("box");
                        DrawField("m_bossActions", true);
                        EditorGUILayout.EndVertical();
                    }
                }
                else
                    EditorGUILayout.LabelField("Select/Add a phase from the list.");

            EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }
}
