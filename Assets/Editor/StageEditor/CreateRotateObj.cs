using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class CStageEditor : EditorWindow
{
	private Vector2 ScPos;          // バー座標
	private bool isCreateStart;     // 回転オブジェクトを作成開始する
	GameObject _parentObject;       // 一番親のオブジェクト
	Rigidbody _parentRigdbody;      // 親のRigidbody
	RotatableObject _parentRotObj;  // 親のRotatableObject
	GameObject _object;             // 仲介のオブジェクト
	GameObject[] _childrenObject;   // 要素のオブジェクト
	GameObject _selectChildObj;     // 選択している子オブジェクト
	GameObject _selectAddChildPrefab;// 選択している子オブジェクト
	Vector3 _rotobjpos;
	Vector3 _childpos;
	bool _isBaseSettings;
	bool _isAddChildObj;
	bool _isChildrenSettings;
	int _selectChildID;
	void CreateRotateObjInitialize()
	{
		isCreateStart = false;
		_parentObject = null;
		_object = null;
		_selectAddChildPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Stage/Pf_Parts.prefab");
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
				using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(ScPos, EditorStyles.helpBox, GUILayout.Width(250)))
				{
					// タイトル
					GUILayout.Box("回転オブジェクト親子関係");
					Line();
					LayoutParentList();
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
		// スタイルの設定
		GUIStyleState style = new GUIStyleState()
		{
			textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f),
		};
		GUIStyle centerbold = new GUIStyle()
		{
			fontStyle = FontStyle.Bold,
			normal = style,
		};

		EditorGUILayout.LabelField(_parentObject.name);
		using (new EditorGUI.IndentLevelScope())
		{
			var childNum = _object.gameObject.transform.childCount;
			EditorGUILayout.LabelField(_object.name + "　(" + childNum + ")");
			using (new EditorGUI.IndentLevelScope())
			{
				if(childNum == 0) return;
				for (int i = 0; i < childNum; i++)
				{
					var child = _object.gameObject.transform.GetChild(i);
					if (_selectChildID != i)
					{
						EditorGUILayout.LabelField(child.name);
					}
					else
					{
						EditorGUILayout.LabelField(child.name, centerbold);
					}

				}
			}
		}

		// キー入力の検出と処理
		if (Event.current.type == EventType.KeyDown)
		{
			if (Event.current.keyCode == KeyCode.UpArrow)
			{
				_selectChildID--;
				if(_selectChildID == -1) _selectChildID = _object.gameObject.transform.childCount - 1;
				Event.current.Use();
			}
			else if (Event.current.keyCode == KeyCode.DownArrow)
			{
				_selectChildID++;
				if(_selectChildID == _object.gameObject.transform.childCount) _selectChildID = 0;
				Event.current.Use();
			}
			_selectChildObj = _object.gameObject.transform.GetChild(_selectChildID).gameObject;
		}
	}

	private void LayoutSettings()
	{
		// 親の基本設定
		_isBaseSettings = EditorGUILayout.BeginFoldoutHeaderGroup(_isBaseSettings, "基本設定");
		if (_isBaseSettings)
		{
			using (new GUILayout.VerticalScope("HelpBox"))
			{
				LayoutBaseSettings();
			}
		}
		EditorGUILayout.EndFoldoutHeaderGroup();
		// 親の基本設定
		_isAddChildObj = EditorGUILayout.BeginFoldoutHeaderGroup(_isAddChildObj, "子の追加");
		if (_isAddChildObj)
		{
			using (new GUILayout.VerticalScope("HelpBox"))
			{
				LayoutAddChildObj();
			}
		}
		EditorGUILayout.EndFoldoutHeaderGroup();

		// 子の基本設定
		_isChildrenSettings = EditorGUILayout.BeginFoldoutHeaderGroup(_isChildrenSettings, "子の設定");
		if (_isChildrenSettings)
		{
			using (new GUILayout.VerticalScope("HelpBox"))
			{
				LayoutChildrenSettings();
			}
		}
		EditorGUILayout.EndFoldoutHeaderGroup();

		if (GUILayout.Button("生成終了"))
		{
			isCreateStart = false;
		}
	}

	private void LayoutBaseSettings()
	{
		// スタイルの設定
		GUIStyleState style = new GUIStyleState()
		{
			textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f),
		};
		GUIStyle centerbold = new GUIStyle()
		{
			fontStyle = FontStyle.Bold,
			normal = style,
		};

		// オブジェクトの名前
		using (new GUILayout.HorizontalScope())
		{
			EditorGUILayout.LabelField("オブジェクト名", centerbold);
			string name = _parentObject.name;
			name = EditorGUILayout.TextField(name);
			_parentObject.name = name;
		}

		// オブジェクトの座標
		using (new GUILayout.HorizontalScope())
		{
			EditorGUILayout.LabelField("座標", centerbold);
			_parentObject.transform.position = EditorGUILayout.Vector3Field("", _parentObject.transform.position);
		}

		// Rigidbodyの設定
		EditorGUILayout.LabelField("Rigidbbody", centerbold);
		using (new EditorGUI.IndentLevelScope())
		{
			_parentRigdbody.mass = EditorGUILayout.FloatField("質量", _parentRigdbody.mass);
			_parentRigdbody.drag = EditorGUILayout.FloatField("抗力", _parentRigdbody.drag);
			_parentRigdbody.angularDrag = EditorGUILayout.FloatField("回転抗力", _parentRigdbody.angularDrag);
			_parentRigdbody.useGravity = EditorGUILayout.Toggle("重力使用", _parentRigdbody.useGravity);
			_parentRigdbody.isKinematic = EditorGUILayout.Toggle("Is Kinematic", _parentRigdbody.isKinematic);
		}

		// RotatableObjectの設定
		EditorGUILayout.LabelField("RotatableObject", centerbold);
		using (new EditorGUI.IndentLevelScope())
		{
			_parentRotObj._isRotating = EditorGUILayout.ToggleLeft("isRotating", _parentRotObj._isRotating);
			_parentRotObj._isSpin = EditorGUILayout.ToggleLeft("isSpin", _parentRotObj._isSpin);
		}
	}

	private void LayoutAddChildObj()
	{
		_selectAddChildPrefab = EditorGUILayout.ObjectField("追加オブジェクト", _selectAddChildPrefab, typeof(GameObject), false) as GameObject;
		_childpos = EditorGUILayout.Vector3Field("座標", _childpos);
		if (GUILayout.Button("追加"))
		{
			var tmpObj = Instantiate(_selectAddChildPrefab, _childpos, Quaternion.identity);
			tmpObj.gameObject.transform.parent = _object.gameObject.transform;
			tmpObj.name = tmpObj.name.Replace("(Clone)", "");
			_selectChildObj = tmpObj;
			_selectChildID = _object.gameObject.transform.childCount - 1;
		}
	}

	private void LayoutChildrenSettings()
	{
		// スタイルの設定
		GUIStyleState style = new GUIStyleState()
		{
			textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f),
		};
		GUIStyle centerbold = new GUIStyle()
		{
			fontStyle = FontStyle.Bold,
			normal = style,
		};
		// ヌルチェック
		if (_selectChildObj == null)
		{
			EditorGUILayout.HelpBox("子オブジェクトが選択されていません", MessageType.Warning);
			return;
		}

		// オブジェクトの名前
		using (new GUILayout.HorizontalScope())
		{
			EditorGUILayout.LabelField("オブジェクト名", centerbold);
			string name = _selectChildObj.name;
			name = EditorGUILayout.TextField(name);
			_selectChildObj.name = name;
		}

		// オブジェクトの座標
		using (new GUILayout.HorizontalScope())
		{
			EditorGUILayout.LabelField("座標", centerbold);
			_selectChildObj.transform.position = EditorGUILayout.Vector3Field("", _selectChildObj.transform.position);
		}

		if (GUILayout.Button("削除"))
		{
			DestroyImmediate(_selectChildObj);
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
