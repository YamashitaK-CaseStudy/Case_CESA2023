using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftCollider : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerStay(Collider other) {

        var myobj = this.transform.root.gameObject;
        var youObj = other.transform.root.gameObject;

        // ���g�����t�g�ő��肪��]�I�u�W�F�N�g�ł���ꍇ
        if (myobj.tag == "Lift" && youObj.tag == "RotateObject") {

            // ���t�g�̏��𑊎�̉�]�I�u�W�F�N�g�ɓ`����
            youObj.GetComponent<LiftWithMoveInfo>().LiftObj = myobj;

            // ���t�g�ɉ�]�I�u�W�F�N�g��ǉ�����
            myobj.GetComponent<Lift>().AddMoveRotObj(youObj);
        }

        // ���g����]�I�u�W�F�N�g�ő��肪��]�I�u�W�F�N�g�ł���ꍇ
        else if (myobj.tag == "RotateObject" && youObj.tag == "RotateObject") {

            // ���������t�g�̏��������Ă������]�I�u�W�F�N�g�ɓ`����
            var myhavelift = myobj.GetComponent<LiftWithMoveInfo>().LiftObj;
            if (myhavelift != null) {
                youObj.GetComponent<LiftWithMoveInfo>().LiftObj = myhavelift;
                myhavelift.GetComponent<Lift>().AddMoveRotObj(youObj);
            }
        }

        // ���g����]�I�u�W�F�N�g�ő��肪�s���I�u�W�F�N�g�ł���ꍇ
        else if (myobj.tag == "RotateObject" && youObj.name == "Floor") {

            // ���g���s���I�u�W�F�N�g�ɂ���������~�߂�t���O�𗧂Ă�
            myobj.GetComponent<LiftWithMoveInfo>().IsStopMove = true;
            Debug.Log("���g����]�I�u�W�F�N�g�ő��肪�s���I�u�W�F�N�g�ł���ꍇ");
        }

        // ���̏�Ԃ͂��܂�Ȃ�
        else if (myobj.tag == "Lift" && youObj.name == "Floor") {

        }
    }
}
