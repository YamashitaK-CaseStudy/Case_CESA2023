using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace SuzumuraTomoki {
	public class SceneManager : MonoBehaviour {

		//�萔
		public const int NON_STAGE_SCENES_COUNT = 2;

		public enum TitleInitState
        {
			TITLE,
			STAGE_SELECT
        }

		//statics
		static public int _currentStageNum = 1;
		static public bool missionFailed = false;
		static public bool missionClear = false;//������Ԃ�\���ϐ����ӂ�����̂͊�����A���ߐ؂肪�߂����ߕύX�ӏ������Ȃ����邽��

		static private SceneManager _instance = null;
		static private UnityEngine.InputSystem.InputActionMap _playerInput = null;
		static private TitleInitState _titleInitStatet = TitleInitState.TITLE;

		/*****�ÓI�C���^�[�t�F�C�X�i���J�����o�j*******/

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
        * �w�肵���ԍ��̃X�e�[�W�����[�h���܂��B
        * @param[in] �X�e�[�W�̔ԍ��B�P�ȏ�B
        * @return false:�O�ȉ��܂��͑��݂��Ȃ��ԍ�
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

		/*****�C���^�[�t�F�C�X�i���J�����o�j*******/

		public int stageCount {
			get {
				return _stageCount;
			}
		}

		/*************��������***************/

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
		private int beforeSceneNumber = 0;
		private int currentSceneNumber = 0;
	}

}