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
	RotObjkinds _parentObjectkind;  // 親のRotObjKinds
	RotObjkinds.ObjectKind _kinds;
	GameObject _object;             // 仲介のオブジェクト
	GameObject _selectChildObj;     // 選択している子オブジェクト
	GameObject _selectAddChildPrefab;// 選択している子オブジェクト
	Vector3 _rotobjpos;
	Vector3 _childpos;
	bool _isBaseSettings;
	bool _isAddChildObj;
	bool _isChildrenSettings;
	bool _isLineX;
	bool _isLineY;
	bool _isLineZ;
	bool _isBoxZ;
	int _selectChildID;
	int _length;
	enum CREATETYPE
	{
		normal,
		line,
		box,
		stairs,
	}
	CREATETYPE _type = CREATETYPE.normal;
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
			_kinds = (RotObjkinds.ObjectKind)EditorGUILayout.EnumPopup("オブジェクトの種類", _kinds);
			// オブジェクトの生成開始
			if (GUILayout.Button("オブジェクトの生成開始"))
			{
				isCreateStart = true;
				CreateBaseRotateObject();
				string path = null;
				if (_kinds == RotObjkinds.ObjectKind.UnionRotObject)
				{
					path = "Assets/Prefabs/Stage/Pf_PartsUnionRed.prefab";
				}
				else
				{
					path = "Assets/Prefabs/Stage/Pf_Parts.prefab";
				}

				_selectAddChildPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
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
				if (childNum == 0) return;
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
				if (_selectChildID == -1) _selectChildID = _object.gameObject.transform.childCount - 1;
				Event.current.Use();
			}
			else if (Event.current.keyCode == KeyCode.DownArrow)
			{
				_selectChildID++;
				if (_selectChildID == _object.gameObject.transform.childCount) _selectChildID = 0;
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
			_selectChildObj = null;
			_childpos = new Vector3(0,0,0);
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
			_parentRotObj._rotRequirdTime = EditorGUILayout.FloatField("回転速度", _parentRotObj._rotRequirdTime);
		}
	}

	private void LayoutAddChildObj()
	{
		_selectAddChildPrefab = EditorGUILayout.ObjectField("追加オブジェクト", _selectAddChildPrefab, typeof(GameObject), false) as GameObject;
		_childpos = EditorGUILayout.Vector3Field("座標", _childpos);
		_type = (CREATETYPE)EditorGUILayout.EnumPopup("生成タイプ", _type);

		using (new GUILayout.VerticalScope("HelpBox"))
		{
			switch (_type)
			{
				case CREATETYPE.normal:
					break;
				case CREATETYPE.line:
					_length = EditorGUILayout.IntField("長さ", _length);
					_isLineX = EditorGUILayout.ToggleLeft("X", _isLineX);
					_isLineY = EditorGUILayout.ToggleLeft("Y", _isLineY);
					_isLineZ = EditorGUILayout.ToggleLeft("Z", _isLineZ);
					break;
				case CREATETYPE.box:
					_length = EditorGUILayout.IntField("1辺の長さ", _length);
					_isBoxZ = EditorGUILayout.ToggleLeft("奥行も設定するか", _isBoxZ);
					break;
				case CREATETYPE.stairs:
					_length = EditorGUILayout.IntField("1辺の長さ", _length);
					using (new GUILayout.HorizontalScope())
					{
						_tb = (STAIRSDIRTB)EditorGUILayout.EnumPopup("上下", _tb);
						_lr = (STAIRSDIRLR)EditorGUILayout.EnumPopup("左右", _lr);
					}
					break;
			}
		}
		if (GUILayout.Button("追加"))
		{
			switch (_type)
			{
				case CREATETYPE.normal:
					AddChildObj(_childpos);
					break;
				case CREATETYPE.line:
					if (_isLineX)
					{

						for (int i = 0; i < _length; i++)
						{
							Vector3 pos = _childpos;
							pos.x += i;
							AddChildObj(pos);
						}
					}
					if (_isLineY)
					{
						for (int i = 1; i < _length; i++)
						{
							Vector3 pos = _childpos;
							pos.y += i;
							AddChildObj(pos);
						}
					}
					if (_isLineZ)
					{
						for (int i = 1; i < _length; i++)
						{
							Vector3 pos = _childpos;
							pos.z += i;
							AddChildObj(pos);
						}
					}
					break;
				case CREATETYPE.box:
					for (int z = 0; z < _length; z++)
					{
						for (int y = 0; y < _length; y++)
						{
							for (int x = 0; x < _length; x++)
							{
								Vector3 pos = _childpos;
								pos.x += x;
								pos.y += y;
								pos.z += z;
								AddChildObj(pos);
							}
						}
						if (!_isBoxZ) break;
					}
					break;
				case CREATETYPE.stairs:
					// 階段の生成プログラム
					for (int y = 0; y < _length; y++)
					{
						bool isSkip = false;
						for (int x = 0; x < _length; x++)
						{
							if (x == _length - y) isSkip = true;
							if (isSkip)
							{
								break;
							}
							else
							{
								// 座標の設定
								Vector3 tmppos = floorpos;
								tmppos.x += x * (int)_lr;
								tmppos.y += y * (int)_tb;
								// インスタンス化行う
								AddChildObj(tmppos);
							}
						}
					}
					break;
			}
		}
	}
	private void AddChildObj(Vector3 pos)
	{
		var tmpObj = Instantiate(_selectAddChildPrefab, pos, Quaternion.identity);
		tmpObj.gameObject.transform.parent = _object.gameObject.transform;
		tmpObj.name = tmpObj.name.Replace("(Clone)", "");
		// 親子付けを設定
		_selectChildObj = tmpObj;
		_selectChildID = _object.gameObject.transform.childCount - 1;
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
		// 種類の設定
		_parentObjectkind = _parentObject.AddComponent<RotObjkinds>();
		_parentObjectkind._RotObjKind = _kinds;

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
