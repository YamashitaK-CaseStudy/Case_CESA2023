using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

public class ReloadPrefabFloor : EditorWindow
{
	GameObject[] _floorObj;
	private string _path = "Assets/Prefabs/Stage/Floor/";  // 検索するファイル
	string[] _paths;
	bool _isSwitchFloorObj = false;
	int[] _posOfChangeLow = new int[3];
	int[] _posOfChangeHight = new int[3];
	bool _isSetX = true;
	bool _isSetY = false;
	bool _isSetZ = false;
	GameObject _floorParts;
	[MenuItem("Editor/床オブジェクト更新")]
	private static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(ReloadPrefabFloor));
	}

	private void OnEnable(){
		_floorParts = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Stage/Floor/Pf_FloorParts01.prefab");
	}

	private void OnGUI()
	{
		EditorGUILayout.HelpBox("Floorが複数ある場合の移し替えボタン", MessageType.Info);
		if (GUILayout.Button("移し替え"))
		{
			MoveFloor();
		}
		EditorGUILayout.HelpBox("床のプレハブが更新されていたら", MessageType.Info);
		if (GUILayout.Button("差し替え"))
		{
			UpdateFloor();
		}

		Line();
		_isSwitchFloorObj = EditorGUILayout.ToggleLeft("置き変えを行うか", _isSwitchFloorObj);
		if (_isSwitchFloorObj)
		{
			EditorGUILayout.HelpBox("床のオブジェクトを置き換える\n指定した範囲のFloorオブジェクトを置き換える\n 最大 5 最小1の場合は1 ~ 5の間にあるものを置き換える", MessageType.Info);
			_floorParts = (GameObject)EditorGUILayout.ObjectField("パーツ", _floorParts, typeof(GameObject), false);
			_isSetX = EditorGUILayout.ToggleLeft("X", _isSetX);
			if (_isSetX)
			{
				using (new GUILayout.HorizontalScope())
				{
					_posOfChangeHight[0] = EditorGUILayout.IntField("最大値", _posOfChangeHight[0]);
					GUILayout.FlexibleSpace();
					_posOfChangeLow[0] = EditorGUILayout.IntField("最小値", _posOfChangeLow[0]);
				}
			}
			_isSetY = EditorGUILayout.ToggleLeft("Y", _isSetY);
			if (_isSetY)
			{
				using (new GUILayout.HorizontalScope())
				{
					_posOfChangeHight[1] = EditorGUILayout.IntField("最大値", _posOfChangeHight[1]);
					GUILayout.FlexibleSpace();
					_posOfChangeLow[1] = EditorGUILayout.IntField("最小値", _posOfChangeLow[1]);
				}
			}
			_isSetZ = EditorGUILayout.ToggleLeft("Z", _isSetZ);
			if (_isSetZ)
			{
				using (new GUILayout.HorizontalScope())
				{
					_posOfChangeHight[2] = EditorGUILayout.IntField("最大値", _posOfChangeHight[2]);
					GUILayout.FlexibleSpace();
					_posOfChangeLow[2] = EditorGUILayout.IntField("最小値", _posOfChangeLow[2]);
				}
			}
			if (GUILayout.Button("置き換え"))
			{
				SwitchFloor();
			}
		}
	}

	private void MoveFloor()
	{
		int arrayLength = 0;
		System.Array.Resize(ref _floorObj, 0);
		foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
		{
			// シーン上に存在するオブジェクトならば処理.
			if (obj.transform.gameObject.name == "Floor")
			{
				System.Array.Resize(ref _floorObj, arrayLength + 1);
				_floorObj[arrayLength] = obj.transform.gameObject;
				arrayLength++;
			}
		}

		if (arrayLength > 1)
		{
			var baseObj = _floorObj[0];
			for (int i = 1; i < arrayLength; i++)
			{
				for (int j = 0; j < _floorObj[i].gameObject.transform.childCount; j++)
				{
					var child = _floorObj[i].gameObject.transform.GetChild(j).gameObject;
					child.transform.parent = baseObj.gameObject.transform;
					Debug.Log(j);
				}
			}
		}

		for (int i = 0; i < _floorObj[0].transform.childCount; i++)
		{
			var child = _floorObj[0].transform.GetChild(i);

			if (child.name.Contains(("Clone")))
			{
				child.name = child.name.Replace("(Clone)", "");
			}
			if (child.name.Contains(("Floar")))
			{
				child.name = child.name.Replace("Floar", "Floor");
			}
		}

	}

	private void UpdateFloor()
	{
		if (_floorObj.Length != 1) return;

		// GUIDの検索
		string[] guids = AssetDatabase.FindAssets("t:prefab", new string[] { _path });
		_paths = guids.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
		// 名前検索させるためにパス部分と拡張子を一度消したものを別途で用意する
		string[] name = new string[_paths.Length];
		for (int i = 0; i < _paths.Length; i++)
		{
			name[i] = _paths[i].Replace(_path, "");
			name[i] = name[i].Replace(".prefab", "");
		}

		GameObject newFloor = new GameObject("Floor");
		newFloor.layer = LayerMask.NameToLayer("Block");

		int childNum = _floorObj[0].transform.childCount;
		for (int i = 0; i < childNum; i++)
		{
			var child = _floorObj[0].transform.GetChild(i);
			for (int j = 0; j < _paths.Length; j++)
			{
				// 同名のオブジェクトに置き換える
				if (child.name == name[j])
				{
					Replace(child.gameObject, newFloor, _paths[j]);
					break;
				}
				// 旧式名の場合01番に変更する
				if (child.name == "Pf_FloarParts")
				{
					Replace(child.gameObject, newFloor, _paths[0]);
					break;
				}
				if (child.name == "Pf_FloorParts")
				{
					Replace(child.gameObject, newFloor, _paths[0]);
					break;
				}
			}
		}
		Undo.DestroyObjectImmediate(_floorObj[0].gameObject);
	}

	private void Replace(GameObject child, GameObject parent, string prefabPath)
	{
		var prefabData = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
		// プレハブからインスタンスを生成
		var childObj = Instantiate(prefabData, child.transform.position, Quaternion.identity);
		childObj.gameObject.transform.parent = parent.transform;
		childObj.name = childObj.name.Replace("(Clone)", "");
		Undo.RegisterCreatedObjectUndo(childObj, "Create New GameObject");
	}

	private void SwitchFloor()
	{
		foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
		{
			// シーン上に存在するオブジェクトならば処理.
			if (obj.name == "Floor")
			{
				for (int i = 0; i < obj.transform.childCount; i++)
				{
					var floorObj = obj.transform.GetChild(i).gameObject;
					var floorObjPos = floorObj.transform.position;
					bool isSwitchFloorX = true;
					bool isSwitchFloorY = true;
					bool isSwitchFloorZ = true;
					// 各種方向の
					if (_isSetX)
					{
						isSwitchFloorX = CheckPositionScope(floorObjPos.x, 0);
					}
					if (_isSetY)
					{
						isSwitchFloorY = CheckPositionScope(floorObjPos.y, 1);
					}
					if (_isSetZ)
					{
						isSwitchFloorZ = CheckPositionScope(floorObjPos.z, 2);
					}

					if (isSwitchFloorX && isSwitchFloorY && isSwitchFloorZ)
					{
						var newfloor = Instantiate(_floorParts, floorObjPos, Quaternion.identity);
						newfloor.gameObject.transform.parent = obj.transform;
						newfloor.name = newfloor.name.Replace("(Clone)", "");
						Undo.RegisterCreatedObjectUndo(newfloor, "Create New GameObject");
						Undo.DestroyObjectImmediate(floorObj.gameObject);
					}
				}
				break;
			}
		}

		// シーンに変更があったことを知らせる
		var scene = SceneManager.GetActiveScene();
		EditorSceneManager.MarkSceneDirty(scene);
	}

	private void Line()
	{
		var splitterRect = EditorGUILayout.GetControlRect(false, GUILayout.Height(1));
		splitterRect.x = 0;
		splitterRect.width = position.width - 100f;
		EditorGUI.DrawRect(splitterRect, Color.Lerp(Color.gray, Color.gray, 1.0f));
	}

	bool CheckPositionScope(float pos, int num)
	{
		Debug.Log(pos);
		if (_posOfChangeHight[num] >= pos && pos >= _posOfChangeLow[num])
		{
			Debug.Log("あ");
			return true;
		}

		return false;
	}
}

