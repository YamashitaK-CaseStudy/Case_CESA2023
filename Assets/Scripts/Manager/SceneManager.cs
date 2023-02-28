using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuzumuraTomoki
{
    [CreateAssetMenu(fileName = "SceneManager", menuName = "SceneManager")]
    public class SceneManager : ScriptableObject
    {


        public void LoadStage(int stageID)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(stageList[stageID].name);
        }

        public void LoadTitle()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(titleScene.name);
        }

        public void LoadResult()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(resultScene.name);
        }

        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (SceneManager)Resources.Load(typeof(SceneManager).Name);
                }
                return instance;
            }
        }


        static SceneManager instance;

        [SerializeField]
        private List<Object> stageList;

        [SerializeField]
        private Object titleScene;

        [SerializeField]
        private Object resultScene;

    }
}