using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;
using UnityEngine.UI;

public class TextStageNum : FadeOutCompletionReceiver
{
    void Start()
    {
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
            ++sCurrentStageIndex;
            if (sCurrentStageIndex > maxStages)
            {
                sCurrentStageIndex = maxStages;
            }
            text.text = sCurrentStageIndex.ToString();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --sCurrentStageIndex;
            if (sCurrentStageIndex < 1)
            {
                sCurrentStageIndex = 1;
            }
            text.text = sCurrentStageIndex.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Return))//KeyCode.Return は Enter
        {
            fadePanel.GetComponent<Fader>().FadeOut(this);
        }
    }

    public override void ProcessAfterFadeOut()
    {
        SceneManager.Instance.LoadStage(sCurrentStageIndex);
    }

    private static int sCurrentStageIndex = 1;
    private int maxStages;//private readOnly int MAX_STAGES;読み取り専用にしたかったが、コンストラクタでの初期化でエラーが起きるので普通の変数にしてstart()で設定する
    private Text text;
    [SerializeField] private GameObject fadePanel = null;
}
