using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SuzumuraTomoki;

public class TitleInput : MonoBehaviour{

    private PlayerInput _playerInput;
    private InputAction _titleEnter;

    private void Start() {

        TryGetComponent(out _playerInput);

        _titleEnter = _playerInput.actions.FindAction("TitleEnter");
    }

    void Update()
    {
        if (_titleEnter.WasPressedThisFrame())
        {
            Fader.instance.fadeTime = 0.5f;
            SceneManager.LoadStageSelect();
        }
    }
}
