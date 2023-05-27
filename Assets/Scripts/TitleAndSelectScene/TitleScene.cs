using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SuzumuraTomoki;


public class TitleScene : MonoBehaviour
{

    private InputAction _MoveToSelect;
    [SerializeField] private GameObject _titleUI = null;
    [SerializeField] private GameObject _stageSelectUI = null;

    public InputAction MoveToSelect
	{
		get
		{
            return _MoveToSelect;
		}
	}

	private void Awake()
	{
        if (SceneManager.titleInitState == SceneManager.TitleInitState.STAGE_SELECT)
        {
            _stageSelectUI.SetActive(true);
            return;
        }

        //else
        SceneManager.playerInput.FindAction("StageSelectEnter").Disable();
        _stageSelectUI.SetActive(false);

        _MoveToSelect = SceneManager.playerInput.FindAction("TitleEnter");
        if (_MoveToSelect == null)
        {
            print("InputAction\"TitleEnter\"‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½");
        }

        _MoveToSelect.performed += ProcessMoveToSelect;

        _MoveToSelect.Enable();

        SystemSoundManager.Instance.PlayBGMWithFade(BGMSoundData.BGM.Title, 0.1f, 3);
    }

    private void ProcessMoveToSelect(InputAction.CallbackContext context)
	{
        _stageSelectUI.SetActive(true);
        _titleUI.SetActive(false);
        _MoveToSelect.performed -= ProcessMoveToSelect;
        _MoveToSelect.Disable();
    }

}
