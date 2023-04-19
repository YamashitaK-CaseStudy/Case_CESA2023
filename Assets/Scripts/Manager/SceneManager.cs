using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace SuzumuraTomoki
{
    public class SceneManager : ScriptableObject
    {
        private void OnEnable()
        {
            if (sInstance == null)
            {
                sInstance = instance;
            }

            if (beforeSceneName == null)
            {
                beforeSceneName = titleScene.name;
            }
        }
        /*�C���^�[�t�F�C�X�i���J�֐��j*/
        public static SceneManager Instance
        {
            get
            {
                return sInstance;
            }
        }

        public int StageSize
        {
            get
            {
                return stageSceneList.Count;
            }
        }

        //public int CurrentStageIndex
        //{
        //    get
        //    {
        //        return currentStageIndex;
        //    }
        //}

        private ForCoroutine MonoInstance
        {
            get
            {
                if (sMonoBehaviourObject == null)
                {
                    sMonoBehaviourObject = new GameObject("CreatedByScenemManager");
                    sMonoBehaviourObject.AddComponent<ForCoroutine>();
                    return sMonoBehaviourObject.GetComponent<ForCoroutine>();//�I�[�o�[�w�b�h�Z�k�ł���H
                }

                return sMonoBehaviourObject.GetComponent<ForCoroutine>();
            }
        }

        /**
        * �w�肵���ԍ��̃X�e�[�W�����[�h���܂��B
        * @param[in] �X�e�[�W�̔ԍ��B�P�ȏ�B
        * @return false:�O�ȉ��܂��͑��݂��Ȃ��ԍ�
        */
        public bool LoadStage(int stageIndex)
        {
            if (stageIndex < 1)
            {
                return false;
            }
            if (stageIndex > stageSceneList.Count)
            {
                return false;
            }

            //MonoInstance.StartCoroutine(LoadSceneAsync(stageSceneList[stageIndex - 1].name));
            UnityEngine.SceneManagement.SceneManager.LoadScene(stageSceneList[stageIndex - 1].name);
            return true;
        }

        public void LoadTitle()
        {
            //MonoInstance.StartCoroutine(LoadSceneAsync(titleScene.name));
            UnityEngine.SceneManagement.SceneManager.LoadScene(titleScene.name);
        }

        public void LoadResult()
        {
            //MonoInstance.StartCoroutine(LoadSceneAsync(resultScene.name));
            UnityEngine.SceneManagement.SceneManager.LoadScene(resultScene.name);
        }

        public void LoadStageSelect()
        {
            //MonoInstance.StartCoroutine(LoadSceneAsync(stageSelectScene.name));
            UnityEngine.SceneManagement.SceneManager.LoadScene(stageSelectScene.name);
            Debug.Log("������ʂ�̂̓��[�h��H");

        }

        public void LoadBeforeScene()
        {
            //MonoInstance.StartCoroutine(LoadSceneAsync(beforeSceneName));
            UnityEngine.SceneManagement.SceneManager.LoadScene(stageSelectScene.name);

        }

        /*�����֐�*/

        //private void OnValidate()
        //{
        //    sInstance = instance;
        //}

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            beforeSceneName = currentScene.name;

            //���[�h��ɑO�̃V�[���̕`����~���邽�߂Ɏ擾���Ă���
            var camera = GameObject.Find("Main Camera");
            var canvas = GameObject.Find("Canvas");

            //���[�h
            Debug.Log("���[�h");
            var state = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (state.isDone == false)
            {
                yield return null;
            }

            //�O�̃V�[���̕`����~
            if (camera != null)
            {
                camera.SetActive(false);
            }
            if (canvas != null)
            {
                canvas.SetActive(false);
            }

            //�A�����[�h
            var state2 = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
            while (state.isDone == false)
            {
                //�ҋ@�BUnloadScene()���p�~����Ďg���Ȃ�����UnloadSceneAsync()���g�p���܂������A��������܂őҋ@���܂��B
                yield return null;
            }
            Resources.UnloadUnusedAssets();

        }

        private static SceneManager sInstance = null;
        private static GameObject sMonoBehaviourObject;

        [SerializeField]
        private SceneManager instance = null;

        [SerializeField]
        private List<Object> stageSceneList;

        [SerializeField]
        private Object titleScene;

        [SerializeField]
        private Object resultScene;

        [SerializeField]
        private Object stageSelectScene;

        private string beforeSceneName;

        //private static MonoBehaviour monoBehaviour;

    }

}