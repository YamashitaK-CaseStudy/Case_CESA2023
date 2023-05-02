using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private void Start()
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

        if (fadeOut)
        {
            gameObject.SetActive(true);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            FadeIn();
        }
        else
        {
            gameObject.SetActive(false);//フェイド中以外はUpdate()が呼ばれないようにゲームオブジェクトを無効状態にする
        }
    }

    private void Update()
    {
        UpdateFader();
    }


    public void FadeOut(FadeOutCompletionReceiver receiver)
    {
        this.receiver = receiver;
        gameObject.SetActive(true);
        UpdateFader = UpdateFadeOut;
        countTime = 0;
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        UpdateFader = UpdateFadeIn;
        countTime = 0;
    }

    private void UpdateFadeOut()
    {
        countTime += Time.deltaTime;
        if (countTime >= timeToFade)
        {
            fadeOut = true;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            receiver.ProcessAfterFadeOut();
            gameObject.SetActive(false);
            return;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, countTime / timeToFade);

    }

    private void UpdateFadeIn()
    {
        countTime += Time.deltaTime;
        if (countTime >= timeToFade)
        {
            fadeOut = false;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
            gameObject.SetActive(false);
            return;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1 - countTime / timeToFade);
    }

    private static bool fadeOut = false;
    [SerializeField] private float timeToFade;
    //[SerializeField]private float timeToInputInvalid;プレイヤーの入力無効時間　プレイヤー側で設定した方が良い
    private float countTime = 0;
    Image fadeImage;
    private delegate void FadePanelUpdate();
    FadePanelUpdate UpdateFader;
    FadeOutCompletionReceiver receiver = null;
}

public abstract class FadeOutCompletionReceiver : MonoBehaviour
{
    public abstract void ProcessAfterFadeOut();
}