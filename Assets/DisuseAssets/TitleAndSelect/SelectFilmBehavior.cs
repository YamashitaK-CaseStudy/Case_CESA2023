using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct SeedScore
{
    public SeedIconData.TotalCountType total;
    public int obtained;
}

public class SelectFilmBehavior : MonoBehaviour
{
    /*<目次>
     *定数
     *静的公開メンバ
     *静的非公開メンバ
     *公開メンバ
     *非公開メンバ
     */

    /*定数*/
    public const int MAX_STAGE = 5;
    public const int MAX_WORLD = 3;

    //private const float OUTER_FRAME_RATIO = 36.0f / 3261;
    private const float FILM_FRAME_RATIO = 645.0f / 3261;

    private enum TextID
    {
        WORLD_NUM,
        HORIZONTAL_BAR,
        STAGE_NUM,
        SEED_ICON
    }

    /*静的公開メンバ*/

    static public int SeedScore
    {
        get
        {
            return _totalObtained;
        }
    }

    static public SeedScore SeedScoreCurrentStage
    {
        get
        {
            return _seedScoreArray[CurrentStageIndex];
        }
        set
        {
            _totalObtained += value.obtained - _seedScoreArray[CurrentStageIndex].obtained;
            _seedScoreArray[CurrentStageIndex] = value;
        }
    }

    /*静的非公開メンバ*/

    static private int CurrentStageIndex
    {
        get
        {
            return _stageNum - 1 + MAX_STAGE * (_worldNum - 1);
        }
    }

    static private int _worldNum = 1;
    static private int _stageNum = 1;
    static private int _totalObtained = 0;
    static private SeedScore[] _seedScoreArray = new SeedScore[MAX_STAGE * MAX_WORLD];


    /*公開メンバ*/

    public void EnableInput()
    {
        _actionDecision.Enable();
        _actionMove.Enable();
        _actionStageSelectL.Enable();
        _actionStageSelectR.Enable();
    }

    public void DisableInput()
    {
        _actionDecision.Disable();
        _actionMove.Disable();
        _actionStageSelectL.Disable();
        _actionStageSelectR.Disable();
    }


    /*非公開メンバ*/

    private void Awake()
    {

        InitInput();

        InitTransform();

        InitText();

        UpdateSeedScore();

        _stagePreviewImage.sprite = _stagePreviewData.GetSprite(_worldNum, _stageNum);

        if (Fader.state == Fader.State.FADING_IN)
        {
            Fader.stopInput = SuzumuraTomoki.SceneManager.playerInput;
        }
    }

