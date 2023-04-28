using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{
  
    private void PlayerRotationStart() {

        // 各種ボタンの取得
        _rotationButton     = _playerInput.actions.FindAction("Rotation");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");
    }

    private void PlayerRotationUpdate() {

        // 移動中、ジャンプ中は回転させない
        if(Mathf.Abs( _rigidbody.velocity.x) > 0 || !_groundCheck.IsGround) {
            return;
        }

        // 下に回転オブジェクトがある場合無条件で優先
        
        // 真下に回転オブジェクトがある時
        if (_bottomHitCheck.GetIsRotHit) {

            var rotatbleComp = _bottomHitCheck.GetRotObj.GetComponent<RotatableObject>();

            // 通常軸回転
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, 90);
            }

            // 高速回転
            if (_rotationSpinButton.WasPressedThisFrame()) {
                rotatbleComp.StartSpin(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up);
            }
        }

        // 前方に回転オブジェクトがある時
        else if (_frontHitCheck.GetIsRotHit) {

            var rotatbleComp = _frontHitCheck.GetRotObj.GetComponent<RotatableObject>();

            // 通常軸回転
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right, 90);
            }

            // 高速回転
            if (_rotationSpinButton.WasPressedThisFrame()) {
                rotatbleComp.StartSpin(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right);
            }
        }
    }
}
