using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class EffectEndDestroy : MonoBehaviour{

    [SerializeField, Header("�G�t�F�N�g�N�������~�܂ł̎���")] private float _effectStopTime;
    [SerializeField, Header("�G�t�F�N�g��~����Q�[���I�u�W�F�N�g�폜�܂ł̎���")] private float _objectDeleteTime;


    private float _effectStopCulTime = 0.0f;
    private float _objectDeleteCulTime = 0.0f;
    private bool _isEffectStop = false;
    private bool _isObjectDelete = false;

    // Start is called before the first frame update
    void Start(){
    }

    public void EffctStopTimerStart() {
        _isEffectStop = true;
    }

    // Update is called once per frame
    void Update() {

        if (_isObjectDelete) {
            _objectDeleteCulTime += Time.deltaTime;

            if (_objectDeleteCulTime > _objectDeleteTime) {
                GameObject.Destroy(this.gameObject);
            }
        }
        else if (_isEffectStop) {
            _effectStopCulTime += Time.deltaTime;

            if (_effectStopCulTime > _effectStopTime) {
                GetComponent<VisualEffect>().SendEvent("StopEffect");
                _isObjectDelete = true;
            }
        }
    }
}
