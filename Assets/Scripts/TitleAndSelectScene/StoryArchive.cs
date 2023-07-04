using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class StoryArchive : MonoBehaviour
{
    /*�N���X�E�\����*/

    [System.Serializable]
    private struct StoryMovie
    {
        public Sprite _image;
        public int _unlockNum;
    }

    /*Unity�֐�*/

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _selectedLocalPosX = -((_currentStoryNum - 1) * _distance);
        _rectTransform.localPosition = new Vector3(_selectedLocalPosX, 0, 0);

        for (int i = 0, length = _storyArray.Length; i < length; ++i)
        {
            var instance = Instantiate(_pfStoryThumbnail, transform);
            instance.transform.localPosition = new Vector3(_distance * i, 0, 0);
            //instance.transform.parent = transform;
            instance.GetComponent<UnityEngine.UI.Image>().sprite = _storyArray[i]._image;
        }

        _actionMoveRight = SuzumuraTomoki.SceneManager.inputSheet.FindActionMap("StoryArchive").FindAction("MoveRight");

        if (_actionMoveRight == null)
        {
            Debug.LogError("�C���v�b�g�A�N�V�����FMoveRight�@��������܂���ł���");
        }

        _actionMoveRight.started += CallBack_Started_MoveRight;

        _actionMoveLeft = SuzumuraTomoki.SceneManager.inputSheet.FindActionMap("StoryArchive").FindAction("MoveLeft");

        if (_actionMoveLeft == null)
        {
            Debug.LogError("�C���v�b�g�A�N�V�����FMoveLeft�@��������܂���ł���");
        }

        _actionMoveLeft.started += CallBack_Started_MoveLeft;
    }

    private void OnDestroy()
    {
        _actionMoveRight.started -= CallBack_Started_MoveRight;
        _actionMoveLeft.started -= CallBack_Started_MoveLeft;
    }

    /*���J*/

    /*����J*/

    private void CallBack_Started_MoveLeft(InputAction.CallbackContext context)
    {
        _scrollSpeed = _scrollBaseSpeed;
        BeginScrollRight();
    }

    private void CallBack_Started_MoveRight(InputAction.CallbackContext context)
    {
        _scrollSpeed = _scrollBaseSpeed;
        BeginScrollLeft();
    }

    private void BeginScrollRight()
    {
        if (SuzumuraTomoki.SceneManager._currentStageNum <= 1)
        {
            return;
        }

        --_currentStoryNum;
        _selectedLocalPosX += _distance;

        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Slide);

        var work = coroutine;//�ċA�ɑΉ����邽�ߌ�ŃX�g�b�v�B��ɃX�g�b�v����Ƃ��̃��\�b�h���I�����Ă��܂��H
        coroutine = ScrollRight();
        StartCoroutine(coroutine);
        StopCoroutine(work);
    }

    private void BeginScrollLeft()
    {
        if (_currentStoryNum >= _storyArray.Length)
        {
            return;
        }

        ++_currentStoryNum;
        _selectedLocalPosX -= _distance;

        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Slide);

        var work = coroutine;//�ċA�ɑΉ����邽�ߌ�ŃX�g�b�v�B��ɃX�g�b�v����Ƃ��̃��\�b�h���I�����Ă��܂��H
        coroutine = ScrollLeft();
        StartCoroutine(coroutine);
        StopCoroutine(work);
    }

    private IEnumerator ScrollRight()
    {
        _scrolling = true;
        float startTime = Time.time;

        Vector3 oldPos = _rectTransform.localPosition;

        float gainSpeed = (_selectedLocalPosX - oldPos.x) / _distance;
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
            BeginScrollRight();
        }

    }

    private IEnumerator ScrollLeft()
    {
        _scrolling = true;
        float startTime = Time.time;

        Vector3 oldPos = _rectTransform.localPosition;

        float gainSpeed = (_selectedLocalPosX - oldPos.x) / _distance;
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
            BeginScrollLeft();
        }

    }

    static private float _selectedLocalPosX = 0;
    static private int _currentStoryNum = 1;

    [SerializeField] private StoryMovie[] _storyArray;
    [SerializeField] private GameObject _pfStoryThumbnail;
    [SerializeField] private float _distance = 13.5f;
    [SerializeField] private float _scrollBaseSpeed = 13;
    [SerializeField] private float _scrollKeepSpeed = 26;

    private InputAction _actionMoveRight = null;
    private InputAction _actionMoveLeft = null;
    private bool _scrolling = false;
    private float _scrollSpeed = 1;
    private float _lastReleased_IncStageNum = 0;
    private float _lastReleased_DecStageNum = 0;
    private IEnumerator coroutine = null;
    private RectTransform _rectTransform;

}
