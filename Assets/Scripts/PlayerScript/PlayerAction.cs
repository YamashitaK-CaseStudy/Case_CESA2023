using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{

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
