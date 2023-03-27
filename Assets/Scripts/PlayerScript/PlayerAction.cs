using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    private GameObject _touchColliderFront = null;
    


    // Start is called before the first frame update
    void StartAction(){
        _touchColliderFront = this.transform.Find("FrontTouchCollider").gameObject;
  
    }

    // Update is called once per frame
    void UpdateAction(){

        if ( Input.GetButtonDown("RotateSelfAxis") ) {
           

            if ( _touchColliderFront == null ) {     
                Debug.Log("�܂킷���{�^���������ꂽ");
            }
            else {
               Debug.Log(_touchColliderFront.name);
            }
               

            var a = _touchColliderFront.GetComponent<TouchCollider>();

            var targetRotObj = a.GetTouchObject();
                        
            if ( targetRotObj == null ) {
                Debug.Log("�Ώۂ̃I�u�W�F�N�g������܂���");
                return;
            }

            targetRotObj.GetComponent<RotatableObject>().StartRotate(_touchColliderFront.transform.position,Vector3.right);
            Debug.Log("�܂킷��");

        }

    }

}
