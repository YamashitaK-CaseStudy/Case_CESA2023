using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour{

    // ������t���O
    private bool _isPossession { get; set; } = false;

    void StartPossession(){
    }

    void UpdatePossession(){

        if (Input.GetButtonDown("Possession")) {
            if (_isPossession == false) {
                OnPossession();
            }
            else {
                OffPossession();
            }
        }

    }

    private void OnPossession() {
       
        if(_selectGameObject != null) {
            _isPossession = true;

            // ����̉�]���s��
            _selectGameObject.GetComponent<MeshRenderer>().material.color = Color.red;

            // �}�e���A���̐F��ύX����
            this.GetComponent<MeshRenderer>().material.color = Color.gray;


            _selectGameObject.GetComponent<RotatableObject>().RotateSmallAxisSelf();

        }
    }

    private void OffPossession() {
        _isPossession = false;

        // �}�e���A���̐F��ύX����
        _selectGameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
        this.GetComponent<MeshRenderer>().material.color = Color.red;

    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.name == "RArm" || other.transform.gameObject.name == "LArm") {
            transform.SetParent(other.transform);

            Debug.Log("�̂ĂȂ�");
        }
    }

    private void OnTriggerExit(Collider other) {
        transform.SetParent(null);
    }
}
