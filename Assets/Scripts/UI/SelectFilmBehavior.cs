using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectFilmBehavior : MonoBehaviour
{
    const int MAX_STAGE = 5;
    const int MAX_WORLD = 4;

    static int _currentWorldNum = 1;

    void Start()
    {
        _actionMove = GetComponent<PlayerInput>().actions.FindAction("Move");

        if (_actionMove == null)
        {
            Debug.LogError("PlayerInputに　アクション：Move　がありません");
        }

        _actionDecision = GetComponent<PlayerInput>().actions.FindAction("StageSelectEnter");

        if (_actionDecision == null)
        {
            Debug.LogError("PlayerInputに　アクション：StageSelectEnter　がありません");
        }

        _actionStageSelectL = GetComponent<PlayerInput>().actions.FindAction("StageSelectL");

        if (_actionStageSelectL == null)
        {
            Debug.LogError("PlayerInputに　アクション：StageSelectL　がありません");
        }

        _actionStageSelectR = GetComponent<PlayerInput>().actions.FindAction("StageSelectR");

        if (_actionStageSelectR == null)
        {
            Debug.LogError("PlayerInputに　アクション：StageSelectR　がありません");
        }


        for (int i = 0; i < MAX_STAGE; ++i)
        {
            Instantiate(_pf_textNumber, transform).GetComponent<UnityEngine.UI.Text>().text = _currentWorldNum.ToString();
            Instantiate(_pf_textNumber, transform).GetComponent<UnityEngine.UI.Text>().text = (i + 1).ToString();
        }

        Init();
    }

    private void Init()
    {
        GetComponent<RectTransform>().localPosition = new Vector3(_scrollAmount * 2 + _numberDistance / 2, 0, 0);

        int childCount = transform.childCount;
        Transform childTransform = null;
        for (int i = 0; i < childCount; ++i)
        {
            childTransform = transform.GetChild(i);
            float offset = _scrollAmount * (i / 2);
            childTransform.position = new Vector3(_numberOffset.x + offset, _numberOffset.y, 0) + transform.position;
            childTransform.localScale = new Vector3(_numberScale, _numberScale, 0);

            childTransform = transform.GetChild(++i);
            childTransform.position = new Vector3(_numberOffset.x + offset + _numberDistance, _numberOffset.y, 0) + transform.position;
            childTransform.localScale = new Vector3(_numberScale, _numberScale, 0);
        }

    }

    void Update()
    {
        if (_stopInput)
        {
            return;
        }
        UpdateLoadStageInput();
        UpdateScroll();
        UpdateInputSelectWoeld();
    }

    private void UpdateLoadStageInput()
    {
        if (_actionDecision.triggered)
        {
            bool success = SuzumuraTomoki.SceneManager.instance.LoadStage(_stageNumber + (_currentWorldNum - 1) * MAX_STAGE);
            if (!success)
            {
                print("ステージセレクト「そんなステージはありません」");
            }
        }
    }


    private void UpdateScroll()
    {

        float x = _actionMove.ReadValue<Vector2>().x;
        if (Mathf.Abs(x) <= 0.1f)
        {
            return;
        }
        else if (x > 0)
        {
            if (_stageNumber >= MAX_STAGE)
            {
                return;
            }
            StartCoroutine(Scroll(false));
        }
        else
        {
            if (_stageNumber <= 1)
            {
                return;
            }
            StartCoroutine(Scroll(true));
        }
    }

    private void UpdateInputSelectWoeld()
    {
        if (_actionStageSelectL.ReadValue<bool>())
        {
            if (_currentWorldNum > 1)
            {
                --_currentWorldNum;
            }
        }
        if (_actionStageSelectR.ReadValue<bool>())
        {
            if (_currentWorldNum < MAX_WORLD)
            {
                ++_currentWorldNum;
            }
        }
    }

    private IEnumerator Scroll(bool right)
    {
        _stopInput = true;

        float speed = _scrollSpeed;
        float scrollAmount = _scrollAmount;
        if (!right)
        {
            speed *= -1;
            scrollAmount *= -1;
            ++_stageNumber;
        }
        else
        {
            --_stageNumber;
        }

        Vector3 oldPos = transform.localPosition;
        while (Mathf.Abs(oldPos.x - transform.localPosition.x) < _scrollAmount)
        {
            yield return null;
            transform.localPosition += new Vector3(speed, 0, 0);
        }

        transform.localPosition = oldPos + new Vector3(scrollAmount, 0, 0);
        _stopInput = false;
    }


    private void OnValidate()
    {
        Init();
    }

    [SerializeField, Header("Textオブジェクト。フォントが完成次第入れ替えます。")] private GameObject _pf_textNumber;
    //[SerializeField] private Sprite _numberFont;
    [SerializeField] private float _scrollAmount = 300;
    [SerializeField] private float _scrollSpeed = 1;
    [SerializeField] private float _numberScale = .15f;
    [SerializeField] private Vector2 _numberOffset = new Vector2(-700, 0);
    [SerializeField] private float _numberDistance = 100;
    private InputAction _actionMove = null;
    private InputAction _actionDecision = null;
    private InputAction _actionStageSelectL = null;
    private InputAction _actionStageSelectR = null;
    private bool _stopInput = false;
    private int _stageNumber = 1;
}
