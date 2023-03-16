using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;
using UnityEngine.UI;

public class TextStageNum : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        currentStageIndex = SceneManager.Instance.CurrentStageIndex;
        maxStages = SceneManager.Instance.StageSize;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
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
        if (Input.GetKeyDown(KeyCode.Return))//KeyCode.ReturnはEnter
        {
            SceneManager.Instance.LoadStage(currentStageIndex);
        }
    }

    private int currentStageIndex;
    private int maxStages;//private readOnly int MAX_STAGES;読み取り専用にしたかったが、コンストラクタでの初期化でエラーが起きるので普通の変数にしてstart()で設定する
    private Text text;
}
