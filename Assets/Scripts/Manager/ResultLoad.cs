using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;

public class ResultLoad : MonoBehaviour
{
    public void LoadStageSelectScene() {
        Fader.stopInput = _inputs.FindActionMap("UI");
        SceneManager.LoadStageSelect();
    }
    public void LoadBeforeScene() {
        Fader.stopInput = _inputs.FindActionMap("UI");
        SceneManager.LoadBeforeScene();
    }


    [SerializeField] UnityEngine.InputSystem.InputActionAsset _inputs = null;

}
