using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCallBackAnimation : MonoBehaviour
{
    [SerializeField] ResultEffect _eff;
    public void OnFinishAnimation(){
        _eff.FinishAnimation();
    }

    public void OnStart(){
        PlayerSoundManager.Instance.PlayPlayerSE(PlayerSESoundData.PlayerSE.Move);
    }

    public void OnStartJump(){
        PlayerSoundManager.Instance.PlayPlayerSE(PlayerSESoundData.PlayerSE.Jump);
    }

    public void OnFinishJump(){
        //PlayerSoundManager.Instance.PlayPlayerSE(Pla)
    }
}
