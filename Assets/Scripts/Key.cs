using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Key : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
    [SerializeField] private Vector3 _animOffsetMove;
    private bool isAnimation = false;

    void Update() {

        // 常に回転
        this.transform.rotation *= Quaternion.AngleAxis(_rotSpeed,Vector3.up);
    }

    // 鍵がプレイヤーに取得された時呼ばれる関数
    public void ToBeObtained() {

        // 取得されたら鍵を連続で取得できないように当たり判定を消す
        this.GetComponent<BoxCollider>().enabled = false;

        this.transform.DOMove(_animOffsetMove, 0.8f).SetRelative().OnComplete(AnimationComplete);
    }
    
    // アニメーションが完了したら自身を削除する
    private void AnimationComplete() {

        Destroy(this.gameObject);
    }
}
