using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        fadeImage = gameObject.GetComponent<Image>();
        if (fadeImage == null)
        {
            print("�G���[�F�t�F�[�h�p�l���ɃR���|�[�l���g Image ���t���Ă��܂���");
        }

        if (timeToFade <= 0)
        {
            timeToFade = float.Epsilon;/*0����̉��*/
        }

        if (fadeOuted)
        {
            gameObject.SetActive(true);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            FadeIn();
        }
        else
        {
            gameObject.SetActive(false);//�t�F�C�h���ȊO��Update()���Ă΂�Ȃ��悤�ɃQ�[���I�u�W�F�N�g�𖳌���Ԃɂ���
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
        if (fadeOuted)
        {
            return;
        }

        countTime += Time.deltaTime;
        if (countTime >= timeToFade)
        {
            fadeOuted = true;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            receiver.ProcessAfterFadeOut();
            //gameObject.SetActive(false);
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

    private static bool fadeOuted = false;
    [SerializeField] private float timeToFade;
    //[SerializeField]private float timeToInputInvalid;�v���C���[�̓��͖������ԁ@�v���C���[���Őݒ肵�������ǂ�
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