using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bolt))]
public class BoltInspector : Editor
{
    SerializedProperty _threadObject;
    SerializedProperty _length;
    SerializedProperty _translationLimit;
    SerializedProperty _translationPerRotation;
    SerializedProperty _spinningTranslationSpeed;
    SerializedProperty _interlockingObjectList;

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        _threadObject = serializedObject.FindProperty("_threadObject");
        _length = serializedObject.FindProperty("_length");
        _translationLimit = serializedObject.FindProperty("_translationLimit");
        _translationPerRotation = serializedObject.FindProperty("_translationPerRotation");
        _spinningTranslationSpeed = serializedObject.FindProperty("_spinningTranslationSpeed");
        _interlockingObjectList = serializedObject.FindProperty("_interlockingObjectList");
    }

    public override void OnInspectorGUI()
    {

        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        EditorGUILayout.PropertyField(_threadObject, new GUIContent("�l�W�̕����f��"));
        EditorGUILayout.PropertyField(_length, new GUIContent("�{���g�̒���"));
        EditorGUILayout.PropertyField(_translationLimit, new GUIContent("�ő�ړ���"));
        EditorGUILayout.PropertyField(_translationPerRotation, new GUIContent("�P��]������̈ړ���"));
        EditorGUILayout.PropertyField(_spinningTranslationSpeed, new GUIContent("������]���̃X�s�[�h"));
        EditorGUILayout.PropertyField(_interlockingObjectList, new GUIContent("�A������I�u�W�F�N�g�̃��X�g"));

        //Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();

        Bolt bolt = serializedObject.targetObject as Bolt;
        if (bolt == null)
        {
            Debug.Log("Bolt�ɕϊ��ł��Ȃ�����");
        }
        else
        {
            bolt.ApplyInspector();
        }
    }
}
