using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerskAnimationCallBack : MonoBehaviour{

    // 回転アニメーション中フラグ
    private bool _isRotationAnimPlay = false;

    // 実際に回転できるようになるまでのフラグ
    private bool _isRotationValid = false;

    // 大ジャンプ出来るまでの待ちフラグ
    private bool _isBigJumpValid = false;

    // ジャンプ時のエフェクトフラグ
    private bool _isJumpEffectPlay = false;

    public void AnimRotStart() {
        //Debug.Log("回転アニメーション開始");
        _isRotationAnimPlay = true;
    }

    public void AnimRotEnd() {
        //Debug.Log("回転アニメーション終了");
        _isRotationAnimPlay = false;
    }

    public void RotationValid() {
        //Debug.Log("回転動作可能状態");
        _isRotationValid = true;
    }

    public void RotationInValid() {
        _isRotationValid = false;
    }

    // 大ジャンプ可能状態
    public void BigJumpVaild() {
        _isBigJumpValid = true;
    }

    // 大ジャンプ可能状態
    public void BigJumpInVaild() {
        _isBigJumpValid = false;
    }

    // ロックSE
    public void PlayerSE_Lock() {
        Debug.Log("ロックSE");
        PlayerSoundManager.Instance.PlayPlayerSE(PlayerSESoundData.PlayerSE.Lock);
    }

    // プレイヤー回転DE
    public void PlayerSE_Rotation() {
        Debug.Log("プレイヤー回転SE");
        PlayerSoundManager.Instance.PlayPlayerSE(PlayerSESoundData.PlayerSE.Rotation);
    }

    // プレイヤージャンプ時のエフェクト起動開始
    public void PlayerJumpEffectPlay() {
        // エフェクトを起動する
        _isJumpEffectPlay = true;
    }

    // プレイヤージャンプ時のエフェクト停止
    public void PlayerJumpEffectStop() {
        // エフェクトを起動する
         _isJumpEffectPlay = false;
    }

    // Getter
    public bool GetIsRotationAnimPlay {
        get { return _isRotationAnimPlay; }
    }

    public bool GetIsRotationValid{
        get{ return _isRotationValid; }
    }

    public bool GetIsBigJumpVaild {
        get { return _isBigJumpValid; }
    }

    public bool GetIsJumpEffectPlay {
        get { return _isJumpEffectPlay; }
    }
}
