using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{

    static public Fader instance
    {
        get
        {
            if (_instance == null)
            {
                /*キャンバスとフェードパネルを生成*/
                GameObject fadeCanvus = new GameObject("FadeCanvus");
                DontDestroyOnLoad(fadeCanvus);
                Canvas canvas = fadeCanvus.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 2;//大きいほど手前に表示される

                fadeCanvus.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

                GameObject fadePanel = new GameObject("FadePanel");
                fadePanel.transform.parent = fadeCanvus.transform;
                fadePanel.transform.localPosition = new Vector3(0, 0, 0);

                _instance = fadePanel.AddComponent<Fader>();
                _instance._fadeImage = fadePanel.AddComponent<Image>();

                _instance._fadeImage.color = new Color(1, 1, 1, 0);

                RectTransform rectTransform = fadePanel.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.sizeDelta = new Vector2(0, 0);
            }

            return _instance;
        }
    }

    public float fadeTime
    {
        get
        {
            return _fadeTime;
        }
        set
        {
            _fadeTime = value;
            if (_fadeTime <= 0)
            {
                _fadeTime = float.Epsilon;
            }
        }
    }

    public Color color
    {
        get
        {
            return _fadeImage.color;
        }
        set
        {
            _fadeImage.color = new Color(value.r, value.g, value.b, color.a);
        }
    }

    public Sprite texure
    {
        set
        {
            _fadeImage.sprite = value;
        }
    }

    private void Update()
    {
        UpdateFader();
    }


    public void FadeOut(int loadSceneNumber)
    {
        this._loadSceneNumber = loadSceneNumber;
        this.fadeTime = fadeTime;
        if (this.fadeTime == 0) this.fadeTime = float.Epsilon;

        gameObject.SetActive(true);
        UpdateFader = UpdateFadeOut;
        _countTime = 0;
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        UpdateFader = UpdateFadeIn;
        _countTime = 0;
    }

    private void UpdateFadeOut()
    {

        if (_fadeOuted)
        {
            return;
        }

        _countTime += Time.deltaTime;
        if (_countTime >= fadeTime)
        {
            _fadeOuted = true;
            _fadeImage.color = new Color(color.r, color.g, color.b, 1);
            UnityEngine.SceneManagement.SceneManager.LoadScene(_loadSceneNumber);

            FadeIn();
            SuzumuraTomoki.SceneManager.playerInput.Enable();
            return;
        }

        _fadeImage.color = new Color(color.r, color.g, color.b, _countTime / fadeTime);

    }

    private void UpdateFadeIn()
    {
        _countTime += Time.deltaTime;
        if (_countTime >= fadeTime)
        {

            _fadeOuted = false;
            _fadeImage.color = new Color(color.r, color.g, color.b, 0);
            gameObject.SetActive(false);
            return;
        }

        _fadeImage.color = new Color(color.r, color.g, color.b, 1 - _countTime / fadeTime);
    }


    //c++でいう関数オブジェクトのようなもの
    private delegate void FadePanelUpdate();
    private FadePanelUpdate UpdateFader;

    /*変数*/
    private static Fader _instance = null;
    private bool _fadeOuted = false;//非同期ロードのため。使っていない。2023/4/20
    private float _countTime = 0;
    private Image _fadeImage;
    private int _loadSceneNumber = 0;
    private float _fadeTime = 0.5f;//0にすると0割が発生します
}