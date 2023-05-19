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
        EditorGUILayout.PropertyField(_threadObject, new GUIContent("ネジの部モデル"));
        EditorGUILayout.PropertyField(_length, new GUIContent("ボルトの長さ"));
        EditorGUILayout.PropertyField(_translationLimit, new GUIContent("最大移動量"));
        EditorGUILayout.PropertyField(_translationPerRotation, new GUIContent("１回転あたりの移動量"));
        EditorGUILayout.PropertyField(_spinningTranslationSpeed, new GUIContent("高速回転時のスピード"));
        EditorGUILayout.PropertyField(_interlockingObjectList, new GUIContent("連動するオブジェクトのリスト"));

        //Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();

        Bolt bolt = serializedObject.targetObject as Bolt;
        if (bolt == null)
        {
            Debug.Log("Boltに変換できなかった");
        }
        else
        {
            bolt.ApplyInspector();
        }
    }
}
