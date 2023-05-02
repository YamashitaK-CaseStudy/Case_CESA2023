using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace SuzumuraTomoki {
	public class SceneManager : MonoBehaviour {

		/*****�C���^�[�t�F�C�X�i���J�����j*******/

		//�萔
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

		//�v���p�e�B�E�֐�
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
        * �w�肵���ԍ��̃X�e�[�W�����[�h���܂��B
        * @param[in] �X�e�[�W�̔ԍ��B�P�ȏ�B
        * @return false:�O�ȉ��܂��͑��݂��Ȃ��ԍ�
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

		/*************��������***************/

		private void LoadScene(int sceneNumber) {
			beforeSceneNumber = currentSceneNumber;
			currentSceneNumber = sceneNumber;
			Fader.instance.FadeOut(sceneNumber);
		}

		private IEnumerator LoadSceneAsync(int sceneNumber) {
			beforeSceneNumber = currentSceneNumber;
			currentSceneNumber = sceneNumber;

			//���[�h��ɑO�̃V�[���̕`����~���邽�߂Ɏ擾���Ă���
			var camera = GameObject.Find("Main Camera");
			var canvas = GameObject.Find("Canvas");

			//���[�h
			var state = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);

			// Wait until the asynchronous scene fully loads
			while ( state.isDone == false ) {
				yield return null;
			}

			//�O�̃V�[���̕`����~
			if ( camera != null ) {
				camera.SetActive(false);
			}
			if ( canvas != null ) {
				canvas.SetActive(false);
			}

			//�A�����[�h
			state = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(beforeSceneNumber);
			while ( state.isDone == false ) {
				//�ҋ@�BUnloadScene()���p�~����Ďg���Ȃ�����UnloadSceneAsync()���g�p���܂������A��������܂őҋ@���܂��B
				yield return null;
			}
			Resources.UnloadUnusedAssets();
		}

		//�ϐ�

		private int _stageCount=0;
		private int score = 0;
		private int beforeSceneNumber = 0;
		private int currentSceneNumber = 0;
	}

}