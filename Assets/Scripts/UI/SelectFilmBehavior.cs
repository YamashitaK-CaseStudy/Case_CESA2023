using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectFilmBehavior : MonoBehaviour
{
    const int _MAX_STAGE = 7;
    const int MAX_WORLD = 4;

    static public int obtainedCount
    {
        get
        {
            return _obtainedCountList[currentStageIndex];
        }
        set
        {
            _obtainedCountList[currentStageIndex] = value;
        }
    }

    static public int MAX_STAGE
    {
        get
        {
            return _MAX_STAGE;
        }
    }

    static private int currentStageIndex
    {
        get
        {
            return _stageNumber - 1 + _MAX_STAGE * (_currentWorldNum - 1);
        }
    }

    static private int _currentWorldNum = 1;
    static private int _stageNumber = 1;
    static private List<int> _obtainedCountList = new List<int>(new int[_MAX_STAGE * MAX_WORLD]);

    void Start()
    {
        /*テキスト生成・初期化*/

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


        for (int i = 1; i <= _MAX_STAGE; ++i)
        {
            //注意　１ループで生成する数（１ステージごとの数）をInit()のfloat offset = _scrollAmount * (i / １ステージごとの数);に反映してください 検索ワード：マジックナンバー 単語単位の検索を解除しないと出ない(vs2022)

            /*ステージ番号*/
            Instantiate(_pfTextNumber, transform).GetComponent<UnityEngine.UI.Text>().text = _currentWorldNum.ToString();
            Instantiate(_pfTextNumber, transform).GetComponent<UnityEngine.UI.Text>().text = "-";
            Instantiate(_pfTextNumber, transform).GetComponent<UnityEngine.UI.Text>().text = (i).ToString();

            /*種スコア*/
            Instantiate(_pfTextNumber, transform).GetComponent<UnityEngine.UI.Text>().text = "たね";
            int totalSeeds = UiSeedBehavior.totalSeeds;
            if (totalSeeds == 0)
            {
                totalSeeds = _seedData.GetDefaultTotalCount(_currentWorldNum, i);
            }
            Instantiate(_pfTextNumber, transform).GetComponent<UnityEngine.UI.Text>().text = _obtainedCountList[i - 1].ToString() + "/" + totalSeeds.ToString();
        }

        Init();
    }

    private void Init()
    {
        _stageNumber = 1;

        GetComponent<RectTransform>().localPosition = new Vector3(_scrollAmount * 2, 0, 0);

        int childCount = transform.childCount;
        Transform childTransform = null;
        for (int i = 0; i < childCount; ++i)
        {
            childTransform = transform.GetChild(i);
            float offset = _scrollAmount * (i / 5);//マジックナンバー

            /*ステージ番号*/
            childTransform.position = new Vector3(_numberOffset.x + offset, _numberOffset.y, 0) + transform.position;
            childTransform.localScale = new Vector3(_numberScale, _numberScale, 0);

            childTransform = transform.GetChild(++i);
            childTransform.position = new Vector3(_numberOffset.x + offset + _numberDistance / 2, _numberOffset.y + _numberScale * 30/*ハイフンのフォントが低い位置になるため微調整*/, 0) + transform.position;
            childTransform.localScale = new Vector3(_numberScale, _numberScale, 0);

            childTransform = transform.GetChild(++i);
            childTransform.position = new Vector3(_numberOffset.x + offset + _numberDistance, _numberOffset.y, 0) + transform.position;
            childTransform.localScale = new Vector3(_numberScale, _numberScale, 0);

            /*たねスコア*/
            childTransform = transform.GetChild(++i);
            childTransform.position = new Vector3(_seedIconOffset.x + offset, _seedIconOffset.y, 0) + transform.position;
            childTransform.localScale = new Vector3(_seedIconScale, _seedIconScale, 0);

            childTransform = transform.GetChild(++i);
            childTransform.position = new Vector3(_seedScoreOffset.x + offset, _seedScoreOffset.y, 0) + transform.position;
            childTransform.localScale = new Vector3(_seedScoreScale, _seedScoreScale, 0);
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
            bool success = SuzumuraTomoki.SceneManager.instance.LoadStage(_stageNumber + (_currentWorldNum - 1) * _MAX_STAGE);
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
            if (_stageNumber >= _MAX_STAGE)
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

    [SerializeField, Header("Textオブジェクト。フォントが完成次第入れ替えます。")] private GameObject _pfTextNumber;
    [Header("ステージ番号設定")]
    //[SerializeField] private Sprite _numberFont;
    [SerializeField] private float _scrollAmount = 300;
    [SerializeField] private float _scrollSpeed = 5;
    [SerializeField] private float _numberScale = .15f;
    [SerializeField] private Vector2 _numberOffset = new Vector2(-700, 0);
    [SerializeField] private float _numberDistance = 100;
    [Header("種スコア設定")]
    [SerializeField] private Vector2 _seedIconOffset = new Vector2(-650, -30);
    [SerializeField] private float _seedIconScale = .05f;
    [SerializeField] private Vector2 _seedScoreOffset = new Vector2(-550, -30);
    [SerializeField] private float _seedScoreScale = .05f;
    [SerializeField] private SeedData _seedData;
    private InputAction _actionMove = null;
    private InputAction _actionDecision = null;
    private InputAction _actionStageSelectL = null;
    private InputAction _actionStageSelectR = null;
    private bool _stopInput = false;
}
