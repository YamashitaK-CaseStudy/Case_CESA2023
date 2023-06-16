using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Lift))]
public class LiftInspector : Editor {

    SerializedProperty _liftDirection;
    SerializedProperty _liftStickobj;
    SerializedProperty _liftRideobj;
    SerializedProperty _lineLength;
    SerializedProperty _rideBlockIndex;

    void OnEnable() {

        _liftDirection = serializedObject.FindProperty("_liftDirection");
        _liftStickobj = serializedObject.FindProperty("_liftStickobj");
        _liftRideobj = serializedObject.FindProperty("_liftRideobj");
        _lineLength = serializedObject.FindProperty("_stickLength");
        _rideBlockIndex = serializedObject.FindProperty("_rideBlockIndex");
    }

    [System.Obsolete]
    public override void OnInspectorGUI() {

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_liftDirection, new GUIContent("���t�g�𓮂�������"));
        EditorGUILayout.PropertyField(_liftStickobj, new GUIContent("���t�g�𓮂����_�̃��f��"));
        EditorGUILayout.PropertyField(_liftRideobj, new GUIContent("���t�g�ɏ�郂�f��"));
        
        if(_liftDirection.enumValueIndex == 0) {
            EditorGUILayout.PropertyField(_lineLength, new GUIContent("�㑤�ɒǉ����Ă����_�̒���"));
        }
        else {
            EditorGUILayout.PropertyField(_lineLength, new GUIContent("�E���ɒǉ����Ă����_�̒���"));
        }

        EditorGUILayout.PropertyField(_rideBlockIndex, new GUIContent("���t�g�̏��ʒu(�_�̗v�f���Ŏw��)"));

        // �ݒ�̊m��
        serializedObject.ApplyModifiedProperties();
  
        var lift = serializedObject.targetObject as Lift;

        if (GUILayout.Button("Apply")) {
            lift.ApplyInspector();
        }
    }
}
