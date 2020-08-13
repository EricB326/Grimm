//========== Grimm - ExtendedEditorWindow.cs - 11/08/2020 ==========//
// Author:  Eric Brkic
// Purpose: 
//==================================================================//
using UnityEngine;
using UnityEditor;

public class ExtendedEditorWindow : EditorWindow
{
	protected SerializedObject serializedObject;
	protected SerializedProperty currentProperty;

	private string selectedPropertyPath;
	protected SerializedProperty selectedProperty;

	protected void DrawProperties(SerializedProperty _property, bool _drawChildren)
	{
		string lastPropertyPath = string.Empty;

		foreach (SerializedProperty thisProperty in _property)
		{
			if (thisProperty.isArray && thisProperty.propertyType == SerializedPropertyType.Generic)
			{
				EditorGUILayout.BeginHorizontal();
				thisProperty.isExpanded = EditorGUILayout.Foldout(thisProperty.isExpanded, thisProperty.displayName);
				EditorGUILayout.EndHorizontal();

				if (thisProperty.isExpanded)
				{
					EditorGUI.indentLevel++;
					DrawProperties(thisProperty, _drawChildren);
					EditorGUI.indentLevel--;
				}
			}
			else 
			{
				if (!string.IsNullOrEmpty(lastPropertyPath) && thisProperty.propertyPath.Contains(lastPropertyPath))
					continue;

				lastPropertyPath = thisProperty.propertyPath;
				EditorGUILayout.PropertyField(thisProperty, _drawChildren);
			}
		}
	}

	protected void DrawSidebar(SerializedProperty _property)
	{
		foreach (SerializedProperty thisProperty in _property)
		{
			if (GUILayout.Button(thisProperty.displayName))
			{
				selectedPropertyPath = thisProperty.propertyPath;
			}
		}

		if (GUILayout.Button("Add Element"))
		{
			_property.InsertArrayElementAtIndex(_property.arraySize);
		}
		if (GUILayout.Button("Remove Element") && _property.arraySize > 0)
		{
			_property.DeleteArrayElementAtIndex(_property.arraySize - 1);
		}

		if (!string.IsNullOrEmpty(selectedPropertyPath))
		{
			selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
		}
	}

	protected void DrawField(string _propertyName, bool _isRelative)
	{
		if (_isRelative && currentProperty != null)
			EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative(_propertyName), true);
		else if (serializedObject != null)
			EditorGUILayout.PropertyField(serializedObject.FindProperty(_propertyName), true);
	}

	protected void ApplyChanges()
	{
		serializedObject.ApplyModifiedProperties();
	}
}
