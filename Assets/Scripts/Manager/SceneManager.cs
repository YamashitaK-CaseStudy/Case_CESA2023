using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace SuzumuraTomoki {
	public class SceneManager : MonoBehaviour {

		//定数
		public const int NON_STAGE_SCENES_COUNT = 2;

		public enum TitleInitState
        {
			TITLE,
			STAGE_SELECT
        }

		//statics
		static public int _currentStageNum = 1;
		static public bool missionFailed = false;
		static public bool missionClear = false;//同じ状態を表す変数がふたつあるのは奇妙だが、締め切りが近いため変更箇所を少なくするため

		static private SceneManager _instance = null;
		static private UnityEngine.InputSystem.InputActionMap _playerInput = null;
		static private TitleInitState _titleInitStatet = TitleInitState.TITLE;

		/*****静的インターフェイス（公開メンバ）*******/

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

		static public UnityEngine.InputSystem.InputActionMap playerInput
		{
			get
			{
				if (_playerInput == null)
				{
					_playerInput = Resources.Load<UnityEngine.InputSystem.InputActionAsset>("InputSeet").FindActionMap("Player");
				}

				return _playerInput;
			}
		}

		static public TitleInitState titleInitState
		{
			get
			{
				return _titleInitStatet;
			}
		}

		/**
        * 指定した番号のステージをロードします。
        * @param[in] ステージの番号。１以上。
        * @return false:０以下または存在しない番号
        */
		static public bool LoadStage(int stageNumber) {
			if ( stageNumber < 1 ) {
				return false;
			}
			if ( stageNumber > instance._stageCount) {
				return false;
			}

			bool flg = instance.LoadScene(stageNumber + NON_STAGE_SCENES_COUNT - 1);
			if(flg) {
				_currentStageNum = stageNumber;
			}
			return flg;
		}

		static public void LoadTitle() {
			_titleInitStatet = TitleInitState.TITLE;
			instance.LoadScene(0);
		}
		static public void LoadStageSelect() {
			_titleInitStatet = TitleInitState.STAGE_SELECT;
			instance.LoadScene(0);
		}
		static public void LoadResult() {
			instance.LoadScene(1);
		}

		static public void LoadBeforeScene() {
			instance.LoadScene(instance.beforeSceneNumber);
		}

		static public void LoadCurrentScene(){
			instance.LoadScene(instance.currentSceneNumber);
		}

		static public void LoadNextScene()
		{
            if (!LoadStage(_currentStageNum + 1))
            {
				if(_currentStageNum == instance.stageCount){
					LoadTitle();
				}
            }
		}

		/*****インターフェイス（公開メンバ）*******/

		public int stageCount {
			get {
				return _stageCount;
			}
		}

		/*************内部実装***************/

		private bool LoadScene(int sceneNumber) {
			if(Fader.state != Fader.State.NONE)
            {
				return false;
            }
			playerInput.Disable();
			beforeSceneNumber = currentSceneNumber;
			currentSceneNumber = sceneNumber;
			Fader.instance.FadeOut(sceneNumber);
			GameSoundManager.Instance.StopGameBGMWithFade(Fader.instance.fadeTime);
			GameSoundManager.Instance.StopGameSEWithFade(Fader.instance.fadeTime);
			SystemSoundManager.Instance.StopBGMWithFade(Fader.instance.fadeTime);
			return true;
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
		private int beforeSceneNumber = 0;
		private int currentSceneNumber = 0;
	}

}