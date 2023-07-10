using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using SuzumuraTomoki;

public class SelectScene : MonoBehaviour
{
    /*�^��`*/

    [System.Serializable]
    private struct StageData
    {
        public Sprite thumbnail;
        public int unlockNumber;
    }

    /*����J�ϐ�*/

    static private float _selectedLocalPosX = 0;

    [SerializeField] private GameObject _selectPiecePrefab;
    [SerializeField] private GameObject _buttonUi_R_Prefab;
    [SerializeField] private GameObject _buttonUi_L_Prefab;
    [SerializeField] private GameObject _lockUi_Prefab;
    [SerializeField] private Sprite _stageNumBoard_ExtraStage;
    [Header("�X�e�[�W�摜�ƕK�v�X�R�A\n�r���}����Element���E�N���b�N�ADuplicate..�ŏo���܂�\n�r���h�ݒ�ƕ��т���v�����ĉ�����")]
    [SerializeField] private StageData[] _stageDataArray;
    [SerializeField] private float _scrollAmount = 13.475f;
    [SerializeField] private float _scrollBaseSpeed = 13;
    [SerializeField] private float _scrollKeepSpeed = 26;
    [SerializeField] private GameObject _storyArchive = null;
    [SerializeField] private PlayerInput _playerInput = null;

    private InputAction _actionDecision = null;
    private InputAction _actionStageSelectL = null;
    private InputAction _actionStageSelectR = null;
    private InputAction _actionStorySelect = null;
    private bool _scrolling = false;
    private float _scrollSpeed = 1;
    private float _lastReleased_IncStageNum = 0;
    private float _lastReleased_DecStageNum = 0;
    private RectTransform _rectTransform;
    private IEnumerator coroutine = null;

    /*Unity�֐�*/

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _selectedLocalPosX = -((SceneManager._currentStageNum - 1) * _scrollAmount);
        _rectTransform.localPosition = new Vector3(_selectedLocalPosX, 0, 0);
        coroutine = ScrollRight();//null���

        /*�I��������*/
        GameObject selectPieceInstance = null;
        for (int i = 0, stageNum = 1; i < _stageDataArray.Length; ++i)
        {
            selectPieceInstance = Instantiate(_selectPiecePrefab, transform);
            selectPieceInstance.transform.localPosition = new Vector3(i * _scrollAmount, 0, 0);

            ref StageData stageData = ref _stageDataArray[i];
            selectPieceInstance.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = stageData.thumbnail;
            if (stageData.unlockNumber == 0)
            {
                selectPieceInstance.transform.GetChild(3).GetChild(0).GetComponent<UiNumberBehavior>().Number = stageNum++;
            }
            else
            {
                selectPieceInstance.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = _stageNumBoard_ExtraStage;
                Destroy(selectPieceInstance.transform.GetChild(3));
                if (UiSeedBehavior.seedScore.obtained < stageData.unlockNumber)
                {
                    Instantiate(_lockUi_Prefab, selectPieceInstance.transform);
                }
            }

            /*�{�^��UILR����*/
            if (i != 0 || i != _stageDataArray.Length - 1)
            {
                Instantiate(_buttonUi_L_Prefab, selectPieceInstance.transform);
                Instantiate(_buttonUi_R_Prefab, selectPieceInstance.transform);
            }
            else if (i == 0)
            {
                Instantiate(_buttonUi_R_Prefab, selectPieceInstance.transform);
            }
            else
            {
                Instantiate(_buttonUi_L_Prefab, selectPieceInstance.transform);
            }
        }

