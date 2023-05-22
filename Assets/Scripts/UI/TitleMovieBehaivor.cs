using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuzumuraTomoki;

public class TitleMovieBehaivor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.titleInitState == SceneManager.TitleInitState.STAGE_SELECT)
        {
            SetMovie_toSelect();
            _videoPlayer.time = _selectBeginLoopPoint;
            return;
        }

        _titleInput.titleEnter.performed += CallbackTitleEnter;
    }

    private void OnDestroy()
    {
        _videoPlayer.loopPointReached -= PlayFromBeginLoopPoint;
    }

    private void SetMovie_toSelect()
    {
        _videoPlayer.clip = _videoToSelect;
        _videoPlayer.isLooping = false;
        _videoPlayer.loopPointReached += PlayFromBeginLoopPoint;
    }

    private void PlayFromBeginLoopPoint(UnityEngine.Video.VideoPlayer viPlayer)
    {
        viPlayer.Play();
        viPlayer.time = _selectBeginLoopPoint;
    }

    private void CallbackTitleEnter(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SetMovie_toSelect();
        _titleInput.titleEnter.performed -= CallbackTitleEnter;
    }

    [SerializeField] private double _selectBeginLoopPoint;
    [SerializeField] private UnityEngine.Video.VideoPlayer _videoPlayer;
    [SerializeField] private UnityEngine.Video.VideoClip _videoToSelect;
    [SerializeField] private TitleInput _titleInput;
}
