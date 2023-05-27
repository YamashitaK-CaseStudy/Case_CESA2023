using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SelectScene : MonoBehaviour
{
    public const int STAGE_NUM = 15;

    [SerializeField] private float _scroolAmount = 13.475f;
    [SerializeField] private float _scrollSpeed = 3;
    static private float _scrollTotalAmount = 0;

    //[Header("種スコア設定")]
    //[SerializeField] private Vector2 _seedIconOffset = new Vector2(-650, -30);
    //[SerializeField] private float _seedIconScale = .05f;
    //[SerializeField] private Vector2 _seedScoreOffset = new Vector2(-550, -30);
    //[SerializeField] private float _seedScoreScale = .05f;


    private InputAction _actionDecision = null;
    private InputAction _actionStageSelectL = null;
    private InputAction _actionStageSelectR = null;
    private bool _stopInput = false;

    private RectTransform _rectTransform;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
        InitInputAction();
    }


    private void OnDestroy()
    {
        _actionStageSelectR.started -= IncreaseStageNum;
        _actionStageSelectL.started -= DecreaseStageNum;
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
            _actionStageSelectL.started += DecreaseStageNum;
        }

        _actionStageSelectR = SuzumuraTomoki.SceneManager.playerInput.FindAction("StageSelectR");

        if (_actionStageSelectR == null)
        {
            Debug.LogError("PlayerInputに　アクション：StageSelectR　がありません");
        }
        else
        {
            _actionStageSelectR.started += IncreaseStageNum;
        }
    }


    // 右にスクロール
    private IEnumerator ScrollRight()
    {
        _stopInput = true;
        // ステージ番号追加

        Vector3 oldPos = _rectTransform.localPosition;

        while (Mathf.Abs(oldPos.x - _rectTransform.localPosition.x) < _scroolAmount)
        {
            yield return null;
            _rectTransform.localPosition -= new Vector3(_scrollSpeed * Time.deltaTime, 0, 0);
        }

        _rectTransform.localPosition = new Vector3(_scrollTotalAmount, 0, 0);
        _stopInput = false;
    }

    // 左にスクロール
    private IEnumerator ScrollLeft()
    {
        _stopInput = true;
        // ステージ番号追加

        Vector3 oldPos = _rectTransform.localPosition;

        while (Mathf.Abs(oldPos.x - _rectTransform.localPosition.x) < _scroolAmount)
        {
            yield return null;
            _rectTransform.localPosition += new Vector3(_scrollSpeed * Time.deltaTime, 0, 0);
        }

        _rectTransform.localPosition = new Vector3(_scrollTotalAmount, 0, 0);
        _stopInput = false;
    }

    // ステージ番号を減らす
    private void DecreaseStageNum(InputAction.CallbackContext context){
		if (SuzumuraTomoki.SceneManager._currentStageNum > 0)
		{
            SuzumuraTomoki.SceneManager._currentStageNum--;
            _scrollTotalAmount += _scroolAmount;
            StartCoroutine(ScrollLeft());
            
        }
	}

    // ステージ番号を増やす
    private void IncreaseStageNum(InputAction.CallbackContext context)
    {
		if (SuzumuraTomoki.SceneManager._currentStageNum < STAGE_NUM)
		{
            SuzumuraTomoki.SceneManager._currentStageNum++;
            _scrollTotalAmount -= _scroolAmount;
            StartCoroutine(ScrollRight());

        }
    }

    private void GoToStage(InputAction.CallbackContext context)
    {
        if (_stopInput)
        {
            return;
        }

        bool success = SuzumuraTomoki.SceneManager.LoadStage(SuzumuraTomoki.SceneManager._currentStageNum);

        if (!success)
        {
            //TODO:無効な入力を伝えるSE
            print("ステージセレクト「ステージが存在しません」");
            return;
        }

        _actionDecision.started -= GoToStage;
        _actionDecision.Disable();
    }
}