    private void OnDestroy()
    {
        _actionStageSelectR.started -= IncreaseWorldNum;
        _actionStageSelectL.started -= DecreaseWorldNum;
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
            if (_stageNum >= MAX_STAGE)
            {
                return;
            }
            StartCoroutine(ScrollRight());
        }
        else
        {
            if (_stageNum <= 1)
            {
                return;
            }
            StartCoroutine(ScrollLeft());
        }
    }

    private void InitInput()
    {
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
            _actionDecision.performed += GoStage;
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
    }

    private void InitText()
    {
        UpdateWorldNum();

        for (int i = 0; i < MAX_STAGE; ++i)
        {
            int stageNum = i + 1;
            transform.GetChild(i).GetChild((int)TextID.STAGE_NUM).GetComponent<NumberFontImage>().numberImage = (NumberFontImage.NumberImage)stageNum;
        }
    }

    private void InitTransform()
    {
        RectTransform rectTrans = GetComponent<RectTransform>();

        _scrollAmount = rectTrans.rect.width * FILM_FRAME_RATIO * rectTrans.localScale.x;//OnValidate()で0割り対策済み

        rectTrans.localPosition = new Vector3(_scrollAmount * (MAX_STAGE - 1) / 2.0f, 0, 0);
        float currentStageOffset = _scrollAmount * (_stageNum - 1);
        rectTrans.localPosition -= new Vector3(currentStageOffset, 0, 0);

        Transform childTransform = null;
        for (int i = 0; i < MAX_STAGE; ++i)
        {
            float offset = _scrollAmount * i + _textUnitOffset.x;
            childTransform = transform.GetChild(i);
            childTransform.localPosition = (new Vector3(offset - currentStageOffset, _textUnitOffset.y, 0) - rectTrans.localPosition) / rectTrans.localScale.x;
            childTransform.localScale = new Vector3(_textUnitScale, _textUnitScale, 0);
        }
    }

    private IEnumerator ScrollRight()
    {
        _stopInput = true;
        ++_stageNum;

        Vector3 oldPos = transform.localPosition;

        while (Mathf.Abs(oldPos.x - transform.localPosition.x) < _scrollAmount)
        {
            yield return null;
            transform.localPosition -= new Vector3(_scrollSpeed * Time.deltaTime, 0, 0);
        }

        transform.localPosition = oldPos + new Vector3(-_scrollAmount, 0, 0);
        _stopInput = false;

        _stagePreviewImage.sprite = _stagePreviewData.GetSprite(_worldNum, _stageNum);
    }

    private IEnumerator ScrollLeft()
    {
        _stopInput = true;
        --_stageNum;

        Vector3 oldPos = transform.localPosition;

        while (Mathf.Abs(oldPos.x - transform.localPosition.x) < _scrollAmount)
        {
            yield return null;
            transform.localPosition += new Vector3(_scrollSpeed * Time.deltaTime, 0, 0);
        }

        transform.localPosition = oldPos + new Vector3(_scrollAmount, 0, 0);
        _stopInput = false;

        _stagePreviewImage.sprite = _stagePreviewData.GetSprite(_worldNum, _stageNum);
    }

    private void UpdateWorldNum()
    {
        for (int i = 0; i < MAX_STAGE; ++i)
        {
            transform.GetChild(i).GetChild((int)TextID.WORLD_NUM).GetComponent<NumberFontImage>().numberImage = (NumberFontImage.NumberImage)_worldNum;
        }
        _stagePreviewImage.sprite = _stagePreviewData.GetSprite(_worldNum, _stageNum);
    }

    private void GoStage(InputAction.CallbackContext context)
    {

        if (_stopInput)
        {
            return;
        }

        bool success = SuzumuraTomoki.SceneManager.LoadStage(_stageNum + (_worldNum - 1) * MAX_STAGE);

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
        if (_worldNum > 1)
        {
            --_worldNum;
            UpdateWorldNum();
            UpdateSeedScore();
        }
    }

    private void IncreaseWorldNum(InputAction.CallbackContext context)
    {
        if (_worldNum < MAX_WORLD)
        {
            ++_worldNum;
            UpdateWorldNum();
            UpdateSeedScore();
        }
    }

    private void UpdateSeedScore()
    {
        int scoreIndex;
        for (int i = 0; i < MAX_STAGE; ++i)
        {
            scoreIndex = i + (_worldNum - 1) * MAX_STAGE;
            if (_seedScoreArray[scoreIndex].total == 0)
            {
                _seedScoreArray[scoreIndex].total = _totalSeedData.GetDefaultTotalCount(_worldNum, i + 1);
            }
            transform.GetChild(i).GetChild((int)TextID.SEED_ICON).GetComponent<SeedScoreIcon>().ChangeIcon(_seedScoreArray[scoreIndex]);
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
    [SerializeField] private TotalSeedData _totalSeedData;
    [SerializeField] private StagePreviewData _stagePreviewData;
    [SerializeField] private UnityEngine.UI.Image _stagePreviewImage;
    private InputAction _actionMove = null;
    private InputAction _actionDecision = null;
    private InputAction _actionStageSelectL = null;
    private InputAction _actionStageSelectR = null;
    private bool _stopInput = false;
}
