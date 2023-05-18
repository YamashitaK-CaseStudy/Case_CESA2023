using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectFilmBehavior : MonoBehaviour
{
    /*定数*/
    public const int MAX_STAGE = 5;

    private const int MAX_WORLD = 4;
    //private const float OUTER_FRAME_RATIO = 36.0f / 3261;
    private const float FILM_FRAME_RATIO = 645.0f / 3261;

    private enum TextID
    {
        WORLD_NUM,
        HORIZONTAL_BAR,
        STAGE_NUM,
        SEED_ICON,
        SEED_SCORE
    }

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

    static private int currentStageIndex
    {
        get
        {
            return _stageNumber - 1 + MAX_STAGE * (_currentWorldNum - 1);
        }
    }

    static private int _currentWorldNum = 1;
    static private int _stageNumber = 1;
    static private List<int> _obtainedCountList = new List<int>(new int[MAX_STAGE * MAX_WORLD]);

    void Start()
    {
        /*テキスト生成・初期化*/

        _actionMove = SuzumuraTomoki.SceneManager.playerInput.FindAction("Move");

        if (_actionMove == null)
        {
            Debug.LogError("PlayerInputに　アクション：Move　がありません");
        }

        _actionDecision = SuzumuraTomoki.SceneManager.playerInput.FindAction("StageSelectEnter");

        if (_actionDecision == null)
        {
            Debug.LogError("PlayerInputに　アクション：StageSelectEnter　がありません");
        }
        else
        {
            _actionDecision.started += GoStage;
            _actionDecision.Enable();
        }

        _actionStageSelectL = SuzumuraTomoki.SceneManager.playerInput.FindAction("StageSelectL");

        if (_actionStageSelectL == null)
        {
            Debug.LogError("PlayerInputに　アクション：StageSelectL　がありません");
        }
        else
        {
            _actionStageSelectL.started += DecreaseWorldNum;
        }

        _actionStageSelectR = SuzumuraTomoki.SceneManager.playerInput.FindAction("StageSelectR");

        if (_actionStageSelectR == null)
        {
            Debug.LogError("PlayerInputに　アクション：StageSelectR　がありません");
        }
        else
        {
            _actionStageSelectR.started += IncreaseWorldNum;
        }

        Init();
    }

    private void OnDestroy()
    {
        _actionStageSelectR.started -= IncreaseWorldNum;
        _actionStageSelectL.started -= DecreaseWorldNum;
    }

    private void Init()
    {
        _stageNumber = 1;

        InitTransform();

        InitText();
    }

    private void Update()
    {
        if (_stopInput)
        {
            return;
        }

        float x = _actionMove.ReadValue<Vector2>().x;

        //デッドゾーン
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
            StartCoroutine(ScrollRight());
        }
        else
        {
            if (_stageNumber <= 1)
            {
                return;
            }
            StartCoroutine(ScrollLeft());
        }
    }

    private void InitText()
    {
        UpdateWorldNum();

        Transform textUnitTransform = null;
        for (int i = 0; i < MAX_STAGE; ++i)
        {
            textUnitTransform = transform.GetChild(i);
            int stageNum = i + 1;
            textUnitTransform.GetChild((int)TextID.STAGE_NUM).GetComponent<UnityEngine.UI.Text>().text = stageNum.ToString();

            int totalSeeds = UiSeedBehavior.totalSeeds;
            if (totalSeeds == 0)
            {
                totalSeeds = _seedData.GetDefaultTotalCount(_currentWorldNum, stageNum);
            }
            textUnitTransform.GetChild((int)TextID.SEED_SCORE).GetComponent<UnityEngine.UI.Text>().text = _obtainedCountList[i + (_currentWorldNum - 1) * MAX_STAGE].ToString() + "/" + totalSeeds.ToString();
        }
    }

    private void InitTransform()
    {
        RectTransform rectTrans = GetComponent<RectTransform>();

        _scrollAmount = rectTrans.rect.width * FILM_FRAME_RATIO * rectTrans.localScale.x;//OnValidate()で0割り対策済み

        rectTrans.localPosition = new Vector3(_scrollAmount * (MAX_STAGE - 1) / 2.0f, 0, 0);

        Transform childTransform = null;
        for (int i = 0; i < MAX_STAGE; ++i)
        {
            float offset = _scrollAmount * i + _textUnitOffset.x;
            childTransform = transform.GetChild(i);
            childTransform.localPosition = (new Vector3(offset, _textUnitOffset.y, 0) - rectTrans.localPosition) / rectTrans.localScale.x;
            childTransform.localScale = new Vector3(_textUnitScale, _textUnitScale, 0);
        }
    }

    private IEnumerator ScrollRight()
    {
        _stopInput = true;
        ++_stageNumber;

        Vector3 oldPos = transform.localPosition;

        while (Mathf.Abs(oldPos.x - transform.localPosition.x) < _scrollAmount)
        {
            yield return null;
            transform.localPosition -= new Vector3(_scrollSpeed * Time.deltaTime, 0, 0);
        }

        transform.localPosition = oldPos + new Vector3(-_scrollAmount, 0, 0);
        _stopInput = false;
    }

    private IEnumerator ScrollLeft()
    {
        _stopInput = true;
        --_stageNumber;

        Vector3 oldPos = transform.localPosition;

        while (Mathf.Abs(oldPos.x - transform.localPosition.x) < _scrollAmount)
        {
            yield return null;
            transform.localPosition += new Vector3(_scrollSpeed * Time.deltaTime, 0, 0);
        }

        transform.localPosition = oldPos + new Vector3(_scrollAmount, 0, 0);
        _stopInput = false;
    }

    private void UpdateWorldNum()
    {
        for (int i = 0; i < MAX_STAGE; ++i)
        {
            transform.GetChild(i).GetChild((int)TextID.WORLD_NUM).GetComponent<UnityEngine.UI.Text>().text = _currentWorldNum.ToString();
        }
    }

    private void GoStage(InputAction.CallbackContext context)
    {
        
        if (_stopInput)
        {
            return;
        }

        bool success = SuzumuraTomoki.SceneManager.LoadStage(_stageNumber + (_currentWorldNum - 1) * MAX_STAGE);

        if (!success)
        {
            //TODO:無効な入力を伝えるSE
            print("ステージセレクト「ステージが存在しません」");
            return;
        }

        _actionDecision.performed -= GoStage;
        _actionDecision.Disable();
    }

    private void DecreaseWorldNum(InputAction.CallbackContext context)
    {
        if (_currentWorldNum > 1)
        {
            --_currentWorldNum;
            UpdateWorldNum();
        }
    }

    private void IncreaseWorldNum(InputAction.CallbackContext context)
    {
        if (_currentWorldNum < MAX_WORLD)
        {
            ++_currentWorldNum;
            UpdateWorldNum();
        }
    }

    private void OnValidate()
    {
        _update = false;
        if (transform.localScale.x == 0)
        {
            transform.localScale = transform.localScale + new Vector3(float.Epsilon, 0, 0);
        }
        InitTransform();
    }

    //[Header("ステージ番号設定")]
    //[SerializeField] private Sprite _numberFont;
    private float _scrollAmount = 0;
    [Header("RectTransformを変更した場合updateを押すとシーンが更新されます")]
    [SerializeField] private bool _update = false;//RectTransformをインスペクタで変更するとOnValidate()が呼ばれないため
    [SerializeField] private float _scrollSpeed = 3;
    [SerializeField] private Vector2 _textUnitOffset = new Vector2(0, 0);
    [SerializeField] private float _textUnitScale = 1;
    //[Header("種スコア設定")]
    //[SerializeField] private Vector2 _seedIconOffset = new Vector2(-650, -30);
    //[SerializeField] private float _seedIconScale = .05f;
    //[SerializeField] private Vector2 _seedScoreOffset = new Vector2(-550, -30);
    //[SerializeField] private float _seedScoreScale = .05f;
    [SerializeField] private SeedData _seedData;
    private InputAction _actionMove = null;
    private InputAction _actionDecision = null;
    private InputAction _actionStageSelectL = null;
    private InputAction _actionStageSelectR = null;
    private bool _stopInput = false;
}
