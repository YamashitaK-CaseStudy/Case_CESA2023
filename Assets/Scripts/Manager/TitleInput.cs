using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;

public class TitleInput : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sceneManager.LoadStageSelect();
        }
    }

    [SerializeField] private SceneManager sceneManager;
}
