using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TextStageNum : MonoBehaviour
{
    private PlayerInput _playerInput = null;
    private InputAction _inputAction = null;

    void Start()
    {
        maxStages = SceneManager.instance.stageCount;
        text = GetComponent<Text>();
        
        _playerInput = GetComponent<PlayerInput>();
        _inputAction = _playerInput.actions.FindAction("Move");
    }

    void Update()
    {
        if (_inputAction.ReadValue<Vector2>().x == 1.0f)
        {
            ++sCurrentStageIndex;
            if (sCurrentStageIndex > maxStages)
            {
                sCurrentStageIndex = maxStages;
            }
            text.text = sCurrentStageIndex.ToString();
        }
        if (_inputAction.ReadValue<Vector2>().x == -1.0f)
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
            SceneManager.instance.LoadStage(sCurrentStageIndex);
        }
    }

    private static int sCurrentStageIndex = 1;
    private int maxStages;//private readOnly int MAX_STAGES;読み取り専用にしたかったが、コンストラクタでの初期化でエラーが起きるので普通の変数
    private Text text;
}
