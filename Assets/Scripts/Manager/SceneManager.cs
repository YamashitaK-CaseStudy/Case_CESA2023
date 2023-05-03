using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace SuzumuraTomoki {
	public class SceneManager : MonoBehaviour {

		/*****インターフェイス（公開実装）*******/

		//定数
		public const int NON_STAGE_SCENES_COUNT = 3;


		//statics
		private static SceneManager _instance = null;

		static public SceneManager instance {
			get {
				if ( _instance == null ) {
					GameObject gameObject = new GameObject("SceneManager");
					DontDestroyOnLoad(gameObject);
					_instance = gameObject.AddComponent<SceneManager>();
					_instance._stageCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - NON_STAGE_SCENES_COUNT;
				}

				return _instance;
			}
		}

		//プロパティ・関数
		public int Score {
			get {
				return score;
			}
			set {
				score = value;
			}
		}

		//public int CurrentStageIndex
		//{
		//    get
		//    {
		//        return currentStageIndex;
		//    }
		//}

		/**
        * 指定した番号のステージをロードします。
        * @param[in] ステージの番号。１以上。
        * @return false:０以下または存在しない番号
        */
		public bool LoadStage(int stageNumber) {
			if ( stageNumber < 1 ) {
				return false;
			}
			if ( stageNumber > _stageCount ) {
				return false;
			}

			//MonoInstance.StartCoroutine(LoadSceneAsync(stageSceneList[stageIndex - 1].name));
			//UnityEngine.SceneManagement.SceneManager.LoadScene(stageSceneList[stageIndex - 1].name);
			LoadScene(stageNumber + NON_STAGE_SCENES_COUNT - 1);
			return true;
		}

		public void LoadTitle() {
			LoadScene(0);
		}
		public void LoadStageSelect() {
			LoadScene(1);
		}
		public void LoadResult() {
			LoadScene(2);
		}

		public void LoadBeforeScene() {
			//MonoInstance.StartCoroutine(LoadSceneAsync(beforeSceneName));
			//UnityEngine.SceneManagement.SceneManager.LoadScene(beforeSceneName);
			LoadScene(beforeSceneNumber);

		}

		public void LoadCurrentScene(){
			LoadScene(currentSceneNumber);
		}

		public int stageCount {
			get {
				return _stageCount;
			}
		}

		/*************内部実装***************/

		private void LoadScene(int sceneNumber) {
			beforeSceneNumber = currentSceneNumber;
			currentSceneNumber = sceneNumber;
			Fader.instance.FadeOut(sceneNumber);
		}

		private IEnumerator LoadSceneAsync(int sceneNumber) {
			beforeSceneNumber = currentSceneNumber;
			currentSceneNumber = sceneNumber;

			//ロード後に前のシーンの描画を停止するために取得しておく
			var camera = GameObject.Find("Main Camera");
			var canvas = GameObject.Find("Canvas");

			//ロード
			var state = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);

			// Wait until the asynchronous scene fully loads
			while ( state.isDone == false ) {
				yield return null;
			}

			//前のシーンの描画を停止
			if ( camera != null ) {
				camera.SetActive(false);
			}
			if ( canvas != null ) {
				canvas.SetActive(false);
			}

			//アンロード
			state = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(beforeSceneNumber);
			while ( state.isDone == false ) {
				//待機。UnloadScene()が廃止されて使えないためUnloadSceneAsync()を使用しましたが、完了するまで待機します。
				yield return null;
			}
			Resources.UnloadUnusedAssets();
		}

		//変数

		private int _stageCount=0;
		private int score = 0;
		private int beforeSceneNumber = 0;
		private int currentSceneNumber = 0;
	}

}