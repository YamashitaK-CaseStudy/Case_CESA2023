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
            print("�G���[�F�e�L�X�g�i�X�e�[�W�ԍ��j�̃R���|�[�l���g�@TextStageNum�@�Ƀt�F�[�h�p�l�����ݒ肳��Ă��܂���");
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
        if (Input.GetKeyDown(KeyCode.Return))//KeyCode.Return �� Enter
        {
            fadePanel.GetComponent<Fader>().FadeOut(this);
        }
    }

    public override void ProcessAfterFadeOut()
    {
        SceneManager.Instance.LoadStage(currentStageIndex);
    }

    private int currentStageIndex;
    private int maxStages;//private readOnly int MAX_STAGES;�ǂݎ���p�ɂ������������A�R���X�g���N�^�ł̏������ŃG���[���N����̂ŕ��ʂ̕ϐ��ɂ���start()�Őݒ肷��
    private Text text;
    [SerializeField] private GameObject fadePanel = null;
}
