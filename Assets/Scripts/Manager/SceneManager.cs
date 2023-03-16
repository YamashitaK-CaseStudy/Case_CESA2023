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
                if (instance == null)
                {
                    instance = FindInstance();
                }
                return instance;
            }
        }

        private static SceneManager FindInstance()
        {
            var guidArray = AssetDatabase.FindAssets("t:SceneManager");
            if (guidArray.Length == 0)
            {
                Debug.Log("SceneManagerのインスタンスが見つかりませんでした");
                throw new System.IO.FileNotFoundException("SceneManager does not found");
            }

            var path = AssetDatabase.GUIDToAssetPath(guidArray[0]);
            return AssetDatabase.LoadAssetAtPath<SceneManager>(path);
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
        * 指定した番号のステージをロードします。
        * @param[in] ステージの番号。１以上。
        * @return false:０以下または存在しない番号
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

        private static SceneManager instance = null;

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