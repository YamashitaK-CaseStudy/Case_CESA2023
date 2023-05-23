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
	GameObject _ObserverPrefab;
	GameObject _BackGroundPrefab;
	GameObject _GrobalVolmePrefab;
	GameObject _MainCameraPrefab;
	GameObject _UICanvasPrefab;
	GameObject _SeedObjectprefab;
	GameObject _PlayerVCamPrefab;
	GameObject _GoalobjectPrefab;
	GameObject[] _SoundController;
	TotalSeedData _totalSeedData;
	bool _isMainStage = false;
	bool _isUseUnionObj = false;
	int _stageNumber = 5;
	int _seedCounts = 5;

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
		_ObserverPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Stage/Pf_Observer.prefab");
		_totalSeedData = AssetDatabase.LoadAssetAtPath<TotalSeedData>("Assets/Settings/TotalSeedData.asset");
		_GrobalVolmePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Stage/Pf_GlobalVolume.prefab");
		_MainCameraPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pf_MainCamera.prefab");
		_PlayerVCamPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/VCAM/PlayerFramingTransportVCAM.prefab");
		_UICanvasPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/Pf_UICanvas.prefab");
		_SoundController = new GameObject[3];
		_SoundController[0] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Sound/Pf_GameSoundManager.prefab");
		_SoundController[1] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Sound/Pf_PlayerSoundManager.prefab");
		_SoundController[2] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Sound/Pf_SystemSoundManager.prefab");
		_SeedObjectprefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Stage/Pf_SunflowerSeed.prefab");
		_GoalobjectPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Stage/Pf_Goal.prefab");
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

		// 磁石オブジェクトを使用するかどうか
		_isUseUnionObj = EditorGUILayout.ToggleLeft("磁石オブジェクトを使用するかどうか",_isUseUnionObj);

		// メインステージかどうか
		_isMainStage = EditorGUILayout.ToggleLeft("メインステージの作成かどうか",_isMainStage);

		if(_isMainStage){
			// ステージ番号の設定
			_stageNumber = EditorGUILayout.IntField("ステージ番号", _stageNumber);
			// 種の設定個数を変更
			_seedCounts = EditorGUILayout.IntField("種の最大数", _seedCounts);
		}

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
		var tmpPlayerObj = Instantiate(_playerPrefab, pos, Quaternion.identity);
		tmpPlayerObj.name = "Player";

		// ゴールを生成
		var tmpGoalObj = Instantiate(_playerPrefab, new Vector3(0,0,0), Quaternion.identity);

		// カメラのコンポーネントを設定
		// カメラのオブジェクトを検索
		var tmpCameraObj = GameObject.Find("Main Camera");
		// 最初からあるカメラを削除
		DestroyImmediate(tmpCameraObj);
		tmpCameraObj = Instantiate(_MainCameraPrefab, new Vector3(0,0,0), Quaternion.identity);
		tmpCameraObj.name = "Main Camera";

		// VCam設置
		var tmpVCam = Instantiate(_PlayerVCamPrefab, new Vector3(0,0,0), Quaternion.identity);
		var tmpVCamComp = tmpVCam.GetComponent<Cinemachine.CinemachineVirtualCamera>();
		tmpVCamComp.Follow = tmpPlayerObj.transform;

		// 背景のインスタンスを生成
		var tmpBG = Instantiate(_BackGroundPrefab, new Vector3(0,0,0), Quaternion.identity);
		var tmpBGComp = tmpBG.GetComponent<Canvas>();
		tmpBGComp.worldCamera = tmpCameraObj.GetComponent<Camera>();
		tmpBGComp.planeDistance = 99;
		var BGChildComp = tmpBG.transform.GetChild(0).GetComponent<ScrollBackGround>();
		BGChildComp._player = tmpPlayerObj;
		BGChildComp._goal = tmpGoalObj;

		// 環境エフェクトを配置する

		// 種オブジェクトの設定を書き換える
		_totalSeedData.defaultTotalCountList[_stageNumber - 1] = _seedCounts;
		// 種を配置する
		for(int i = 0 ; i < _seedCounts; i++){
			Instantiate(_SeedObjectprefab, new Vector3(0,0,0), Quaternion.identity);
		}

		// グローバルボリュームの生成
		var tmpGV = Instantiate(_GrobalVolmePrefab, new Vector3(0,0,0), Quaternion.identity);

		// 磁石オブジェクト用のオブザーバーをインスタンス化する
		var tmp = Instantiate(_ObserverPrefab, pos, Quaternion.identity);
		tmp.name = "ObserverObj";
		tmp.GetComponent<RotObjUnionObtherber>()._isUseUnion = _isUseUnionObj;

		// サウンドマネージャーを生成する
		for(int i = 0; i < _SoundController.Length; i++){
			Instantiate(_SoundController[i], new Vector3(0,0,0), Quaternion.identity);
		}
	}
}
