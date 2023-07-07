using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class StoryArchive : MonoBehaviour
{
    /*目次
     * クラス・構造体
     * Unity関数
     * 非公開関数
     * 非公開変数
     */

    /*クラス・構造体*/

    [System.Serializable]
    private struct StoryData
    {
        public Sprite _image;
        public int _unlockNum;
    }

    /*Unity関数*/

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _selectedLocalPosX = -((_currentStoryNum - 1) * _distance);
        _rectTransform.localPosition = new Vector3(_selectedLocalPosX, 0, 0);

        //生成
        for (int i = 0, length = _storyArray.Length; i < length; ++i)
        {
            var thumbnailInstance = Instantiate(_storyThumbnailPrefab, transform);
            thumbnailInstance.transform.localPosition = new Vector3(_distance * i, 0, 0);
            ref var storyData = ref _storyArray[i];
            thumbnailInstance.GetComponent<UnityEngine.UI.Image>().sprite = storyData._image;

            if(SelectFilmBehavior.SeedScore< storyData._unlockNum)
            {
                Instantiate(_lockUiPrefab, thumbnailInstance.transform);
            }
        }

        _actionMoveRight = _playerInput.currentActionMap.FindAction("MoveRight");

        if (_actionMoveRight == null)
        {
            Debug.LogError("インプットアクション：MoveRight　が見つかりませんでした");
        }

        _actionMoveRight.started += CallBack_Started_MoveRight;

        _actionMoveLeft = _playerInput.currentActionMap.FindAction("MoveLeft");

        if (_actionMoveLeft == null)
        {
            Debug.LogError("インプットアクション：MoveLeft　が見つかりませんでした");
        }

        _actionMoveLeft.started += CallBack_Started_MoveLeft;


        _actionStageSelect = _playerInput.currentActionMap.FindAction("StageSelect");

        if (_actionStageSelect == null)
        {
            Debug.LogError("インプットアクション：StageSelect　が見つかりませんでした");
        }

        _actionStageSelect.started += CallBack_Started_SwitchStageSelect;
    }

    private void OnDestroy()
    {
        _actionMoveRight.started -= CallBack_Started_MoveRight;
        _actionMoveLeft.started -= CallBack_Started_MoveLeft;
        _actionStageSelect.started -= CallBack_Started_SwitchStageSelect;
    }

    /*公開*/

    /*非公開関数*/

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
        if (_currentStoryNum <= 1)
        {
            return;
        }

        --_currentStoryNum;
        _selectedLocalPosX += _distance;

        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Slide);

        var oldCoroutine = coroutine;//再帰に対応するため後でストップ。先にストップするとこのメソッドも終了してしまう？
        coroutine = ScrollRight();
        StartCoroutine(coroutine);
        if (oldCoroutine != null)
        {
            StopCoroutine(oldCoroutine);
        }
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

        var oldCoroutine = coroutine;//再帰に対応するため後でストップ。先にストップするとこのメソッドも終了してしまう？
        coroutine = ScrollLeft();
        StartCoroutine(coroutine);
        if (oldCoroutine != null)
        {
            StopCoroutine(oldCoroutine);
        }
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

    private void CallBack_Started_SwitchStageSelect(InputAction.CallbackContext context)
    {
        //ストーリーアーカイブを無効にする
        gameObject.SetActive(false);
        //ステージセレクトを有効にする
        _stageSelect.SetActive(true);

        _playerInput.SwitchCurrentActionMap("Player");
    }

    /*非公開変数*/

    static private float _selectedLocalPosX = 0;
    static private int _currentStoryNum = 1;

    [SerializeField] private StoryData[] _storyArray;
    [SerializeField] private GameObject _storyThumbnailPrefab;
    [SerializeField] private GameObject _lockUiPrefab;
    [SerializeField] private float _distance = 13.5f;
    [SerializeField] private float _scrollBaseSpeed = 13;
    [SerializeField] private float _scrollKeepSpeed = 26;
    [SerializeField] private GameObject _stageSelect = null;
    [SerializeField] private PlayerInput _playerInput = null;

    private InputAction _actionMoveRight = null;
    private InputAction _actionMoveLeft = null;
    private InputAction _actionStageSelect = null;
    private bool _scrolling = false;
    private float _scrollSpeed = 1;
    private float _lastReleased_IncStageNum = 0;
    private float _lastReleased_DecStageNum = 0;
    private IEnumerator coroutine = null;
    private RectTransform _rectTransform;

}
