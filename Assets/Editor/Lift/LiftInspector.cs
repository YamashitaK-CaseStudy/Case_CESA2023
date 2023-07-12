using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Lift))]
public class LiftInspector : Editor {

    SerializedProperty _liftDirection;
    SerializedProperty _liftStickVertickalobj;
    SerializedProperty _liftStickHorizontalobj;
    SerializedProperty _liftRideobj;
    SerializedProperty _liftDameobj;
    SerializedProperty _liftObjectobj;
    SerializedProperty _lineLength;
    SerializedProperty _rideBlockIndex;
    SerializedProperty _liftMove;
   
    void OnEnable() {

        _liftDirection = serializedObject.FindProperty("_liftDirection");
        _liftStickVertickalobj = serializedObject.FindProperty("_liftStickVertickalobj");
        _liftStickHorizontalobj = serializedObject.FindProperty("_liftStickHorizontalobj");
        _liftRideobj = serializedObject.FindProperty("_liftRideobj");
        _liftDameobj = serializedObject.FindProperty("_liftDameobj");
        _liftObjectobj = serializedObject.FindProperty("_liftObjectobj");
        _lineLength = serializedObject.FindProperty("_stickLength");
        _rideBlockIndex = serializedObject.FindProperty("_rideBlockIndex");
        _liftMove = serializedObject.FindProperty("_liftMove");
    }

    [System.Obsolete]
    public override void OnInspectorGUI() {

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_liftDirection, new GUIContent("リフトを動かす向き"));
        EditorGUILayout.PropertyField(_liftStickVertickalobj, new GUIContent("リフトを動かす棒のモデル縦"));
        EditorGUILayout.PropertyField(_liftStickHorizontalobj, new GUIContent("リフトを動かす棒のモデル横"));
        EditorGUILayout.PropertyField(_liftRideobj, new GUIContent("リフトに乗るモデル"));
        EditorGUILayout.PropertyField(_liftDameobj, new GUIContent("ダミーオブジェクト"));
        EditorGUILayout.PropertyField(_liftObjectobj, new GUIContent("オブジェクト"));
        
        if(_liftDirection.enumValueIndex == 0) {
            EditorGUILayout.PropertyField(_lineLength, new GUIContent("上側に追加していく棒の長さ"));
        }
        else {
            EditorGUILayout.PropertyField(_lineLength, new GUIContent("右側に追加していく棒の長さ"));
        }

        EditorGUILayout.PropertyField(_rideBlockIndex, new GUIContent("リフトの乗る位置(棒の要素数で指定)"));
        EditorGUILayout.PropertyField(_liftMove, new GUIContent("リフトの昇降"));
      
        // 設定の確定
        serializedObject.ApplyModifiedProperties();
  
        var lift = serializedObject.targetObject as Lift;

        if (GUILayout.Button("Apply")) {
            lift.ApplyInspector();
        }
    }
}
