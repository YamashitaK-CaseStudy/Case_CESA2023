using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectUiFader : MonoBehaviour
{
    void Start()
    {
        if (SuzumuraTomoki.SceneManager.titleInitState == SuzumuraTomoki.SceneManager.TitleInitState.STAGE_SELECT)
        {
            _canvasGroup.alpha = 1;
            _filmBehavior.EnableInput();
            return;
        }

        _canvasGroup.alpha = 0;
        _filmBehavior.DisableInput();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float countTime = 0;

        //待機
        while (countTime < _waitTime)
        {
            countTime += Time.deltaTime;
            yield return null;
        }

        countTime = 0;

        //フェードイン
        while (_canvasGroup.alpha < 1)
        {
            countTime += Time.deltaTime;
            _canvasGroup.alpha = countTime / _fadeTime;
            yield return null;
        }

        _filmBehavior.EnableInput();
    }

    private void OnValidate()
    {
        //不正な値なら補正する
        if (_fadeTime <= 0)
        {
            _fadeTime = float.Epsilon;
        }
    }

    [SerializeField] private float _waitTime = 3;
    [SerializeField] private float _fadeTime = 1;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private SelectFilmBehavior _filmBehavior;
}
