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
        /*インターフェイス（公開関数）*/
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
                    return sMonoBehaviourObject.GetComponent<ForCoroutine>();//オーバーヘッド短縮できる？
                }

                return sMonoBehaviourObject.GetComponent<ForCoroutine>();
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

        /*内部関数*/

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
                //待機。UnloadScene()が廃止されて使えないためUnloadSceneAsync()を使用しましたが、完了するまで待機します。
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