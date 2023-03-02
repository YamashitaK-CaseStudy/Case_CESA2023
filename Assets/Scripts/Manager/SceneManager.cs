using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace SuzumuraTomoki
{
    [CreateAssetMenu(fileName = "SceneManager", menuName = "SceneManager")]
    public class SceneManager : ScriptableObject
    {
        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindInstance();
                }
                return instance;
            }
        }

        static SceneManager FindInstance()
        {
            var guidArray = AssetDatabase.FindAssets("t:SceneManager");
            if (guidArray.Length == 0)
            {
                Debug.Log("SceneManager�̃C���X�^���X��������܂���ł���");
                throw new System.IO.FileNotFoundException("SceneManager does not found");
            }

            var path = AssetDatabase.GUIDToAssetPath(guidArray[0]);
            return AssetDatabase.LoadAssetAtPath<SceneManager>(path);
        }
        /**
        * �w�肵���ԍ��̃X�e�[�W�����[�h���܂��B
        * @param[in] �X�e�[�W�̔ԍ��B�P�ȏ�B
        * @return false:�O�ȉ��܂��͑��݂��Ȃ��ԍ�
        */
        public bool LoadStage(int stageID)
        {
            if (stageID < 1)
            {
                return false;
            }
            if (stageID > stageSceneList.Count)
            {
                return false;
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(stageSceneList[stageID - 1].name);
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

        private static SceneManager instance = null;

        [SerializeField]
        private List<Object> stageSceneList;

        [SerializeField]
        private Object titleScene;

        [SerializeField]
        private Object resultScene;

    }
}