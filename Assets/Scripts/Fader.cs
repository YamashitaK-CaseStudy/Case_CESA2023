using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    /*定数*/
    public enum State
    {
        NONE,
        FADE_DONE,
        FADING_OUT,
        FADING_IN
    }

    /*静的公開メンバ*/
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

                _instance.gameObject.SetActive(false);
            }

            return _instance;
        }
    }

    static public UnityEngine.InputSystem.InputActionMap stopInput
    {
        set
        {
            if (instance._state == State.NONE)
            {
                return;
            }
            value.Disable();
            _listStopInput.Add(value);
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
        switch (_state)
        {
            case State.NONE:
                _countTime = 0;
                break;

            case State.FADE_DONE:
                return;
                //break;

            case State.FADING_IN:
                _countTime = fadeTime - _countTime;
                break;

            case State.FADING_OUT:
                return;
                //break;
        }

        _state = State.FADING_OUT;

        this._loadSceneNumber = loadSceneNumber;

        gameObject.SetActive(true);
        UpdateFader = UpdateFadeOut;
    }

    public void FadeIn()
    {
        switch (_state)
        {
            case State.FADE_DONE:
                _countTime = 0;
                break;

            default:
                return;
                //break;
        }

        _state = State.FADING_IN;
        gameObject.SetActive(true);
        UpdateFader = UpdateFadeIn;
    }

    private void UpdateFadeOut()
    {

        _countTime += Time.deltaTime;
        if (_countTime >= fadeTime)
        {
            _state = State.FADE_DONE;
            _fadeImage.color = new Color(color.r, color.g, color.b, 1);
            UnityEngine.SceneManagement.SceneManager.LoadScene(_loadSceneNumber);

            FadeIn();
            return;
        }

        _fadeImage.color = new Color(color.r, color.g, color.b, _countTime / fadeTime);

    }

    private void UpdateFadeIn()
    {
        _countTime += Time.deltaTime;
        if (_countTime >= fadeTime)
        {
            SuzumuraTomoki.SceneManager.playerInput.Enable();
            foreach (var inAcMap in _listStopInput)
            {
                inAcMap.Enable();
            }
            _listStopInput.Clear();

            _state = State.NONE;
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
    static private Fader _instance = null;
    static private List<UnityEngine.InputSystem.InputActionMap> _listStopInput = new List<UnityEngine.InputSystem.InputActionMap>();
    private State _state = State.NONE;
    private float _countTime = 0;
    private Image _fadeImage;
    private int _loadSceneNumber = 0;
    private float _fadeTime = 0.5f;//0にすると0割が発生します
}