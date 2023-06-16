using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {

    // プレイヤーが取得してる鍵の数
    private int _playerKeyNum = 0;

    public int GetPlayerKeyNum {
        get { return _playerKeyNum; }
    }

    void Start() {
    }

    // プレイヤーが鍵を取得したときに呼ばれる関数
    public void PlayerGetKey(GameObject _keyObject) {

        Destroy(_keyObject);
        _playerKeyNum++;
    }

    // ドアがプレイヤーを検知したとき呼ばれる処理
    public void DoorDetection() {
        _playerKeyNum--;
    }

    private void Update() {

        Debug.Log("現在の鍵の数" + _playerKeyNum);
    }
}
