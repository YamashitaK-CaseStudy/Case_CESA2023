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
            print("�G���[�F�t�F�[�h�p�l���ɃR���|�[�l���g Image ���t���Ă��܂���");
        }

        if (timeToFade <= 0)
        {
            timeToFade = float.Epsilon;/*0����̉��*/
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

    //c++�ł����֐��I�u�W�F�N�g�̂悤�Ȃ���
    private delegate void FadePanelUpdate();
    private FadePanelUpdate UpdateFader;

    /*�ϐ�*/
    private static bool fadeOuted = false;//�񓯊����[�h�̂��߁B�g���Ă��Ȃ��B2023/4/20
    [SerializeField,Header("�t�F�[�h����")] private float timeToFade;
    //[SerializeField]private float timeToInputInvalid;�v���C���[�̓��͖������ԁ@�v���C���[���Őݒ肵�������ǂ�
    private float countTime = 0;
    private Image fadeImage;
    private string nextSceneName;
}