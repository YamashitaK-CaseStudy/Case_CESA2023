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

    //[Header("��X�R�A�ݒ�")]
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
            Debug.LogError("PlayerInput�Ɂ@�A�N�V�����FStageSelectEnter�@������܂���");
        }
        else
        {
            _actionDecision.started += GoToStage;
            _actionDecision.Enable();
        }

        _actionStageSelectL = SuzumuraTomoki.SceneManager.playerInput.FindAction("StageSelectL");

        if (_actionStageSelectL == null)
        {
            Debug.LogError("PlayerInput�Ɂ@�A�N�V�����FStageSelectL�@������܂���");
        }
        else
        {
            _actionStageSelectL.started += DecreaseStageNum;
        }

        _actionStageSelectR = SuzumuraTomoki.SceneManager.playerInput.FindAction("StageSelectR");

        if (_actionStageSelectR == null)
        {
            Debug.LogError("PlayerInput�Ɂ@�A�N�V�����FStageSelectR�@������܂���");
        }
        else
        {
            _actionStageSelectR.started += IncreaseStageNum;
        }
    }


    // �E�ɃX�N���[��
    private IEnumerator ScrollRight()
    {
        _stopInput = true;
        // �X�e�[�W�ԍ��ǉ�

        Vector3 oldPos = _rectTransform.localPosition;

        while (Mathf.Abs(oldPos.x - _rectTransform.localPosition.x) < _scroolAmount)
        {
            yield return null;
            _rectTransform.localPosition -= new Vector3(_scrollSpeed * Time.deltaTime, 0, 0);
        }

        _rectTransform.localPosition = new Vector3(_scrollTotalAmount, 0, 0);
        _stopInput = false;
    }

    // ���ɃX�N���[��
    private IEnumerator ScrollLeft()
    {
        _stopInput = true;
        // �X�e�[�W�ԍ��ǉ�

        Vector3 oldPos = _rectTransform.localPosition;

        while (Mathf.Abs(oldPos.x - _rectTransform.localPosition.x) < _scroolAmount)
        {
            yield return null;
            _rectTransform.localPosition += new Vector3(_scrollSpeed * Time.deltaTime, 0, 0);
        }

        _rectTransform.localPosition = new Vector3(_scrollTotalAmount, 0, 0);
        _stopInput = false;
    }

    // �X�e�[�W�ԍ������炷
    private void DecreaseStageNum(InputAction.CallbackContext context){
		if (SuzumuraTomoki.SceneManager._currentStageNum > 0)
		{
            SuzumuraTomoki.SceneManager._currentStageNum--;
            _scrollTotalAmount += _scroolAmount;
            StartCoroutine(ScrollLeft());
            
        }
	}

    // �X�e�[�W�ԍ��𑝂₷
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
            //TODO:�����ȓ��͂�`����SE
            print("�X�e�[�W�Z���N�g�u�X�e�[�W�����݂��܂���v");
            return;
        }

        _actionDecision.started -= GoToStage;
        _actionDecision.Disable();
    }
}
