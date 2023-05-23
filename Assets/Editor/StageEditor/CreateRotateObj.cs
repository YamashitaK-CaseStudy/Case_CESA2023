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
	RotObjkinds _parentObjectkind;  // 親のRotObjKinds
	RotObjkinds.ObjectKind _kinds;
	AudioSource _parentAudioSource; // 親のAudioSource
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
	int _boltLength = 1;
	int _boltTranslationLimit = 1;
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
					switch (_kinds)
					{
						case RotObjkinds.ObjectKind.NomalRotObject:
							LayoutSettings();
							break;
						case RotObjkinds.ObjectKind.UnionRotObject:
							LayoutSettings();
							break;
						case RotObjkinds.ObjectKind.BoltRotObject:
							LayoutBolt();
							break;
						case RotObjkinds.ObjectKind.SpinObject:
							LayoutSettings();
							break;
					}
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
				string path = null;
				switch (_kinds)
				{
					case RotObjkinds.ObjectKind.NomalRotObject:
						path = "Assets/Prefabs/Stage/Pf_Parts.prefab";
						CreateBaseRotateObject();
						break;
					case RotObjkinds.ObjectKind.UnionRotObject:
						path = "Assets/Prefabs/Stage/Pf_PartsUnionRed.prefab";
						CreateBaseRotateObject();
						break;
					case RotObjkinds.ObjectKind.BoltRotObject:
						path = "Assets/Prefabs/Stage/Pf_Bolt.prefab";
						_selectAddChildPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
						CreateBaseBoltObject();
						break;
					case RotObjkinds.ObjectKind.SpinObject:
						path = "Assets/Prefabs/Stage/Pf_PartsSpinOnly.prefab";
						CreateBaseSpinObject();
						break;
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

		if (GUILayout.Button("生成終了"))
		{
			isCreateStart = false;
			_selectChildObj = null;
			_childpos = new Vector3(0, 0, 0);
		}
	}

	private void LayoutBolt(){
		var boltComp = _parentObject.GetComponent<Bolt>();
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

		_isAddChildObj = EditorGUILayout.BeginFoldoutHeaderGroup(_isAddChildObj, "ボルトの設定");
		if (_isAddChildObj)
		{
			using (new GUILayout.VerticalScope("HelpBox"))
			{
				_boltLength = EditorGUILayout.IntField("長さ", _boltLength);
				if(_boltLength <= 0) _boltLength = 1;
				boltComp.length = (uint)_boltLength;
				_boltTranslationLimit = EditorGUILayout.IntField("回せる最大量", _boltTranslationLimit);
				if(_boltTranslationLimit <= 0) _boltTranslationLimit = 1;
				boltComp.translationLimit = (uint)_boltTranslationLimit;
			}
		}
		EditorGUILayout.EndFoldoutHeaderGroup();

		if (GUILayout.Button("生成終了"))
		{
			isCreateStart = false;
			_selectChildObj = null;
			_childpos = new Vector3(0, 0, 0);
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
				// オブジェクトの座標
		using (new GUILayout.HorizontalScope())
		{
			EditorGUILayout.LabelField("回転角", centerbold);
			_parentObject.transform.eulerAngles = EditorGUILayout.Vector3Field("", _parentObject.transform.eulerAngles);
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
								Vector3 tmppos = _childpos;
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
		Undo.RegisterCreatedObjectUndo(tmpObj, "Create New GameObject");
		// 親子付けを設定
		_selectChildObj = tmpObj;
		_selectChildID = _object.gameObject.transform.childCount - 1;
	}

	private void CreateBaseRotateObject()
	{
		// オブジェクトの生成
		_parentObject = new GameObject("RotatableObject");
		// タグ・レイヤーの設定
		_parentObject.tag = "RotateObject";
		_parentObject.layer = LayerMask.NameToLayer("Block");
		// 必要コンポーネントのアタッチ
		// Rigidbodyの生成　初期設定
		_parentRigdbody = _parentObject.AddComponent<Rigidbody>();
		_parentRigdbody.angularDrag = 0;
		_parentRigdbody.isKinematic = true;
		// RotatableObjectの生成
		_parentObject.AddComponent<RotatableObject>();
		// 種類の設定
		_parentObjectkind = _parentObject.AddComponent<RotObjkinds>();
		_parentObjectkind._RotObjKind = _kinds;

		// AudioSourceの追加
		_parentAudioSource = _parentObject.AddComponent<AudioSource>();
		_parentAudioSource.playOnAwake = false;

		_object = new GameObject("Object");
		_object.gameObject.transform.parent = _parentObject.gameObject.transform;
	}

	private void CreateBaseSpinObject()
	{
		// オブジェクトの生成
		_parentObject = new GameObject("SpinObject");
		// タグ・レイヤーの設定
		_parentObject.layer = LayerMask.NameToLayer("Block");
		// 必要コンポーネントのアタッチ
		// Rigidbodyの生成　初期設定
		_parentRigdbody = _parentObject.AddComponent<Rigidbody>();
		_parentRigdbody.angularDrag = 0;
		_parentRigdbody.isKinematic = true;
		Undo.RegisterCreatedObjectUndo(_parentObject, "Create New GameObject");
		// RotatableObjectの生成
		_parentObject.AddComponent<OnlySpinObj>();
		// 種類の設定
		_parentObjectkind = _parentObject.AddComponent<RotObjkinds>();
		_parentObjectkind._RotObjKind = _kinds;

		// AudioSourceの追加
		_parentAudioSource = _parentObject.AddComponent<AudioSource>();
		_parentAudioSource.playOnAwake = false;

		_object = new GameObject("Object");
		_object.gameObject.transform.parent = _parentObject.gameObject.transform;
		Undo.RegisterCreatedObjectUndo(_object, "Create New GameObject");
	}

	private void CreateBaseBoltObject()
	{
		// オブジェクトの生成
		_parentObject = Instantiate(_selectAddChildPrefab, new Vector3(0,0,0), Quaternion.identity);
		_parentObject.name = _parentObject.name.Replace("(Clone)", "");
		Undo.RegisterCreatedObjectUndo(_parentObject, "Create New GameObject");
		// タグ・レイヤーの設定
		_parentObject.layer = LayerMask.NameToLayer("Block");
		// 必要コンポーネントのアタッチ
		// 種類の設定
		_parentObjectkind = _parentObject.AddComponent<RotObjkinds>();
		_parentObjectkind._RotObjKind = _kinds;
		// AudioSourceの追加
		_parentAudioSource = _parentObject.AddComponent<AudioSource>();
		_parentAudioSource.playOnAwake = false;

		_object = _parentObject.transform.GetChild(0).gameObject;
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
