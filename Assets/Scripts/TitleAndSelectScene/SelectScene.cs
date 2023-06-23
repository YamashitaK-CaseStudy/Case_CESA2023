using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SelectScene : MonoBehaviour
{
    public const int STAGE_NUM = 15;

    [SerializeField] private float _scrollAmount = 13.475f;
    [SerializeField] private float _scrollBaseSpeed = 13;
    [SerializeField] private float _scrollKeepSpeed = 26;
    static private float _selectedLocalPosX = 0;

    //[Header("種スコア設定")]
    //[SerializeField] private Vector2 _seedIconOffset = new Vector2(-650, -30);
    //[SerializeField] private float _seedIconScale = .05f;
    //[SerializeField] private Vector2 _seedScoreOffset = new Vector2(-550, -30);
    //[SerializeField] private float _seedScoreScale = .05f;


    private InputAction _actionDecision = null;
    private InputAction _actionStageSelectL = null;
    private InputAction _actionStageSelectR = null;
    private bool _scrolling = false;
    private float _scrollSpeed = 1;
    private float _lastReleased_IncStageNum = 0;
    private float _lastReleased_DecStageNum = 0;
    private RectTransform _rectTransform;
    private IEnumerator coroutine = null;



    public void StopInput()
    {
        _actionDecision.Disable();
        _actionStageSelectL.Disable();
        _actionStageSelectR.Disable();
    }
    public void EnableInput()
    {
        _actionDecision.Enable();
        _actionStageSelectL.Enable();
        _actionStageSelectR.Enable();
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _selectedLocalPosX = -((SuzumuraTomoki.SceneManager._currentStageNum - 1) * _scrollAmount);
        _rectTransform.localPosition = new Vector3(_selectedLocalPosX, 0, 0);
        coroutine = ScrollRight();//null回避

        InitInputAction();
    }


    private void OnDestroy()
    {
        _actionStageSelectR.started -= CallBackStarted_IncStageNum;
        _actionStageSelectR.canceled -= CallBackCanceled_IncStageNum;

        _actionStageSelectL.started -= CallBackStarted_DecStageNum;
        _actionStageSelectL.canceled -= CallBackCanceled_DecStageNum;
    }

    private void InitInputAction()
    {

        _actionDecision = SuzumuraTomoki.SceneManager.playerInput.FindAction("StageSelectEnter");

        if (_actionDecision == null)
        {
            Debug.LogError("PlayerInputに　アクション：StageSelectEnter　がありません");
        }
        else
        {
            _actionDecision.started += GoToStage;
            _actionDecision.Enable();
        }

        _actionStageSelectL = SuzumuraTomoki.SceneManager.playerInput.FindAction("StageSelectL");

        if (_actionStageSelectL == null)
        {
            Debug.LogError("PlayerInputに　アクション：StageSelectL　がありません");
        }
        else
        {
            _actionStageSelectL.started += CallBackStarted_DecStageNum;
            _actionStageSelectL.canceled += CallBackCanceled_DecStageNum;
        }

        _actionStageSelectR = SuzumuraTomoki.SceneManager.playerInput.FindAction("StageSelectR");

        if (_actionStageSelectR == null)
        {
            Debug.LogError("PlayerInputに　アクション：StageSelectR　がありません");
        }
        else
        {
            _actionStageSelectR.started += CallBackStarted_IncStageNum;
            _actionStageSelectR.canceled += CallBackCanceled_IncStageNum;
        }
    }


    private IEnumerator ScrollRight()
    {
        _scrolling = true;
        float startTime = Time.time;
        _lastReleased_IncStageNum = startTime;//ScrollLeftの再帰を止める

        Vector3 oldPos = _rectTransform.localPosition;

        float translationXPerSec = _scrollSpeed * (_selectedLocalPosX - oldPos.x) / _scrollAmount;
        float deltaXThisFrame = translationXPerSec * Time.deltaTime;

        while (Mathf.Abs(_selectedLocalPosX - _rectTransform.localPosition.x) > Mathf.Abs(deltaXThisFrame))
        {
            _rectTransform.localPosition += new Vector3(deltaXThisFrame, 0, 0);

            yield return null;

            deltaXThisFrame = translationXPerSec * Time.deltaTime;
        }

        _rectTransform.localPosition = new Vector3(_selectedLocalPosX, 0, 0);

        _scrolling = false;

        if (_lastReleased_DecStageNum < startTime)
        {
            _scrollSpeed = _scrollKeepSpeed;
            DecreaseStageNum();
        }

    }

    private IEnumerator ScrollLeft()
    {
        _scrolling = true;
        float startTime = Time.time;
        _lastReleased_DecStageNum = startTime;//ScrollRightの再帰を止める

        Vector3 oldPos = _rectTransform.localPosition;

        float translationXPerSec = _scrollSpeed * (_selectedLocalPosX - oldPos.x) / _scrollAmount;
        float deltaXThisFrame = translationXPerSec * Time.deltaTime;

        while (Mathf.Abs(_selectedLocalPosX - _rectTransform.localPosition.x) > Mathf.Abs(deltaXThisFrame))
        {
            _rectTransform.localPosition += new Vector3(deltaXThisFrame, 0, 0);

            yield return null;

            deltaXThisFrame = translationXPerSec * Time.deltaTime;
        }

        _rectTransform.localPosition = new Vector3(_selectedLocalPosX, 0, 0);

        _scrolling = false;

        if (_lastReleased_IncStageNum < startTime)
        {
            _scrollSpeed = _scrollKeepSpeed;
            IncreaseStageNum();
        }

    }

    private void DecreaseStageNum()
    {
        if (SuzumuraTomoki.SceneManager._currentStageNum <= 1)
        {
            return;
        }

        SuzumuraTomoki.SceneManager._currentStageNum--;
        _selectedLocalPosX += _scrollAmount;

        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Slide);

        var work = coroutine;//再帰に対応するため後でストップ。先にストップするとこのメソッドも終了してしまう？
        coroutine = ScrollRight();
        StartCoroutine(coroutine);
        StopCoroutine(work);
    }

    private void IncreaseStageNum()
    {
        if (SuzumuraTomoki.SceneManager._currentStageNum >= STAGE_NUM)
        {
            return;
        }

        SuzumuraTomoki.SceneManager._currentStageNum++;
        _selectedLocalPosX -= _scrollAmount;

        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Slide);

        var work = coroutine;//再帰に対応するため後でストップ。先にストップするとこのメソッドも終了してしまう？
        coroutine = ScrollLeft();
        StartCoroutine(coroutine);
        StopCoroutine(work);
    }

    private void CallBackStarted_DecStageNum(InputAction.CallbackContext context)
    {
        _scrollSpeed = _scrollBaseSpeed;
        DecreaseStageNum();
    }

    private void CallBackStarted_IncStageNum(InputAction.CallbackContext context)
    {
        _scrollSpeed = _scrollBaseSpeed;
        IncreaseStageNum();
    }


    private void GoToStage(InputAction.CallbackContext context)
    {
        if (_scrolling)
        {
            return;
        }

        bool success = SuzumuraTomoki.SceneManager.LoadStage(SuzumuraTomoki.SceneManager._currentStageNum);

        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Decision3);
        SystemSoundManager.Instance.StopBGMWithFade(0.5f);

        if (!success)
        {
            //TODO:無効な入力を伝えるSE
            print("ステージセレクト「ステージが存在しません」");
            return;
        }

        _actionDecision.started -= GoToStage;
        _actionDecision.Disable();
    }

    private void CallBackCanceled_DecStageNum(InputAction.CallbackContext context)
    {
        _lastReleased_DecStageNum = Time.time;//数百時間動かしていると有効桁数が足りなくなってバグる
    }

    private void CallBackCanceled_IncStageNum(InputAction.CallbackContext context)
    {
        _lastReleased_IncStageNum = Time.time;//数百時間動かしていると有効桁数が足りなくなってバグる
    }

    private void OnValidate()
    {
        if (_scrollBaseSpeed <= 0)
        {
            _scrollBaseSpeed = 0.01f;
        }

        if (_scrollKeepSpeed <= 0)
        {
            _scrollKeepSpeed = 0.01f;
        }
    }
}
