using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{
    // Start is called before the first frame update
    void StartAction(){
        
    }

    // Update is called once per frame
    void UpdateAction(){

        if ( Input.GetButtonDown("RotateSelfAxis") ) {
            Debug.Log("���̏�ł܂킷���A�N�V�����I");

            if ( _selectGameObject == null) {
                return;
            }

            // �܂킷�����N������֐����Ăяo��
            _selectGameObject.GetComponent<RotatableObject>().StartRotate();
   
        }

        if ( Input.GetButtonDown("RotateExturnAxis") ) {
            Debug.Log("���̏�ł܂킷��A�N�V�����I");

            if ( _selectGameObject == null ) {
                return;
            }

            // �܂킷�����N������֐����Ăяo��
            _selectGameObject.GetComponent<RotatableObject>().StartSpin();
        }

    }

}
