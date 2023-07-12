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

        EditorGUILayout.PropertyField(_liftDirection, new GUIContent("���t�g�𓮂�������"));
        EditorGUILayout.PropertyField(_liftStickVertickalobj, new GUIContent("���t�g�𓮂����_�̃��f���c"));
        EditorGUILayout.PropertyField(_liftStickHorizontalobj, new GUIContent("���t�g�𓮂����_�̃��f����"));
        EditorGUILayout.PropertyField(_liftRideobj, new GUIContent("���t�g�ɏ�郂�f��"));
        EditorGUILayout.PropertyField(_liftDameobj, new GUIContent("�_�~�[�I�u�W�F�N�g"));
        EditorGUILayout.PropertyField(_liftObjectobj, new GUIContent("�I�u�W�F�N�g"));
        
        if(_liftDirection.enumValueIndex == 0) {
            EditorGUILayout.PropertyField(_lineLength, new GUIContent("�㑤�ɒǉ����Ă����_�̒���"));
        }
        else {
            EditorGUILayout.PropertyField(_lineLength, new GUIContent("�E���ɒǉ����Ă����_�̒���"));
        }

        EditorGUILayout.PropertyField(_rideBlockIndex, new GUIContent("���t�g�̏��ʒu(�_�̗v�f���Ŏw��)"));
        EditorGUILayout.PropertyField(_liftMove, new GUIContent("���t�g�̏��~"));
      
        // �ݒ�̊m��
        serializedObject.ApplyModifiedProperties();
  
        var lift = serializedObject.targetObject as Lift;

        if (GUILayout.Button("Apply")) {
            lift.ApplyInspector();
        }
    }
}
