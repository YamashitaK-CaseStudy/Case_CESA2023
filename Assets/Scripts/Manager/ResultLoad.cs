using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;

public class ResultLoad : MonoBehaviour
{
    static public void LoadStageSelectScene() {
        SceneManager.instance.LoadStageSelect();
    }
    static public void LoadBeforeScene() {
        SceneManager.instance.LoadBeforeScene();
    }
}
