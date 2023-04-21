using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private void Awake()
    {
        fadeImage = gameObject.GetComponent<Image>();
        if (fadeImage == null)
        {
            print("エラー：フェードパネルにコンポーネント Image が付いていません");
        }

        if (timeToFade <= 0)
        {
            timeToFade = float.Epsilon;/*0割りの回避*/
        }
    }

    private void Update()
    {
        if (UpdateFader == null)
        {
            return;
        }

        UpdateFader();
    }


    public void FadeOut(string nextSceneName)
    {
        this.nextSceneName = nextSceneName;
        gameObject.SetActive(true);
        UpdateFader = UpdateFadeOut;
        countTime = 0;
    }

    public void FadeIn()
    {
        UpdateFader = UpdateFadeIn;
        countTime = 0;
    }

    private void UpdateFadeOut()
    {
        if (fadeOuted)
        {
            return;
        }

        countTime += Time.deltaTime;
        if (countTime >= timeToFade)
        {
            fadeOuted = true;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);

            FadeIn();
            return;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, countTime / timeToFade);

    }

    private void UpdateFadeIn()
    {
        countTime += Time.deltaTime;
        if (countTime >= timeToFade)
        {
            fadeOuted = false;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
            gameObject.SetActive(false);
            return;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1 - countTime / timeToFade);
    }

    //c++でいう関数オブジェクトのようなもの
    private delegate void FadePanelUpdate();
    private FadePanelUpdate UpdateFader;

    /*変数*/
    private static bool fadeOuted = false;//非同期ロードのため。使っていない。2023/4/20
    [SerializeField,Header("フェード時間")] private float timeToFade;
    //[SerializeField]private float timeToInputInvalid;プレイヤーの入力無効時間　プレイヤー側で設定した方が良い
    private float countTime = 0;
    private Image fadeImage;
    private string nextSceneName;
}