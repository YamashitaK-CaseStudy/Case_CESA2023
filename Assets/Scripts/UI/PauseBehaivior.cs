using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehaivior : MonoBehaviour
{
    public void BackToGame()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        gameObject.SetActive(false);
        SuzumuraTomoki.SceneManager.LoadCurrentScene();
    }

    public void GoToSelect()
    {
        gameObject.SetActive(false);
        SuzumuraTomoki.SceneManager.LoadStageSelect();
    }

    void Start()
    {
        _inputs.FindActionMap("Pause").FindAction("Pause").performed += CallBackPauseButton;
        _inputs.FindActionMap("Pause").FindAction("Cancel").performed += CallBackCancelButton;
    }

    private void OnDestroy()
    {
        _inputs.FindActionMap("Pause").FindAction("Pause").performed -= CallBackPauseButton;
        _inputs.FindActionMap("Pause").FindAction("Cancel").performed -= CallBackCancelButton;
    }

    private void CallBackPauseButton(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        switch (gameObject.activeSelf)
        {
            case true:
                ExitPause();
                break;
            case false:
                SuzumuraTomoki.SceneManager.playerInput.Disable();
                gameObject.SetActive(true);
                _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
                break;
        }
    }

    private void CallBackCancelButton(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        ExitPause();
    }

    private void ExitPause()
    {
        gameObject.SetActive(false);
        SuzumuraTomoki.SceneManager.playerInput.Enable();
    }

    [SerializeField] private UnityEngine.EventSystems.EventSystem _eventSystem;
    [SerializeField] private UnityEngine.InputSystem.InputActionAsset _inputs;
}
