using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[System.Serializable]
public struct StoryPage
{
    public Sprite _image;
    public string _storyText;
    [Header("複数設定しても１つしか流しません")] public GameSESoundData.GameSE[] _soundArray;
}

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
    public struct StoryData
    {
        public int _unlockNum;
        public StoryPage[] _storyArray;//story page array インスペクタがリセットされるためリネームを保留

    }

    /*Unity関数*/

    private void Awake()
    {
        _pageIdArray = new
        if (_lockArray == null)
        {
            _lockArray
        }
        _rectTransform = GetComponent<RectTransform>();
        _selectedLocalPosX = -((_currentStoryId) * _distance);
        _rectTransform.localPosition = new Vector3(_selectedLocalPosX, 0, 0);

        //生成
        for (int i = 0, length = _storyData._storyArray.Length; i < length; ++i)
        {
            var piceInstance = Instantiate(_storySelectPiecePrefab, transform);
            piceInstance.transform.localPosition = new Vector3(_distance * i, 0, 0);
            ref var storyData = ref _storyData._storyArray[i];
            piceInstance.GetComponent<StorySelectPieceBehv>().Story = storyData._storyArray[0];

            if (SelectFilmBehavior.SeedScore < storyData._unlockNum)
            {
                Instantiate(_lockUiPrefab, piceInstance.transform);
                storyData._lock = true;
            }
        }

        _actionMoveRight = _playerInput.currentActionMap.FindAction("MoveRight");
        _actionMoveRight.started += CallBack_Started_MoveRight;
        _actionMoveRight.canceled += CallBack_Canceled_MoveRight;

        _actionMoveLeft = _playerInput.currentActionMap.FindAction("MoveLeft");
        _actionMoveLeft.started += CallBack_Started_MoveLeft;
        _actionMoveLeft.canceled += CallBack_Canceled_MoveLeft;

        _actionStageSelect = _playerInput.currentActionMap.FindAction("StageSelect");
        _actionStageSelect.started += CallBack_Started_SwitchStageSelect;


        _actionTextDown = _playerInput.currentActionMap.FindAction("TextDown");
        _actionTextDown.started += CallBack_Started_TextDown;


        _actionTextUp = _playerInput.currentActionMap.FindAction("TextUp");
        _actionTextUp.started += CallBack_Started_TextUp;
    }

    private void OnDestroy()
    {
        _actionMoveRight.started -= CallBack_Started_MoveRight;
        _actionMoveRight.canceled -= CallBack_Canceled_MoveRight;
        _actionMoveLeft.started -= CallBack_Started_MoveLeft;
        _actionMoveLeft.canceled -= CallBack_Canceled_MoveLeft;
        _actionStageSelect.started -= CallBack_Started_SwitchStageSelect;
        _actionTextDown.started -= CallBack_Started_TextDown;
        _actionTextUp.started -= CallBack_Started_TextUp;
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
        if (_currentStoryId <= 0)
        {
            return;
        }

        --_currentStoryId;
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
        if (_currentStoryId + 1 >= _storyData._storyArray.Length)
        {
            return;
        }

        ++_currentStoryId;
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

        if (_lastReleased_MoveLeft < startTime)
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

        if (_lastReleased_MoveRight < startTime)
        {
            _scrollSpeed = _scrollKeepSpeed;
            BeginScrollLeft();
        }

    }

    private void CallBack_Started_SwitchStageSelect(InputAction.CallbackContext context)
    {
        if (_scrolling)
        {
            return;
        }

        //ストーリーアーカイブを無効にする
        gameObject.SetActive(false);
        //ステージセレクトを有効にする
        _stageSelect.SetActive(true);

        _playerInput.SwitchCurrentActionMap("Player");
    }

    private void CallBack_Canceled_MoveLeft(InputAction.CallbackContext context)
    {
        _lastReleased_MoveLeft = Time.time;//数百時間動かしていると有効桁数が足りなくなってバグる
    }

    private void CallBack_Canceled_MoveRight(InputAction.CallbackContext context)
    {
        _lastReleased_MoveRight = Time.time;//数百時間動かしていると有効桁数が足りなくなってバグる
    }

    private void CallBack_Started_TextDown(InputAction.CallbackContext context)
    {
        if (_scrolling)
        {
            return;
        }

        ref var storyData = ref _storyData._storyArray[_currentStoryId];

        if (storyData._lock)
        {
            return;
        }

        ref var pageId = ref storyData._pageId;
        ref var storyPageArray = ref storyData._storyArray;

        if (pageId + 1 >= storyPageArray.Length)
        {
            return;
        }

        ref var storyPage = ref storyPageArray[++pageId];

        transform.GetChild(_currentStoryId).GetComponent<StorySelectPieceBehv>().Story = storyPage;

        if (storyPage._soundArray.Length > 0)
        {
            GameSoundManager.Instance.PlayGameSE(storyPage._soundArray[0]);
        }
    }

    private void CallBack_Started_TextUp(InputAction.CallbackContext context)
    {
        if (_scrolling)
        {
            return;
        }

        ref var storyData = ref _storyData._storyArray[_currentStoryId];

        if (storyData._lock)
        {
            return;
        }

        ref var pageId = ref storyData._pageId;

        if (pageId <= 0)
        {
            return;
        }

        ref var storyPage = ref storyData._storyArray[--pageId];
        transform.GetChild(_currentStoryId).GetComponent<StorySelectPieceBehv>().Story = storyPage;
        if (storyPage._soundArray.Length > 0)
        {
            GameSoundManager.Instance.PlayGameSE(storyPage._soundArray[0]);
        }
    }

    /*非公開変数*/

    static private float _selectedLocalPosX = 0;
    static private int _currentStoryId = 0;
    static private bool[] _lockArray = null;

    [SerializeField] private Story.StoryData _storyData;
    [SerializeField] private GameObject _storySelectPiecePrefab;
    [SerializeField] private GameObject _lockUiPrefab;
    [SerializeField] private float _distance = 13.5f;
    [SerializeField] private float _scrollBaseSpeed = 13;
    [SerializeField] private float _scrollKeepSpeed = 26;
    [SerializeField] private GameObject _stageSelect = null;
    [SerializeField] private PlayerInput _playerInput = null;
    private int[] _pageIdArray;

    private InputAction _actionMoveRight = null;
    private InputAction _actionMoveLeft = null;
    private InputAction _actionStageSelect = null;
    private InputAction _actionTextDown = null;
    private InputAction _actionTextUp = null;
    private bool _scrolling = false;
    private float _scrollSpeed = 1;
    private float _lastReleased_MoveRight = 0;
    private float _lastReleased_MoveLeft = 0;
    private IEnumerator coroutine = null;
    private RectTransform _rectTransform;

}
