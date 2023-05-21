using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SuzumuraTomoki;

public class TitleInput : MonoBehaviour
{

    private void Awake()
    {
        if (SceneManager.titleInitState == SceneManager.TitleInitState.STAGE_SELECT)
        {
            _stageSelectFilm.SetActive(true);
            return;
        }

        //else
        SceneManager.playerInput.FindAction("StageSelectEnter").Disable();
        _stageSelectFilm.SetActive(false);

        _titleEnter = SceneManager.playerInput.FindAction("TitleEnter");
        if (_titleEnter == null)
        {
            print("InputAction\"TitleEnter\"‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½");
        }

        _titleEnter.performed += ProcessInput;

        _titleEnter.Enable();
    }

    private void ProcessInput(InputAction.CallbackContext context)
    {
        _stageSelectFilm.SetActive(true);
        _titleEnter.performed -= ProcessInput;
        _titleEnter.Disable();
    }

    [SerializeField] private GameObject _stageSelectFilm = null;
    private InputAction _titleEnter = null;
}
