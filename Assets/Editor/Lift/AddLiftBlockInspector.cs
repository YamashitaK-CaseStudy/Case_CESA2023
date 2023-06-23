using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddLiftBlock))]

//複数選択有効
[CanEditMultipleObjects]

public class AddLiftBlockInspector : Editor
{
 
    SerializedProperty _addRightblockNum;
    SerializedProperty _addLefttblockNum;
    SerializedProperty _addUpblockNum;
    SerializedProperty _addDownblockNum;

  
    void OnEnable() {

        _addRightblockNum = serializedObject.FindProperty("_addRightBlockNum");
        _addLefttblockNum = serializedObject.FindProperty("_addLeftBlockNum");
        _addUpblockNum = serializedObject.FindProperty("_addUpBlockNum");
        _addDownblockNum = serializedObject.FindProperty("_addDownBlockNum");
    }

    [System.Obsolete]
    public override void OnInspectorGUI() {

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_addRightblockNum, new GUIContent("右に追加する数"));
        EditorGUILayout.PropertyField(_addLefttblockNum, new GUIContent("左に追加する数"));
        EditorGUILayout.PropertyField(_addUpblockNum, new GUIContent("上に追加する数"));
        EditorGUILayout.PropertyField(_addDownblockNum, new GUIContent("下に追加する数"));

        // 設定の確定
        serializedObject.ApplyModifiedProperties();

        var addLiftBlock = serializedObject.targetObject as AddLiftBlock;

        if (GUILayout.Button("Apply")) {

            addLiftBlock.ApplyInspector();
        }

        if (GUILayout.Button("Delete")) {
            foreach (Object obj in serializedObject.targetObjects) {

               var addcomp = obj as AddLiftBlock;
                addcomp.BlockDesroy();
            }
        }  
    }
}
