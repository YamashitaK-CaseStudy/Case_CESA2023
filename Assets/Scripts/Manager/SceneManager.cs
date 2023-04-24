using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace SuzumuraTomoki
{
    public class SceneManager : ScriptableObject
    {
        /*思うように挙動してくれない　Gitから落としてすぐに実行すると通らない*/
        //private void OnEnable()
        //{
        //    if (sInstance == null)
        //    {
        //        sInstance = instance;
        //    }

        //    if (beforeSceneName == null)
        //    {
        //        beforeSceneName = titleScene.name;
        //    }
        //}

        /*インターフェイス（公開関数）*/
        public int StageSize
        {
            get
            {
                return stageSceneList.Count;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
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

            //MonoInstance.StartCoroutine(LoadSceneAsync(stageSceneList[stageIndex - 1].name));
            //UnityEngine.SceneManagement.SceneManager.LoadScene(stageSceneList[stageIndex - 1].name);
            LoadScene(stageSceneList[stageIndex - 1].name);
            return true;
        }

        public void LoadTitle()
        {
            //MonoInstance.StartCoroutine(LoadSceneAsync(titleScene.name));
            //UnityEngine.SceneManagement.SceneManager.LoadScene(titleScene.name);
            LoadScene(titleScene.name);
        }

        public void LoadResult()
        {
            //MonoInstance.StartCoroutine(LoadSceneAsync(resultScene.name));
            //UnityEngine.SceneManagement.SceneManager.LoadScene(resultScene.name);
            LoadScene(resultScene.name);
        }

        public void LoadStageSelect()
        {
            //MonoInstance.StartCoroutine(LoadSceneAsync(stageSelectScene.name));
            //UnityEngine.SceneManagement.SceneManager.LoadScene(stageSelectScene.name);
            LoadScene(stageSelectScene.name);
        }

        public void LoadBeforeScene()
        {
            //MonoInstance.StartCoroutine(LoadSceneAsync(beforeSceneName));
            //UnityEngine.SceneManagement.SceneManager.LoadScene(beforeSceneName);
            LoadScene(beforeSceneName);

        }

        /*内部関数*/

        //private void OnValidate()
        //{
        //    sInstance = instance;
        //}

        private void LoadScene(string sceneName)
        {
            if (FadeCanvasInstance == null)
            {
                FadeCanvasInstance = Instantiate(prefabFadeCanvas);
                DontDestroyOnLoad(FadeCanvasInstance);
                FaderInstance = FadeCanvasInstance.transform.GetChild(0).GetComponent<Fader>();
            }

            FaderInstance.FadeOut(sceneName);

        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            beforeSceneName = currentScene.name;

            //ロード後に前のシーンの描画を停止するために取得しておく
            var camera = GameObject.Find("Main Camera");
            var canvas = GameObject.Find("Canvas");

            //ロード
            Debug.Log("ロード");
            var state = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (state.isDone == false)
            {
                yield return null;
            }

            //前のシーンの描画を停止
            if (camera != null)
            {
                camera.SetActive(false);
            }
            if (canvas != null)
            {
                canvas.SetActive(false);
            }

            //アンロード
            var state2 = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
            while (state.isDone == false)
            {
                //待機。UnloadScene()が廃止されて使えないためUnloadSceneAsync()を使用しましたが、完了するまで待機します。
                yield return null;
            }
            Resources.UnloadUnusedAssets();

        }

        [SerializeField]
        private List<Object> stageSceneList;

        [SerializeField]
        private Object titleScene;
        [SerializeField]
        private Object resultScene;
        [SerializeField]
        private Object stageSelectScene;
        [SerializeField]
        private GameObject prefabFadeCanvas;

        private GameObject FadeCanvasInstance;
        private Fader FaderInstance;
        private int score = 0;

        private string beforeSceneName = "Title";//変数にするべきだがUnityとの兼ね合いがあるのでやむを得ずベタ打ち

        //private static MonoBehaviour monoBehaviour;

    }

}