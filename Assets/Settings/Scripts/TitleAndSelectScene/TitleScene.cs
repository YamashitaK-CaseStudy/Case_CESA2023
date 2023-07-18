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
    [SerializeField] private GameObject _stageSelectCanvus = null;
    [SerializeField] private GameObject _stageSelect = null;
    [SerializeField] private GameObject _stageSelectUI = null;
    [SerializeField] private GameObject _canvasPrologue = null;

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
            _stageSelectCanvus.SetActive(true);
            _stageSelect.SetActive(true);
            _stageSelectUI.SetActive(true);
            return;
        }

        //else
        SceneManager.playerInput.FindAction("StageSelectEnter").Disable();
        _stageSelectCanvus.SetActive(false);
        _stageSelectUI.SetActive(false);

        _MoveToSelect = SceneManager.playerInput.FindAction("TitleEnter");
        if (_MoveToSelect == null)
        {
            print("InputAction\"TitleEnter\"��������܂���ł���");
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
        // TODO:Wait����������
        //DOTween.TO
        _canvasGroup.DOFade(1.0f, 1f).OnComplete(()=> { _canvasPrologue.SetActive(true); });
        
    }

    public void FadeInSelect()
	{
        _MoveToSelect.Disable();
        _titleUI.SetActive(false);
        _stageSelectCanvus.SetActive(true);
        _stageSelectUI.SetActive(true);

        _stageSelect.SetActive(true);
        var SelectCmp = _stageSelect.GetComponent<SelectScene>();
        _canvasGroup.DOFade(0.0f, 0.5f).OnComplete(SelectCmp.EnableInput);
        SelectCmp.StopInput();
	}

}