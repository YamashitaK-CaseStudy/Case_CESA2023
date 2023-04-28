using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

public partial class SceneEditor : EditorWindow
{
	private DefaultAsset _searchFolder;
	string _sceneName = "NewScene";
	string _scenePath = "Assets/Scenes";
	int _tabNumber = 0;
	GameObject _playerPrefab;
	string _path;
	bool _isGetSameName = false;
	bool _isOnUnionObj = false;
	GameObject _UnionObthberPrefab;
	GameObject _BackGroundPrefab;

	[MenuItem("Editor/シーンエディター")]
	private static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(SceneEditor));
	}
	private void OnEnable()
	{
		_searchFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(_scenePath);
		_playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Pf_Player.prefab");
		_BackGroundPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Stage/Pf_BackGroundCanvas.prefab");
		_UnionObthberPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/RotObjectUnionObthber.prefab");
	}
	private void OnGUI()
	{
		if (EditorApplication.isPlaying)
		{
			EditorGUILayout.HelpBox("実行中は操作できません", MessageType.Warning);
			return;
		}

		string[] tabList = { "新しいシーンの作成" };
		_tabNumber = (int)GUILayout.Toolbar(_tabNumber, tabList, EditorStyles.toolbarButton);
		switch (_tabNumber)
		{
			case 0:
				LayoutCreateNewScene();
				break;
			default:
				Debug.LogError("Error");
				break;
		}

	}
	private void LayoutCreateNewScene()
	{
		// ドラッグアンドドロップで保存先を指定する
		_searchFolder = (DefaultAsset)EditorGUILayout.ObjectField("保存フォルダ", _searchFolder, typeof(DefaultAsset), false);

		// シーン名を設定する
		_sceneName = EditorGUILayout.TextField("シーン名", _sceneName);

		// プレイヤーのプレハブを設定
		_playerPrefab = (GameObject)EditorGUILayout.ObjectField("プレイヤープレハブ", _playerPrefab, typeof(GameObject), false);

		// 磁石オブジェクトを設置する場合置かなければいけないものがある
		_isOnUnionObj = EditorGUILayout.Toggle("磁石オブジェクトを設置するか", _isOnUnionObj);

		if (GUILayout.Button("シーンの作成を開始する"))
		{
			CreateScene();
		}

		if (_isGetSameName)
		{
			EditorGUILayout.HelpBox("同名のシーンが存在します", MessageType.Error);
		}

		EditorGUILayout.HelpBox("稀に必要オブジェクトが生成されないことがあるので\nその場合は以下のボタンを押してください", MessageType.Info);
		if (GUILayout.Button("再生成"))
		{
			SceneSettings();
		}
	}
	private void CreateScene()
	{
		// 同名のシーンがないか確認する
		_isGetSameName = false;
		if (CheckSameNameScene())
		{
			_isGetSameName = true;
			return;
		};

		// 新しいシーンを作成する
		Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
		newScene.name = _sceneName;
		// シーンをアクティブにする
		SceneManager.SetActiveScene(newScene);
		// シーンの保存先を変更する
		var searchFolder = AssetDatabase.GetAssetOrScenePath(_searchFolder);
		var path = searchFolder + "/" + _sceneName + ".unity";
		EditorSceneManager.SaveScene(newScene, path);

		// 生成したのちに必要なコンポーネントを設定する
		SceneSettings();
	}
	private bool CheckSameNameScene()
	{
		// 同名のシーンが存在するか確認する
		var searchFolder = AssetDatabase.GetAssetOrScenePath(_searchFolder);
		var scenePath = searchFolder + "/" + _sceneName + ".unity";
		// GUIDの検索
		string[] tmp = AssetDatabase.FindAssets("t:Scene", new string[] { searchFolder });
		for (int i = 0; i < tmp.Length; i++)
		{
			string[] paths = tmp.Select(guid => AssetDatabase.GUIDToAssetPath(tmp[i])).ToArray();
			if (string.Join("\n", paths) == scenePath)
			{
				return true;
			}
		}
		return false;
	}
	private void SceneSettings()
	{
		Debug.Log("設定開始");
		// プレイヤーの配置
		Vector3 pos = new Vector3(0, 1, 0);

		Debug.Log("プレイヤー生成");
		// プレハブからインスタンスを生成
		var tmpObj = Instantiate(_playerPrefab, pos, Quaternion.identity);
		tmpObj.name = "Player";
		Debug.Log(tmpObj);

		// カメラのコンポーネントを設定
		// カメラのオブジェクトを検索
		var tmpCameraObj = GameObject.Find("Main Camera");
		// プレイヤーカメラのコンポーネントを所得
		var tmpCameraComp = tmpCameraObj.AddComponent<PlayerCamera>();
		tmpCameraComp.targetName = tmpObj.name;
		tmpCameraComp.cameraOffset = new Vector3(0, 2f, -9);
		tmpCameraComp.targetOffset = new Vector3(0, 0, 0);

		// 背景のインスタンスを生成
		var tmpBG = Instantiate(_BackGroundPrefab, new Vector3(0,0,0), Quaternion.identity);
		var tmpBGComp = tmpBG.GetComponent<Canvas>();
		tmpBGComp.worldCamera = tmpCameraObj.GetComponent<Camera>();
		tmpBGComp.planeDistance = 99;
		// 磁石オブジェクト用のオブザーバーをインスタンス化する
		if (_isOnUnionObj)
		{
			var tmp = Instantiate(_UnionObthberPrefab, pos, Quaternion.identity);
			tmp.name = "RotObjectUnionObthber";
		}
	}
}
