using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    private GameObject _touchColliderFront = null;
    private GameObject _touchColliderBottom = null;
    


    // Start is called before the first frame update
    void StartAction(){
        _touchColliderFront = this.transform.Find("FrontTouchCollider").gameObject;
        if ( _touchColliderFront == null ) {     
            Debug.Log("Front Touch Collider object does not exist");
        }

        _touchColliderBottom = this.transform.Find("BottomTouchCollider").gameObject;
        if ( _touchColliderBottom == null ) {
            Debug.Log("Bottom Touch Collider object does not exist");
        }

    }

    // Update is called once per frame
    void UpdateAction(){

        // 
        if ( Input.GetButtonDown("RotateSelfAxis") ) {


            // Get Input LeftStick Vertical
            var inputVart = Input.GetAxis("Vertical");

            // Rot AxisY Bottom
            if ( inputVart < -0.5 ) {
                if ( _touchColliderBottom == null ) {
                    Debug.Log("Bottom Touch Collider object does not exist");
                }
                else {
                    Debug.Log(_touchColliderBottom.name);
                }


                var touthObj = _touchColliderBottom.GetComponent<TouchCollider>();

                var targetRotObjVert = touthObj.GetTouchObject();

                if ( targetRotObjVert == null ) {
                    Debug.Log("There is no object to rotate.");
                    return;
                }

                targetRotObjVert.GetComponent<RotatableObject>().StartRotate(_touchColliderBottom.transform.position, Vector3.up);
                Debug.Log("Small rotation : axisY");

                return;

            }
            // Rot AxisX
            else {
                
                if ( _touchColliderFront == null ) {     
                    Debug.Log("Front Touch Collider object does not exist");
                }
                else {
                   Debug.Log(_touchColliderFront.name);
                }
                   

                var a = _touchColliderFront.GetComponent<TouchCollider>();

                var targetRotObj = a.GetTouchObject();
                            
                if ( targetRotObj == null ) {
                    Debug.Log("There is no object to rotate.");
                    return;
                }

                targetRotObj.GetComponent<RotatableObject>().StartRotate(_touchColliderFront.transform.position,Vector3.right);
                Debug.Log("Small rotation : axisX");

            }
        }

    }

}
