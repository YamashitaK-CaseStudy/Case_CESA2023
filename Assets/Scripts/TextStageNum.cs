using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;

public class TextStageNum : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.Instance.CurrentScene;//TODO SceneManager�ɃA�N�Z�TCurrentScene�ƕϐ���ǉ�
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentScene;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentScene;
        }
    }

    private int currentScene;
    private const int MAX_STAGES = SceneManager.Instance.StageSize;//TODO SceneManager�ɃA�N�Z�TStageSize��ǉ�;
}
