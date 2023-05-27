using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCallBackAnimation : MonoBehaviour
{
    [SerializeField] ResultEffect _eff;
    public void OnFinishAnimation(){
        _eff.FinishAnimation(true);
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

    public void OnFinishAnimationLose(){
        _eff.FinishAnimation(false);
    }

    public void OnStartLose(){
        
    }
    public void OnDestoroy(){

    }
}
