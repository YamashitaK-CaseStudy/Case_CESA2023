using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SuzumuraTomoki;

public class TitleInput : MonoBehaviour
{

    [SerializeField] private GameObject _canvusStageSelect = null;

    private void Start()
    {
        if (SceneManager.titleInitState == SceneManager.TitleInitState.STAGE_SELECT)
        {
            _canvusStageSelect.SetActive(true);
            return;
        }

        //else

        _canvusStageSelect.SetActive(false);

        var titleEnter = SceneManager.playerInput.FindAction("TitleEnter");
        if (titleEnter == null)
        {
            print("InputAction\"TitleEnter\"‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½");
        }

        titleEnter.performed += func =>
        {
            _canvusStageSelect.SetActive(true);
            titleEnter.Disable();
        };

        titleEnter.Enable();
    }

}
