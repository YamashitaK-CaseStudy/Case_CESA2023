using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDestroy : MonoBehaviour{

    // アタッチされているオブジェクトを削除する
    public void Destroy() {
        Destroy(this.gameObject);   
    }

}
