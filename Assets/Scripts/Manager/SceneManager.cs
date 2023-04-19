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

        public int CurrentStageIndex
        {
            get
            {
                return currentStageIndex;
            }
        }

        private ForCoroutine MonoInstance
        {
            get
            {
                if (sMonoBehaviourObject == null)
                {
                    sMonoBehaviourObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
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

            UnityEngine.SceneManagement.SceneManager.LoadScene(stageSceneList[stageIndex - 1].name, LoadSceneMode.Additive);
            currentStageIndex = stageIndex;
            return true;
        }

        public void LoadTitle()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(titleScene.name, LoadSceneMode.Additive);
        }

        public void LoadResult()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(resultScene.name, LoadSceneMode.Additive);
        }

        public void LoadStageSelect()
        {
            MonoInstance.StartCoroutine(LoadSceneAsync(stageSelectScene.name));
            //UnityEngine.SceneManagement.SceneManager.LoadScene(stageSelectScene.name, LoadSceneMode.Additive);
            
        }

        /*�����֐�*/

        //private void OnValidate()
        //{
        //    sInstance = instance;
        //}

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var state = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (state.isDone == false)
            {
                yield return null;
            }
            currentScene.
            yield break;
            state = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
            while (state.isDone == false)
            {
                //�ҋ@�BUnloadScene()���p�~����Ďg���Ȃ�����UnloadSceneAsync()���g�p���܂������A��������܂őҋ@���܂��B
                yield return null;
            }
            Resources.UnloadUnusedAssets();

        }

        private static SceneManager sInstance = null;

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

        private int currentStageIndex = 1;

        private static GameObject sMonoBehaviourObject;
        //private static MonoBehaviour monoBehaviour;

    }

    class ForCoroutine : MonoBehaviour
    {

    }

}