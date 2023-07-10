using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueBehv : MonoBehaviour
{
    enum State
    {
        DISPLAY_PAGE,
        CHANGE_PAGE,
        FADE_OUT,
    }

    private delegate void Callback();

    private void Awake()
    {
        _canvusGroup.alpha = 0;
        ref var firstPage = ref storyArray[0];
        _thumbnail.sprite = firstPage._image;
        _text.text = firstPage._storyText;
        _thumbnail2.color = new Color(1, 1, 1, 0);
        _text2.color = new Color(1, 1, 1, 0);

        StartCoroutine(FadeIn_Coroutine());
    }

    private void OnValidate()
    {
        if (_fadeTime <= 0)
        {
            _fadeTime = 0.1f;
        }
    }

    private void Update()
    {
        if (_doneFadeIn == false)
        {
            return;
        }

        switch (_state)
        {
            case State.DISPLAY_PAGE:
                _countTime += Time.deltaTime;

                if (_countTime >= _pageWaitTime || _actionDecision.triggered)
                {
                    _countTime = 0;

                    if (_pageId + 1 >= storyArray.Length)
                    {
                        _state = State.FADE_OUT;
                        return;
                    }

                    _state = State.CHANGE_PAGE;

                    _thumbnail2.sprite = _thumbnail.sprite;
                    _thumbnail2.color = Color.white;
                    _text2.text = _text.text;
                    _text2.color = Color.white;

                    ref var nextPage = ref storyArray[++_pageId];
                    _thumbnail.sprite = nextPage._image;
                    _text.text = nextPage._storyText;
                    _text.color = Color.clear;

                    if (nextPage._soundArray.Length != 0)
                    {
                        GameSoundManager.Instance.PlayGameSE(nextPage._soundArray[0]);
                    }
                }
                break;

            case State.CHANGE_PAGE:
                _countTime += Time.deltaTime;

                float alpha = _countTime / _pageChangeTime;

                _thumbnail2.color = new Color(1, 1, 1, 1.0f - alpha);
                _text2.color = new Color(1, 1, 1, 1.0f - alpha);
                _text.color = new Color(1, 1, 1, alpha);

                if (alpha >= 1)
                {
                    _state = State.DISPLAY_PAGE;
                }

                break;

            case State.FADE_OUT:
                _countTime += Time.deltaTime;
                _canvusGroup.alpha = 1 - _countTime / _fadeTime;

                if (_canvusGroup.alpha <= 0)
                {
                    _callback.Invoke();
                    transform.parent.gameObject.SetActive(false);//ƒLƒƒƒ“ƒoƒX
                }
                break;
        }

    }

    IEnumerator FadeIn_Coroutine()
    {
        float countTime = 0;
        while (_canvusGroup.alpha < 1)
        {
            countTime += Time.deltaTime;
            _canvusGroup.alpha = countTime / _fadeTime;
            yield return null;
        }

        _actionDecision = SuzumuraTomoki.SceneManager.playerInput.FindAction("Decision");
    }

    [SerializeField] private StoryPage[] storyArray;
    [SerializeField] private UnityEngine.UI.Image _thumbnail;
    [SerializeField] private UnityEngine.UI.Image _thumbnail2;
    [SerializeField] private UnityEngine.UI.Text _text;
    [SerializeField] private UnityEngine.UI.Text _text2;
    [SerializeField] private CanvasGroup _canvusGroup;
    [SerializeField] private float _pageWaitTime = 3.0f;
    [SerializeField] private float _pageChangeTime = 0.3f;
    [SerializeField] private UnityEngine.Events.UnityEvent _callback;

    private int _pageId = 0;
    private float _fadeTime = 0.3f;
    private float _countTime = 0;
    private bool _doneFadeIn = false;
    private State _state = State.DISPLAY_PAGE;
    private UnityEngine.InputSystem.InputAction _actionDecision;
}
