using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class CStageEditor : EditorWindow
{
	private Vector3 floorpos;
	private int _floorWidth = 0;
	private GameObject _floorParts;
	private GameObject _floorParents;
	private bool isStairs = false;
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
	STAIRSDIRTB _tb;	// 上下の設定
	STAIRSDIRLR _lr;	// 左右の設定
	void CreateFloorInitialize()
	{
		string floorpath = "Assets/Prefabs/Stage/Pf_FloarParts.prefab";
		_floorParts = AssetDatabase.LoadAssetAtPath<GameObject>(floorpath);
		isStairs = false;
		// 方向設定
		_tb = STAIRSDIRTB.TOP;
		_lr = STAIRSDIRLR.LEFT;
	}

	void LayoutCreateFloor()
	{
		// 親子関係表示
		float wdith = 500;
		using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(prefabListScPos, EditorStyles.helpBox, GUILayout.Width(wdith)))
		{
			_floorParts = (GameObject)EditorGUILayout.ObjectField("床のパーツ", _floorParts, typeof(GameObject), true);
			// 座標指定
			EditorGUILayout.LabelField("座標");
			floorpos = EditorGUILayout.Vector3Field("", floorpos);
			if (!isStairs)
			{
				// 座標指定
				EditorGUILayout.LabelField("設置個数");
				_floorWidth = EditorGUILayout.IntField("", _floorWidth);
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
			// オブジェクトの生成開始
			if (GUILayout.Button("生成開始"))
			{
				CreateStart();
			}
		}

	}

	private void CreateStart()
	{
		// ヒエラルキーにFloorがあるなら親子付けないなら生成して親子付け
		CheckFloorParent();

		if (isStairs)
		{
			CreateStair();	// 階段を生成する
		}
		else
		{
			// 床の生成
			for (int i = 0; i < _floorWidth; i++)
			{
				// 座標の設定
				Vector3 tmppos = floorpos;
				tmppos.x += i;
				// インスタンス化行う
				var tmpObj = Instantiate(_floorParts, tmppos, Quaternion.identity);
				// 親子付けの設定
				tmpObj.gameObject.transform.parent = _floorParents.gameObject.transform;
			}
		}
	}

	private void CreateStair()
	{
		// 階段の生成プログラム
		for(int y = 0; y < _stairsSide; y++){
			bool isSkip = false;
			for(int x = 0; x < _stairsSide; x++){
				if(x == _stairsSide - y) isSkip = true;
				if(isSkip){
					break;
				}else{
					// 座標の設定
					Vector3 tmppos = floorpos;
					tmppos.x += x * (int) _lr;
					tmppos.y += y * (int) _tb;
					// インスタンス化行う
					var tmpObj = Instantiate(_floorParts, tmppos, Quaternion.identity);
					// 親子付けの設定
					tmpObj.gameObject.transform.parent = _floorParents.gameObject.transform;
				}
			}
		}
	}

	private void CheckFloorParent(){
		if(_floorParents == null){
			_floorParents = new GameObject("Floor");
		}
	}
}
