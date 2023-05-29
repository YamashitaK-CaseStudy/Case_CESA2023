using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using SuzumuraTomoki;


public class TitleScene : MonoBehaviour
{

    private InputAction _MoveToSelect;
    [SerializeField] private GameObject _titleUI = null;
    [SerializeField] private GameObject _FadeUI = null;
    [SerializeField] private GameObject _stageSelectUI = null;

    private CanvasGroup _canvasGroup;
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
            SystemSoundManager.Instance.PlayBGMWithFade(BGMSoundData.BGM.Title,0.01f,1f);
            _titleUI.SetActive(false);
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


        _canvasGroup = _FadeUI.GetComponent<CanvasGroup>(); 
    }

    private void ProcessMoveToSelect(InputAction.CallbackContext context)
	{
     
        _MoveToSelect.performed -= ProcessMoveToSelect;
        SystemSoundManager.Instance.BGMFade(0.01f,2.0f);
        SystemSoundManager.Instance.PlaySE(SystemSESoundData.SystemSE.ToSelect);
        // TODO:Wait‚ð‚©‚¯‚½‚¢
        //DOTween.TO
        _canvasGroup.DOFade(1.0f, 1f).OnComplete(FadeInSelect);
        
    }

    private void FadeInSelect()
	{
        _MoveToSelect.Disable();
        _titleUI.SetActive(false);
        _stageSelectUI.SetActive(true);
        var SelectCmp = _stageSelectUI.GetComponent<SelectScene>();
        _canvasGroup.DOFade(0.0f, 1f).OnComplete(SelectCmp.EnableInput);
        SelectCmp.StopInput();
	}

}
