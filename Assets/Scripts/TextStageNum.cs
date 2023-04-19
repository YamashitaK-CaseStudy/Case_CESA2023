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
            print("�G���[�F�e�L�X�g�i�X�e�[�W�ԍ��j�̃R���|�[�l���g�@TextStageNum�@�Ƀt�F�[�h�p�l�����ݒ肳��Ă��܂���");
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
        if (Input.GetKeyDown(KeyCode.Return))//KeyCode.Return �� Enter
        {
            fadePanel.GetComponent<Fader>().FadeOut(this);
        }
    }

    public override void ProcessAfterFadeOut()
    {
        SceneManager.Instance.LoadStage(sCurrentStageIndex);
    }

    private static int sCurrentStageIndex = 1;
    private int maxStages;//private readOnly int MAX_STAGES;�ǂݎ���p�ɂ������������A�R���X�g���N�^�ł̏������ŃG���[���N����̂ŕ��ʂ̕ϐ��ɂ���start()�Őݒ肷��
    private Text text;
    [SerializeField] private GameObject fadePanel = null;
}
