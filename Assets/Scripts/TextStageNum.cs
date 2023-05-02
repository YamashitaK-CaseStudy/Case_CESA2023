using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;
using UnityEngine.UI;

public class TextStageNum : FadeOutCompletionReceiver
{
    void Start()
    {
        currentStageIndex = SceneManager.Instance.CurrentStageIndex;
        maxStages = SceneManager.Instance.StageSize;
        text = GetComponent<Text>();
        if (fadePanel == null)
        {
            print("エラー：テキスト（ステージ番号）のコンポーネント　TextStageNum　にフェードパネルが設定されていません");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentStageIndex;
            if (currentStageIndex > maxStages)
            {
                currentStageIndex = maxStages;
            }
            text.text = currentStageIndex.ToString();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentStageIndex;
            if (currentStageIndex < 1)
            {
                currentStageIndex = 1;
            }
            text.text = currentStageIndex.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Return))//KeyCode.Return は Enter
        {
            fadePanel.GetComponent<Fader>().FadeOut(this);
        }
    }

    public override void ProcessAfterFadeOut()
    {
        SceneManager.Instance.LoadStage(currentStageIndex);
    }

    private int currentStageIndex;
    private int maxStages;//private readOnly int MAX_STAGES;読み取り専用にしたかったが、コンストラクタでの初期化でエラーが起きるので普通の変数にしてstart()で設定する
    private Text text;
    [SerializeField] private GameObject fadePanel = null;
}
