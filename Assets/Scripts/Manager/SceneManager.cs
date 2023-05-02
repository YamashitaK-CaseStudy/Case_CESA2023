using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace SuzumuraTomoki
{
    public class SceneManager : ScriptableObject
    {
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

            UnityEngine.SceneManagement.SceneManager.LoadScene(stageSceneList[stageIndex - 1].name);
            currentStageIndex = stageIndex;
            return true;
        }

        public void LoadTitle()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(titleScene.name);
        }

        public void LoadResult()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(resultScene.name);
        }

        public void LoadStageSelect()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(stageSelectScene.name);
        }

        private void OnValidate()
        {
            sInstance = instance;
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

    }
}