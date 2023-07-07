using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyUI : MonoBehaviour
{
    // Start is called before the first frame update

    private KeyManager _keyManager;
    private UnityEngine.UI.Image _keyImage;

    void Start()
    {
        _keyImage = this.transform.GetComponent<UnityEngine.UI.Image>();
        _keyManager = GameObject.Find("Pf_KeyManager").GetComponent<KeyManager> ();
    }

    // Update is called once per frame
    void Update(){
        
        if(_keyManager.GetPlayerKeyNum > 0) {
            _keyImage.enabled = true;
        }
        else {
            _keyImage.enabled = false;
        }
    }
}
