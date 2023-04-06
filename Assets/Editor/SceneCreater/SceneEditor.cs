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

	[MenuItem("Editor/シーンエディター")]
	private static void ShowWindow() {
		EditorWindow.GetWindow(typeof(SceneEditor));
	}
	private void OnEnable() {
		_searchFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(_scenePath);
		_playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Pf_NewPlayer.prefab");
	}
	private void OnGUI()
	{
		if(EditorApplication.isPlaying){
			EditorGUILayout.HelpBox("実行中は操作できません", MessageType.Warning);
			return;
		}

		string[] tabList = {"新しいシーンの作成"};
		_tabNumber = (int)GUILayout.Toolbar(_tabNumber, tabList, EditorStyles.toolbarButton);
		switch(_tabNumber){
			case 0:
				LayoutCreateNewScene();
				break;
			default:
				Debug.LogError("Error");
				break;
		}

	}
	private void LayoutCreateNewScene(){
		// ドラッグアンドドロップで保存先を指定する
		_searchFolder = (DefaultAsset) EditorGUILayout.ObjectField("保存フォルダ", _searchFolder, typeof(DefaultAsset), false);

		// シーン名を設定する
		_sceneName = EditorGUILayout.TextField("シーン名",_sceneName);

		// プレイヤーのプレハブを設定
		_playerPrefab =(GameObject) EditorGUILayout.ObjectField("プレイヤープレハブ", _playerPrefab, typeof(GameObject), false);

		if(GUILayout.Button("シーンの作成を開始する")){
			CreateScene();
		}

		if(_isGetSameName){
			EditorGUILayout.HelpBox("同名のシーンが存在します", MessageType.Error);
		}
	}
	private void CreateScene(){
		// 同名のシーンがないか確認する
		_isGetSameName = false;
		if(CheckSameNameScene())
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
	private bool CheckSameNameScene(){
		// 同名のシーンが存在するか確認する
		var searchFolder = AssetDatabase.GetAssetOrScenePath(_searchFolder);
		var scenePath = searchFolder + "/" + _sceneName + ".unity";
		// GUIDの検索
		string[] tmp = AssetDatabase.FindAssets("t:Scene", new string[] {searchFolder});
		for(int i = 0; i < tmp.Length ;i++){
			string[] paths = tmp.Select(guid => AssetDatabase.GUIDToAssetPath(tmp[i])).ToArray();
			if(string.Join("\n", paths) == scenePath){
				return true;
			}
		}
		return false;
	}
	private void SceneSettings(){
		// プレイヤーの配置
		Vector3 pos = new Vector3(0,0,0);

		// プレハブからインスタンスを生成
		var tmpObj = Instantiate(_playerPrefab, pos, Quaternion.identity);
		tmpObj.name = "Player";

		// カメラのコンポーネントを設定
		var tmpCamera = GameObject.Find("Main Camera");
		var tmpCameraComp = tmpCamera.AddComponent<PlayerCamera>();
		tmpCameraComp.targetName = tmpObj.name;
		tmpCameraComp.cameraOffset = new Vector3(0,1.5f,-14);
		tmpCameraComp.targetOffset = new Vector3(1.2f,0,0);
	}
}
