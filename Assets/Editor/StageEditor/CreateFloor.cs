using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class CStageEditor : EditorWindow
{
	private Vector3 floorpos;
	private int _floorWidth = 1;
	private int _floorHeight = 1;
	private int _floorDepth = 1;
	private GameObject[] _floorParts = new GameObject[1];
	private GameObject _floorParents;
	private bool isStairs = false;
	private bool _isDown = true;
	private bool _isDepth = true;
	private bool _isMultiBlock = false;
	private int _stairsSide;
	private enum STAIRSDIRTB    // 上下の設定
	{
		TOP = 1,
		BOTOM = -1
	}
	private enum STAIRSDIRLR    // 左右の設定
	{
		RIGHT = 1,
		LEFT = -1
	}
	STAIRSDIRTB _tb;    // 上下の設定
	STAIRSDIRLR _lr;    // 左右の設定

	void CreateFloorInitialize()
	{
		string floorpath = "Assets/Prefabs/Stage/Pf_FloarParts.prefab";
		_floorParts[0] = AssetDatabase.LoadAssetAtPath<GameObject>(floorpath);
		isStairs = false;
		// 方向設定
		_tb = STAIRSDIRTB.TOP;
		_lr = STAIRSDIRLR.LEFT;
	}

	void LayoutCreateFloor()
	{
		using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(prefabListScPos, EditorStyles.helpBox))
		{
			// 座標指定
			EditorGUILayout.LabelField("座標");
			floorpos = EditorGUILayout.Vector3Field("", floorpos);
			if (!isStairs)
			{
				// 座標指定
				EditorGUILayout.LabelField("設置個数");
				using (new GUILayout.HorizontalScope())
				{
					_floorWidth = EditorGUILayout.IntField("幅(X)", _floorWidth);
					_floorHeight = EditorGUILayout.IntField("高さ(Y)", _floorHeight);
					_floorDepth = EditorGUILayout.IntField("奥行(Z)", _floorDepth);
				}
				_isDown = EditorGUILayout.ToggleLeft("下方向に設置", _isDown);
				_isDepth = EditorGUILayout.ToggleLeft("奥方向に設置", _isDepth);
			}
			// 階段生成
			isStairs = EditorGUILayout.ToggleLeft("階段生成", isStairs);
			if (isStairs)
			{
				using (new GUILayout.VerticalScope("HelpBox"))
				{
					EditorGUILayout.HelpBox("正方形サイズで生成します", MessageType.Info);
					EditorGUILayout.Space();
					_stairsSide = EditorGUILayout.IntField("サイズ", _stairsSide);
					EditorGUILayout.Space();
					EditorGUILayout.HelpBox("それぞれ尖ってる方を指定します。", MessageType.Info);
					using (new GUILayout.HorizontalScope())
					{
						_tb = (STAIRSDIRTB)EditorGUILayout.EnumPopup("上下", _tb);
						_lr = (STAIRSDIRLR)EditorGUILayout.EnumPopup("左右", _lr);
					}
				}
			}

			Line();
			_isMultiBlock = EditorGUILayout.ToggleLeft("複数種類のブロックを使用する", _isMultiBlock);
			if (!_isMultiBlock)
			{
				if (_floorParts.Length != 1)
				{
					System.Array.Resize(ref _floorParts, 1);
				}
				_floorParts[0] = (GameObject)EditorGUILayout.ObjectField("床のパーツ", _floorParts[0], typeof(GameObject), true);
			}
			else
			{
				if (_floorParts.Length != _floorHeight)
				{
					System.Array.Resize(ref _floorParts, _floorHeight);
				}
				EditorGUILayout.HelpBox("Y軸ごとに変更されていきます", MessageType.Info);
				for (int i = 0; i < _floorHeight; i++)
				{
					_floorParts[i] = (GameObject)EditorGUILayout.ObjectField("床のパーツ", _floorParts[i], typeof(GameObject), true);
				}
			}
			// オブジェクトの生成開始
			if (GUILayout.Button("生成開始"))
			{
				CreateStart();
			}
		}

	}

	private void CreateStart()
	{
		// ヒエラルキーにFloorがあるなら親子付け、ないなら生成して親子付け
		CheckFloorParent();

		if (isStairs)
		{
			CreateStair(_stairsSide);   // 階段を生成する
		}
		else
		{
			for (int i = 0; i < _floorHeight; i++)
			{
				int floornum = i;
				if (!_isMultiBlock) floornum = 0;
				// 床の生成
				for (int k = 0; k < _floorDepth; k++)
				{
					for (int j = 0; j < _floorWidth; j++)
					{
						// 座標の設定
						Vector3 tmppos = floorpos;
						tmppos.x += j;
						if (_isDown)
						{
							tmppos.y -= i;
						}
						else
						{
							tmppos.y += i;
						}
						if (_isDepth)
						{
							tmppos.z += k;
						}
						else
						{
							tmppos.z -= k;
						}
						// インスタンス化行う
						var tmpObj = Instantiate(_floorParts[floornum], tmppos, Quaternion.identity);
						// 親子付けの設定
						tmpObj.gameObject.transform.parent = _floorParents.gameObject.transform;
						Undo.RegisterCreatedObjectUndo(tmpObj, "Create New GameObject");
					}
				}
			}
		}
	}

	private void CreateStair(int side)
	{
		// 階段の生成プログラム
		for (int y = 0; y < side; y++)
		{
			bool isSkip = false;
			int floornum = y;
			if (!_isMultiBlock) floornum = 0;
			for (int x = 0; x < side; x++)
			{
				if (x == side - y) isSkip = true;
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
					var tmpObj = Instantiate(_floorParts[floornum], tmppos, Quaternion.identity);
					// 親子付けの設定
					tmpObj.gameObject.transform.parent = _floorParents.gameObject.transform;
					Undo.RegisterCreatedObjectUndo(tmpObj, "Create New GameObject");
				}
			}
		}
	}

	private void CheckFloorParent()
	{
		var tmpObj = GameObject.Find("Floor");
		if (tmpObj != null)
		{
			_floorParents = tmpObj;
		}
		// ヒエラルキー内のオブジェクトを確認
		if (_floorParents == null)
		{
			_floorParents = new GameObject("Floor");
			_floorParents.layer = 13;
		}
	}
}
