using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {

    // �v���C���[���擾���Ă錮�̐�
    private int _playerKeyNum = 0;

    public int GetPlayerKeyNum {
        get { return _playerKeyNum; }
    }

    void Start() {
    }

    // �v���C���[�������擾�����Ƃ��ɌĂ΂��֐�
    public void PlayerGetKey(GameObject _keyObject) {

        Destroy(_keyObject);
        _playerKeyNum++;
    }

    // �h�A���v���C���[�����m�����Ƃ��Ă΂�鏈��
    public void DoorDetection() {
        _playerKeyNum--;
    }

    private void Update() {

        Debug.Log("���݂̌��̐�" + _playerKeyNum);
    }
}
