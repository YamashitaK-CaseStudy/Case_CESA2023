using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class ContactObtherber : MonoBehaviour
{
    private List<GameObject> _unionmaterials = new List<GameObject>();
    private GameObject _selectObj;

    void Start() {
    }

    void Update(){

        // �Q�ȏ㍇�̂���I�u�W�F�N�g�����X�g�̒��ɂ���Ύ��s����
        if (2 <= _unionmaterials.Count) {

            // �v���C���[�̑I�����Ă����I�u�W�F�N�g���擾
            _selectObj = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()._selectGameObject;

            // �V�����I�u�W�F�N�g���쐬����
            GameObject unionobj = Instantiate((GameObject)Resources.Load("Pf_UnionObj"), _selectObj.transform.position, Quaternion.identity);

            unionobj.tag = "RotateObject";
            unionobj.transform.position = _selectObj.transform.position;

            foreach (var obj in _unionmaterials) {

                // �쐬�����e�Ɏq���Ƃ��ă��X�g�̒��̃Q�[���I�u�W�F�N�g�A�^�b�`����
                obj.transform.SetParent(unionobj.transform);

                // �^�O��t���ւ���
                obj.tag = "Untagged";
            }

            // ���X�g�̒��g����ɂ���
            _unionmaterials.Clear();
        }
      
        

    }

    public void ContactCall(in GameObject collision) {

        if(_unionmaterials.Contains(collision) == false) {
            _unionmaterials.Add(collision);
        }
    }
}
