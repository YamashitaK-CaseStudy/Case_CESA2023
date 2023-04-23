using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

public partial class CUpdatePrefab : EditorWindow
{
	private string path = "Assets/Prefabs/Stage/";  // 検索するファイル
	string[] _paths;
	bool isEditorInit = false;  // 初期化チェック
	Vector2 _ScPosSummary;
	Vector2 _ScPosSetting;
	GameObject[] _rotObjects;
	GameObject[] _rotChildObjects;

	[MenuItem("Editor/アップデートプレハブ")]
	private static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(CUpdatePrefab));
	}
	private void Initalize()
	{
		isEditorInit = true;
		Reload();
	}

	// GUI 表示
	private void OnGUI()
	{
		// 初期化処理
		if (!isEditorInit) Initalize();
		// レイアウト
		using (new GUILayout.HorizontalScope())
		{
			// 親子関係表示
			using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(_ScPosSummary, EditorStyles.helpBox, GUILayout.Width(250)))
			{
				_ScPosSummary = EditorGUILayout.BeginScrollView(_ScPosSummary);
				// タイトル
				GUILayout.Box("RotateObject一覧");
				Line();
				LayoutSummary();
				EditorGUILayout.EndScrollView();
			}

			// 詳細設定
			using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(_ScPosSetting, EditorStyles.helpBox))
			{
				// タイトル
				GUILayout.Box("設定");
				Line();
				LayoutSettings();
			}
		}
	}

	private void LayoutSummary()
	{
		if (GUILayout.Button("オブジェクト一覧リロード"))
		{
			Reload();
		}
		for (int i = 0; i < _rotObjects.Length; i++)
		{
			// GameObjectの名前を表示.
			EditorGUILayout.LabelField(_rotObjects[i].name);
			using (new EditorGUI.IndentLevelScope())
			{
				var child = _rotObjects[i].transform.GetChild(0);
				for (int j = 0; j < child.childCount; j++)
				{
					EditorGUILayout.LabelField(child.transform.GetChild(j).name);
				}
			}
		}
	}
	private void Reload()
	{
		int num = 0;
		System.Array.Resize(ref _rotObjects, 0);
		// typeで指定した型の全てのオブジェクトを配列で取得し,その要素数分繰り返す.
		foreach (RotatableObject obj in UnityEngine.Object.FindObjectsOfType(typeof(RotatableObject)))
		{
			// シーン上に存在するオブジェクトならば処理.
			if (obj.transform.gameObject.activeInHierarchy)
			{
				System.Array.Resize(ref _rotObjects, num + 1);
				_rotObjects[num] = obj.transform.gameObject;
			}
			num++;
		}
		Debug.Log(_rotObjects.Length);
	}
	private void LayoutSettings()
	{
		if (GUILayout.Button("置き換え開始"))
		{
			ReplacePrefab();
		}
		// GUIDの検索
		string[] guids = AssetDatabase.FindAssets("t:prefab", new string[] { path });
		_paths = guids.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
		EditorGUILayout.LabelField("プレハブ一覧");
		for (int i = 0; i < _paths.Length; i++)
		{
			EditorGUILayout.LabelField(_paths[i]);
		}
	}
	private void ReplacePrefab()
	{
		string[] name = new string[_paths.Length];
		for (int i = 0; i < _paths.Length; i++)
		{
			name[i] = _paths[i].Replace(path, "");
			name[i] = name[i].Replace(".prefab", "");
		}

		for (int i = 0; i < _rotObjects.Length; i++)
		{
			var tmpObj = new GameObject("Object");
			tmpObj.transform.parent = _rotObjects[i].transform;
			var child = _rotObjects[i].transform.GetChild(0);
			for (int j = 0; j < child.childCount; j++)
			{
				for (int k = 0; k < _paths.Length; k++)
				{
					var obj = child.GetChild(j);
					if (obj.name == name[k])
					{
						Replace(obj.gameObject,tmpObj,_paths[k]);
					}
				}
			}
			DestroyImmediate(_rotObjects[i].transform.GetChild(0).gameObject);
		}

	}
	private void Replace(GameObject child, GameObject parent ,string prefabPath){
		Debug.Log(child.name);
		Debug.Log(prefabPath);

		var prefabData = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
		// プレハブからインスタンスを生成
		var childObj = Instantiate(prefabData, child.transform.position, Quaternion.identity);
		childObj.gameObject.transform.parent = parent.transform;
		childObj.name = childObj.name.Replace("(Clone)","");
	}
	private void Line()
	{
		var splitterRect = EditorGUILayout.GetControlRect(false, GUILayout.Height(1));
		splitterRect.x = 0;
		splitterRect.width = position.width - 100f;
		EditorGUI.DrawRect(splitterRect, Color.Lerp(Color.gray, Color.gray, 1.0f));
	}
}
