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
	[MenuItem("Editor/床オブジェクト更新")]
	private static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(ReloadPrefabFloor));
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

		for(int i = 0; i < _floorObj[0].transform.childCount; i++){
			var child = _floorObj[0].transform.GetChild(i);

			if(child.name.Contains(("Clone"))){
				child.name = child.name.Replace("(Clone)","");
			}
			if(child.name.Contains(("Floar"))){
				child.name = child.name.Replace("Floar","Floor");
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
				if(child.name == "Pf_FloarParts"){
					Replace(child.gameObject, newFloor, _paths[0]);
					break;
				}
				if(child.name == "Pf_FloorParts"){
					Replace(child.gameObject, newFloor, _paths[0]);
					break;
				}
			}
		}
		DestroyImmediate(_floorObj[0].gameObject);
	}

	private void Replace(GameObject child, GameObject parent, string prefabPath)
	{
		var prefabData = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
		// プレハブからインスタンスを生成
		var childObj = Instantiate(prefabData, child.transform.position, Quaternion.identity);
		childObj.gameObject.transform.parent = parent.transform;
		childObj.name = childObj.name.Replace("(Clone)", "");
	}
}
