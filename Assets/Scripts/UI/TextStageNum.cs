using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TextStageNum : MonoBehaviour
{
    private PlayerInput _playerInput = null;
    //private InputAction _inputAction = null;

    private InputAction _selectL;
    private InputAction _selectR;
    private InputAction _selectEntor;

    void Start()
    {
        maxStages = SceneManager.instance.stageCount;
        text = GetComponent<Text>();
        
        _playerInput = GetComponent<PlayerInput>();
        //_inputAction = _playerInput.actions.FindAction("Move");

        // �e��{�^���̎擾
        _selectL = _playerInput.actions.FindAction("StageSelectL");
        _selectR = _playerInput.actions.FindAction("StageSelectR");
        _selectEntor = _playerInput.actions.FindAction("StageSelectEnter");  
    }

    void Update()
    {
        if (_selectR.WasPressedThisFrame())
        {
            ++sCurrentStageIndex;
            if (sCurrentStageIndex > maxStages)
            {
                sCurrentStageIndex = maxStages;
            }
            text.text = sCurrentStageIndex.ToString();
        }
        if (_selectL.WasPressedThisFrame())
        {
            --sCurrentStageIndex;
            if (sCurrentStageIndex < 1)
            {
                sCurrentStageIndex = 1;
            }
            text.text = sCurrentStageIndex.ToString();
        }
        if (_selectEntor.WasPressedThisFrame())//KeyCode.Return �� Enter
        {
            SceneManager.instance.LoadStage(sCurrentStageIndex);
        }
    }

    private static int sCurrentStageIndex = 1;
    private int maxStages;//private readOnly int MAX_STAGES;�ǂݎ���p�ɂ������������A�R���X�g���N�^�ł̏������ŃG���[���N����̂ŕ��ʂ̕ϐ�
    private Text text;
}
