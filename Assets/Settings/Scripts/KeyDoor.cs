using UnityEngine;
using DG.Tweening;

public class KeyDoor : MonoBehaviour
{
    //[SerializeField, Header("�����J������")] private float _openDoorTime;
    [SerializeField, Header("�A�j���[�V�����R���|�[�l���g")] private Animation _openAnim;
    [SerializeField, Header("�h�A�̓����蔻��")] private BoxCollider _doorCollider;
    private KeyManager _keyManager = null;
    private bool _isOpen = false;
    private bool _doOnce = false;

    void Start() {

        _keyManager = GameObject.Find("Pf_KeyManager").GetComponent<KeyManager>();
    
        if (_keyManager == null) {

            Debug.Log("Pf_KeyManager�����݂��܂���");
        }
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.transform.root.tag == "Player") {

            OpenDoor();
        }
    }

    private void OpenDoor() {

        //  // �h�A���󂢂ĂȂ��Ƃ�������1�ȏ㎝���Ă�Ƃ�
        if (!_isOpen && _keyManager.GetPlayerKeyNum >= 1) {

            // ���̐������炷
            _keyManager.DoorDetection();
            _isOpen = true;
        }
    }

    // �h�A���J���鏈��
    [System.Obsolete]
    private void FixedUpdate() {

        if (_isOpen) {

            if (!_doOnce) {
                _doOnce = true;
                _doorCollider.enabled = false;
                OpenDoorAnimation();

                GameSoundManager.Instance.PlayGameSE(GameSESoundData.GameSE.OpenDoor);
            }
        }
    }

    // �h�A�̃A�j���[�V����
    [System.Obsolete]
    private void OpenDoorAnimation() {

        _openAnim.Play();
    }
}
