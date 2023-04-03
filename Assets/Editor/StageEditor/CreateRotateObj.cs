using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class CStageEditor : EditorWindow
{
	private bool isCreateStart; // 回転オブジェクトを作成開始する
	GameObject _parentObject;   // 一番親のオブジェクト
	Rigidbody _parentRigdbody;  // 親のRigidbody
	RotatableObject _parentRotObj;  // 親のRotatableObject
	GameObject _object;         // 仲介のオブジェクト
	GameObject[] _childObject;  // 要素のオブジェクト
	Vector3 _rotobjpos;
	bool _isComponentSetting;
	void CreateRotateObjInitialize()
	{
		isCreateStart = false;
		_parentObject = null;
		_object = null;
	}

	void LayoutRotateObj()
	{
		if (isCreateStart)
		{
			// ヌルチェック
			if (!CheckNowCreateRotObj()) return;

			using (new GUILayout.HorizontalScope())
			{
				// 親子関係表示
				using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(prefabListScPos, EditorStyles.helpBox, GUILayout.Width(250)))
				{
					// タイトル
					GUILayout.Box("回転オブジェクト親子関係");
					Line();
					LayoutParentList();
				}
				// アタッチするオブジェクト項目
				using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(prefabListScPos, EditorStyles.helpBox, GUILayout.Width(250)))
				{
					// タイトル
					GUILayout.Box("オブジェクト項目");
					Line();
					LayoutObjectList();
				}
				// 詳細設定
				using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(previewRunScPos, EditorStyles.helpBox))
				{
					// タイトル
					GUILayout.Box("詳細設定");
					Line();
					LayoutSettings();
				}
			}
		}
		else
		{
			// オブジェクトの生成開始
			if (GUILayout.Button("オブジェクトの生成開始"))
			{
				isCreateStart = true;
				CreateBaseRotateObject();
			}
		}
	}

	private void LayoutParentList()
	{
		EditorGUILayout.LabelField(_parentObject.name);
		using (new EditorGUI.IndentLevelScope())
		{
			EditorGUILayout.LabelField(_object.name);
			using (new EditorGUI.IndentLevelScope())
			{
				var childNum = _object.gameObject.transform.childCount;
				for (int i = 0; i < childNum; i++)
				{
					EditorGUILayout.LabelField(_object.gameObject.transform.GetChild(childNum).name);
				}
			}
		}
	}
	private void LayoutObjectList()
	{

	}
	private void LayoutSettings()
	{
		// 親のコンポーネントの設定
		_isComponentSetting = EditorGUILayout.BeginFoldoutHeaderGroup(_isComponentSetting, "コンポーネントの設定");
		if (_isComponentSetting)
		{
			using (new GUILayout.VerticalScope("HelpBox"))
			{
				LayoutComponentSetting();
			}
		}
		EditorGUILayout.EndFoldoutHeaderGroup();

		if (GUILayout.Button("生成終了"))
		{
			isCreateStart = false;
		}
	}

	private void LayoutComponentSetting()
	{
		// オブジェクトの名前
		using (new GUILayout.HorizontalScope())
		{
			EditorGUILayout.LabelField("オブジェクト名");
			string name = _parentObject.name;
			name = EditorGUILayout.TextField(name);
			_parentObject.name = name;
		}

		EditorGUILayout.HelpBox("以下の項目はインスペクターでも変更できます。", MessageType.Info);
		// Rigidbodyの設定
		EditorGUILayout.LabelField("Rigidbbody");
		using (new EditorGUI.IndentLevelScope())
		{
			_parentRigdbody.mass = EditorGUILayout.FloatField("質量", _parentRigdbody.mass);
			_parentRigdbody.drag = EditorGUILayout.FloatField("抗力", _parentRigdbody.drag);
			_parentRigdbody.angularDrag = EditorGUILayout.FloatField("回転抗力", _parentRigdbody.angularDrag);
			_parentRigdbody.useGravity = EditorGUILayout.Toggle("重力使用", _parentRigdbody.useGravity);
			_parentRigdbody.isKinematic = EditorGUILayout.Toggle("Is Kinematic", _parentRigdbody.isKinematic);
		}

		// RotatableObjectの設定
		EditorGUILayout.LabelField("RotatableObject");
		using (new EditorGUI.IndentLevelScope())
		{
			_parentRotObj._isRotating = EditorGUILayout.ToggleLeft("isRotating", _parentRotObj._isRotating);
			_parentRotObj._isSpin = EditorGUILayout.ToggleLeft("isSpin", _parentRotObj._isSpin);
		}

	}

	private void CreateBaseRotateObject()
	{
		// オブジェクトの生成
		_parentObject = new GameObject("RotatableObject");
		// タグの設定
		_parentObject.tag = "RotateObject";
		// 必要コンポーネントのアタッチ
		// Rigidbodyの生成　初期設定
		_parentRigdbody = _parentObject.AddComponent<Rigidbody>();
		_parentRigdbody.angularDrag = 0;
		_parentRigdbody.isKinematic = true;
		// RotatableObjectの生成
		_parentRotObj = _parentObject.AddComponent<RotatableObject>();

		_object = new GameObject("Object");
		_object.gameObject.transform.parent = _parentObject.gameObject.transform;
	}

	private bool CheckNowCreateRotObj()
	{
		if (isCreateStart && _parentObject == null)
		{
			isCreateStart = false;
			return false;
		}
		return true;
	}

	private void Line()
	{
		var splitterRect = EditorGUILayout.GetControlRect(false, GUILayout.Height(1));
		splitterRect.x = 0;
		splitterRect.width = position.width - 100f;
		EditorGUI.DrawRect(splitterRect, Color.Lerp(Color.gray, Color.gray, 1.0f));
	}
}
