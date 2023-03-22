using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class ContactObtherber : MonoBehaviour
{
    private List<GameObject> _unionmaterials = new List<GameObject>();
    Vector3 localpos = new Vector3(0, 0, 0);
    Vector3 selectpos = new Vector3(0, 0, 0);

    void Start() {
    }

    void Update(){

        var _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player._selectGameObject.GetComponent<RotatableObject>()._isRotating == false) {
            selectpos = _player._selectGameObject.transform.position;
            localpos = _player._selectGameObject.GetComponent<RotatableObject>()._axisCenterLocalPos;
        }

        // �Q�ȏ㍇�̂���I�u�W�F�N�g�����X�g�̒��ɂ���Ύ��s����
        if (2 <= _unionmaterials.Count) {

            // �V�����I�u�W�F�N�g���쐬����
            GameObject unionobj = Instantiate((GameObject)Resources.Load("Pf_RootObj"), selectpos, Quaternion.identity);

            unionobj.tag = "RotateObject";
            unionobj.transform.position = localpos + selectpos;

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