        InitInputAction();
    }


    private void OnDestroy()
    {
        _actionStageSelectR.started -= CallBackStarted_IncStageNum;
        _actionStageSelectR.canceled -= CallBackCanceled_IncStageNum;

        _actionStageSelectL.started -= CallBackStarted_DecStageNum;
        _actionStageSelectL.canceled -= CallBackCanceled_DecStageNum;

        _actionStorySelect.started -= CallBack_Started_SwitchStoryArchive;
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

    /*���J*/

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

    /*����*/

    private void InitInputAction()
    {

        _actionDecision = SceneManager.playerInput.FindAction("StageSelectEnter");

        if (_actionDecision == null)
        {
            Debug.LogError("PlayerInput�Ɂ@�A�N�V�����FStageSelectEnter�@������܂���");
        }
        else
        {
            _actionDecision.started += GoToStage;
            _actionDecision.Enable();
        }

        _actionStageSelectL = SceneManager.playerInput.FindAction("StageSelectL");

        if (_actionStageSelectL == null)
        {
            Debug.LogError("PlayerInput�Ɂ@�A�N�V�����FStageSelectL�@������܂���");
        }
        else
        {
            _actionStageSelectL.started += CallBackStarted_DecStageNum;
            _actionStageSelectL.canceled += CallBackCanceled_DecStageNum;
        }

        _actionStageSelectR = SceneManager.playerInput.FindAction("StageSelectR");

        if (_actionStageSelectR == null)
        {
            Debug.LogError("PlayerInput�Ɂ@�A�N�V�����FStageSelectR�@������܂���");
        }
        else
        {
            _actionStageSelectR.started += CallBackStarted_IncStageNum;
            _actionStageSelectR.canceled += CallBackCanceled_IncStageNum;
        }

        _actionStorySelect = SceneManager.playerInput.FindAction("StoryArchive");

        if (_actionStorySelect == null)
        {
            Debug.LogError("�C���v�b�g�A�N�V�����FStoryArchive�@��������܂���ł���");
        }

        _actionStorySelect.started += CallBack_Started_SwitchStoryArchive;

    }


    private IEnumerator ScrollRight()
    {
        _scrolling = true;
        float startTime = Time.time;
        //_lastReleased_IncStageNum = startTime;//ScrollLeft�̍ċA���~�߂�

        Vector3 oldPos = _rectTransform.localPosition;

        float gainSpeed = (_selectedLocalPosX - oldPos.x) / _scrollAmount;
        gainSpeed = Mathf.Abs(gainSpeed) > 1 ? gainSpeed : gainSpeed / Mathf.Abs(gainSpeed);

        float translationXPerSec = _scrollSpeed * gainSpeed;
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
        //_lastReleased_DecStageNum = startTime;//ScrollRight�̍ċA���~�߂�

        Vector3 oldPos = _rectTransform.localPosition;

        float gainSpeed = (_selectedLocalPosX - oldPos.x) / _scrollAmount;
        gainSpeed = Mathf.Abs(gainSpeed) > 1 ? gainSpeed : gainSpeed / Mathf.Abs(gainSpeed);

        float translationXPerSec = _scrollSpeed * gainSpeed;
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
        if (SceneManager._currentStageNum <= 1)
        {
            return;
        }

        SceneManager._currentStageNum--;
        _selectedLocalPosX += _scrollAmount;

        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Slide);

        var work = coroutine;//�ċA�ɑΉ����邽�ߌ�ŃX�g�b�v�B��ɃX�g�b�v����Ƃ��̃��\�b�h���I�����Ă��܂��H
        coroutine = ScrollRight();
        StartCoroutine(coroutine);
        StopCoroutine(work);
    }

    private void IncreaseStageNum()
    {
        if (SceneManager._currentStageNum >= _stageDataArray.Length)
        {
            return;
        }

        SceneManager._currentStageNum++;
        _selectedLocalPosX -= _scrollAmount;

        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Slide);

        var work = coroutine;//�ċA�ɑΉ����邽�ߌ�ŃX�g�b�v�B��ɃX�g�b�v����Ƃ��̃��\�b�h���I�����Ă��܂��H
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

        if (SelectFilmBehavior.SeedScore < _stageDataArray[SceneManager._currentStageNum - 1].unlockNumber)
        {
            //TODO:�����ȓ��͂�`����SE
            return;
        }

        bool success = SceneManager.LoadStage(SceneManager._currentStageNum);

        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Decision3);
        SystemSoundManager.Instance.StopBGMWithFade(0.5f);

        if (!success)
        {
            //TODO:�����ȓ��͂�`����SE
            print("�X�e�[�W�����݂��܂���B�r���h�ݒ���m�F���Ă��������B");
            return;
        }

        _actionDecision.started -= GoToStage;
        _actionDecision.Disable();
    }

    private void CallBackCanceled_DecStageNum(InputAction.CallbackContext context)
    {
        _lastReleased_DecStageNum = Time.time;//���S���ԓ������Ă���ƗL������������Ȃ��Ȃ��ăo�O��
    }

    private void CallBackCanceled_IncStageNum(InputAction.CallbackContext context)
    {
        _lastReleased_IncStageNum = Time.time;//���S���ԓ������Ă���ƗL������������Ȃ��Ȃ��ăo�O��
    }

    private void CallBack_Started_SwitchStoryArchive(InputAction.CallbackContext context)
    {
        _playerInput.SwitchCurrentActionMap("StoryArchive");

        //�X�e�[�W�Z���N�g���I�t
        gameObject.SetActive(false);
        //�X�g�[���[�A�[�J�C�u��L���ɂ���
        _storyArchive.SetActive(true);
    }

}
