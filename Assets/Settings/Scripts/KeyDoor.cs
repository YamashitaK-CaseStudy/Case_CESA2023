using UnityEngine;
using DG.Tweening;

public class KeyDoor : MonoBehaviour
{
    //[SerializeField, Header("扉が開く時間")] private float _openDoorTime;
    [SerializeField, Header("アニメーションコンポーネント")] private Animation _openAnim;
    [SerializeField, Header("ドアの当たり判定")] private BoxCollider _doorCollider;
    private KeyManager _keyManager = null;
    private bool _isOpen = false;
    private bool _doOnce = false;

    void Start() {

        _keyManager = GameObject.Find("Pf_KeyManager").GetComponent<KeyManager>();
    
        if (_keyManager == null) {

            Debug.Log("Pf_KeyManagerが存在しません");
        }
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.transform.root.tag == "Player") {

            OpenDoor();
        }
    }

    private void OpenDoor() {

        //  // ドアが空いてないときかつ鍵が1つ以上持ってるとき
        if (!_isOpen && _keyManager.GetPlayerKeyNum >= 1) {

            // 鍵の数を減らす
            _keyManager.DoorDetection();
            _isOpen = true;
        }
    }

    // ドアを開ける処理
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

    // ドアのアニメーション
    [System.Obsolete]
    private void OpenDoorAnimation() {

        _openAnim.Play();
    }
}
