using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehaivior : MonoBehaviour
{
    /*  静的 公開関数  */
    static public bool enable
    {
        get
        {
            return _enable;
        }
    }

    /*  静的 変数  */

    static private bool _enable = false;

    /*  公開関数  */

    public void ExitPause()
    {
        _enable = false;
        Time.timeScale = 1;
        _uiUnit.SetActive(false);
        _playerInput.currentActionMap = SuzumuraTomoki.SceneManager.playerInput;
    }
    public void BackToGame()
    {
        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Decision5);

        ExitPause();
    }

    public void Restart()
    {
        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Decision5);

        ExitPause();
        SuzumuraTomoki.SceneManager.LoadCurrentScene();
    }

    public void GoToSelect()
    {
        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Decision5);

        ExitPause();
        SuzumuraTomoki.SceneManager.LoadStageSelect();
    }
    /*  公開変数  */

    public UnityEngine.EventSystems.EventSystem _eventSystem;
    public UnityEngine.InputSystem.InputActionAsset _inputs;
    public UnityEngine.InputSystem.PlayerInput _playerInput;

    /*  非公開  */

    void Start()
    {
        _uiUnit.SetActive(false);

        SuzumuraTomoki.SceneManager.playerInput.FindAction("Pause").performed += CallBackPauseButton;
        _inputs.FindActionMap("Pause").FindAction("Pause").performed += CallBackPauseButton;
        _inputs.FindActionMap("Pause").FindAction("Cancel").performed += CallBackCancelButton;
    }

    private void OnDestroy()
    {
        _inputs.FindActionMap("Player").FindAction("Pause").performed -= CallBackPauseButton;
        _inputs.FindActionMap("Pause").FindAction("Pause").performed -= CallBackPauseButton;
        _inputs.FindActionMap("Pause").FindAction("Cancel").performed -= CallBackCancelButton;
    }

    private void CallBackPauseButton(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        switch (_uiUnit.activeSelf)
        {
            case true:
                SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Cancel);

                ExitPause();
                break;
            case false:
                SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Cancel);

                _enable = true;
                Time.timeScale = 0;
                _playerInput.currentActionMap = _inputs.FindActionMap("Pause");
                _uiUnit.SetActive(true);
                _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
                break;
        }
    }

    private void CallBackCancelButton(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.Cancel);

        ExitPause();
    }

    [SerializeField] private GameObject _uiUnit;
}
