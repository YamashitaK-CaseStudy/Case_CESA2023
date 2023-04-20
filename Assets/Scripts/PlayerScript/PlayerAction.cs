using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour {

    [SerializeField] private GameObject _frontColliderObj;
    [SerializeField] private GameObject _bottomColliderObj;

    private RotObjHitCheck _frontHitCheck = null;
    private RotObjHitCheck _bottomHitCheck = null;
    private PlayerInput _playerInput = null;
    private InputAction _rotationButton = null;
    private InputAction _rotationSpinButton = null;

    private StickRotAngle _stricRotAngle = null;

    void StartAction() {

        // 当たり判定のコンポーネント取得
        _frontHitCheck = _frontColliderObj.GetComponent<RotObjHitCheck>();
        _bottomHitCheck = _bottomColliderObj.GetComponent<RotObjHitCheck>();

        // InputSystem取得
        _playerInput = GetComponent<PlayerInput>();
        _rotationButton = _playerInput.actions.FindAction("Rotation");
        _rotationSpinButton = _playerInput.actions.FindAction("RotationSpin");

        // 右スティックの状態を取得
        _stricRotAngle = GetComponent<StickRotAngle>();
    }

    void UpdateAction() {

        // 左スティックのベクトルを取得する
        var stick = _playerInput.actions["AxisSelect"].ReadValue<Vector2>();

        // Front回転オブジェクトが切り替わった時
        if (_frontHitCheck.GetIsChangeRotHit) {
            _frontHitCheck.InitChangeRotHit();

            // 切り替えタイミング
            Debug.Log("フロント切り替えタイミング");

            _stricRotAngle.UDFB_Many_Jude(_frontHitCheck);
        }

        // Bottom回転オブジェクトが切り替わった時
        if (_bottomHitCheck.GetIsChangeRotHit) {
            _bottomHitCheck.InitChangeRotHit();

            // 切り替えタイミング
            Debug.Log("ボトム切り替えタイミング");

            _stricRotAngle.LRFB_Many_Jude(_bottomHitCheck);
        }

       
        // 前に回転オブジェクトがある時
        if (_frontHitCheck.GetIsRotHit) {

            // 左右に傾けたとき
            if (-0.5 > stick.x || 0.5 < stick.x) {

                var rotatbleComp = _frontHitCheck.GetRotObj.GetComponent<RotatableObject>();

                // 右スティックの更新
                _stricRotAngle.StickRotAngleX_Update();
                _stricRotAngle.UDFB_Many_Jude(_frontHitCheck);

                // 右スティックでの回転
                rotatbleComp.StartRotateX(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right,_stricRotAngle.GetStickDialAngleX);
               
                // 通常軸回転
                if (_rotationButton.WasPressedThisFrame()) {
                    rotatbleComp.StartRotate(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right,90);
                }
                // 高速回転
                if (_rotationSpinButton.WasPressedThisFrame()) {
                    rotatbleComp.StartSpin(CompensateRotationAxis(_frontColliderObj.transform.position), Vector3.right);
                }
            }
        }

        // 移動中とジャンプ中は回転させない
        if (0 < Mathf.Abs(moveVelocity.x) || 0 < moveVelocity.y) {
            return;
        }
        // 下に回転オブジェクトがある時
        if (_bottomHitCheck.GetIsRotHit) {
            var rotatbleComp = _bottomHitCheck.GetRotObj.GetComponent<RotatableObject>();

            // 右スティックの更新
            _stricRotAngle.StickRotAngleY_Update();
            _stricRotAngle.LRFB_Many_Jude(_bottomHitCheck);
          
            // 右スティックでの回転
            rotatbleComp.StartRotateY(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, _stricRotAngle.GetStickDialAngleY);

            // 通常軸回転
            if (_rotationButton.WasPressedThisFrame()) {
                rotatbleComp.StartRotate(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up, 90);
            }

            // 高速回転
            if (_rotationSpinButton.WasPressedThisFrame()) {
                rotatbleComp.StartSpin(CompensateRotationAxis(_bottomColliderObj.transform.position), Vector3.up);
            }
        }
    }

    /*
    private GameObject _touchColliderFront = null;
    private GameObject _touchColliderBottom = null;

    // Start is called before the first frame update
    void StartAction()
    {
        _touchColliderFront = this.transform.Find("FrontTouchCollider").gameObject;
        if (_touchColliderFront == null)
        {
            Debug.Log("Front Touch Collider object does not exist");
        }

        _touchColliderBottom = this.transform.Find("BottomTouchCollider").gameObject;
        if (_touchColliderBottom == null)
        {
            Debug.Log("Bottom Touch Collider object does not exist");
        }

    }

    // Update is called once per frame
    void UpdateAction()
    {

        // 
        if (Input.GetButtonDown("Rotate"))
        {


            // Get Input LeftStick Vertical
            var inputVart = Input.GetAxis("Vertical");

            // Rot AxisY Bottom
            if (inputVart < -0.3)
            {
                if (_touchColliderBottom == null)
                {
                    Debug.Log("Bottom Touch Collider object does not exist");
                }
                else
                {
                    Debug.Log(_touchColliderBottom.name);
                }


                var touthObj = _touchColliderBottom.GetComponent<TouchCollider>();

                // Get Target Object to Rotate
                var targetRotObjVert = touthObj.GetTouchObject();

                if (targetRotObjVert == null)
                {
                    Debug.Log("There is no object to rotateAxisY.");
                    return;
                }

                targetRotObjVert.GetComponent<RotatableObject>().StartRotate(CompensateRotationAxis(_touchColliderBottom.transform.position), Vector3.up);
                Debug.Log("Small rotation : axisY");

                return;

            }
            // Rot AxisX
            else
            {

                if (_touchColliderFront == null)
                {
                    Debug.Log("Front Touch Collider object does not exist");
                }
                else
                {
                    Debug.Log(_touchColliderFront.name);
                }


                var a = _touchColliderFront.GetComponent<TouchCollider>();

                var targetRotObj = a.GetTouchObject();

                if (targetRotObj == null)
                {
                    Debug.Log("There is no object to rotateAxisX.");
                    return;
                }

                targetRotObj.GetComponent<RotatableObject>().StartRotate(CompensateRotationAxis(_touchColliderFront.transform.position), Vector3.right);
                Debug.Log("Small rotation : axisX");

            }
        }

        // Spim Call
        if (Input.GetButtonDown("Spin"))
        {
            // Get Input LeftStick Vertical
            var inputVart = Input.GetAxis("Vertical");

            // Rot AxisY Bottom
            if (inputVart < -0.5)
            {
                if (_touchColliderBottom == null)
                {
                    Debug.Log("Bottom Touch Collider object does not exist");
                }
                else
                {
                    Debug.Log(_touchColliderBottom.name);
                }


                var touthObj = _touchColliderBottom.GetComponent<TouchCollider>();

                var targetRotObjVert = touthObj.GetTouchObject();

                if (targetRotObjVert == null)
                {
                    Debug.Log("There is no object to SpinAxisY.");
                    return;
                }

                targetRotObjVert.GetComponent<RotatableObject>().StartSpin(CompensateRotationAxis(_touchColliderBottom.transform.position), Vector3.up);
                Debug.Log("Spin : axisY");

                return;

            }
            // Rot AxisX
            else
            {

                if (_touchColliderFront == null)
                {
                    Debug.Log("Front Touch Collider object does not exist");
                }
                else
                {
                    Debug.Log(_touchColliderFront.name);
                }


                var a = _touchColliderFront.GetComponent<TouchCollider>();

                var targetRotObj = a.GetTouchObject();

                if (targetRotObj == null)
                {
                    Debug.Log("There is no object to SpinAxisX.");
                    return;
                }

                targetRotObj.GetComponent<RotatableObject>().StartSpin(CompensateRotationAxis(_touchColliderFront.transform.position), Vector3.right);
                Debug.Log("Spin : axisX");

            }

        }

    }
    */

    private Vector3 CompensateRotationAxis(in Vector3 AXIS)
    {
        /*回転するオブジェクトの大きさが１で位置が整数の場合のみ有効。
         * 
         * 拡張する場合
         * オブジェクトの基準位置（特別な理由がなければ原点でいい）とオブジェクトの大きさを定数定義して計算する。
         */

        return new Vector3(RoundOff(AXIS.x), RoundOff(AXIS.y));
    }

    private int RoundOff(float value)
    {
        int valueInt = (int)value;

        if (value - valueInt < 0.5f)
        {
            return valueInt;
        }

        return ++valueInt;
    }

}
