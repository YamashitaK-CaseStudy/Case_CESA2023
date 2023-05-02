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

        // 各種ボタンの取得
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
        if (_selectEntor.WasPressedThisFrame())//KeyCode.Return は Enter
        {
            SceneManager.instance.LoadStage(sCurrentStageIndex);
        }
    }

    private static int sCurrentStageIndex = 1;
    private int maxStages;//private readOnly int MAX_STAGES;読み取り専用にしたかったが、コンストラクタでの初期化でエラーが起きるので普通の変数
    private Text text;
}
