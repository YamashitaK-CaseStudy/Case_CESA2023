using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class SeedBehavior : MonoBehaviour
{
    bool _isOnce = false;
    float _reqTime = 0;
    GameObject _child;
    float ypos;
    [SerializeField] VisualEffect _GetEffect;
    private void Awake()
    {
        //‘”‚ð‰Â•Ï’·‚É‚·‚é‚È‚ç
        //UiSeedBehavior.IncreaseTotal();
        _child = this.transform.GetChild(0).gameObject;
        Debug.Log(this.transform.position);
        _GetEffect.transform.parent = null;
        _GetEffect.transform.position = this.transform.position;
        Debug.Log(_GetEffect.transform.position);
        _GetEffect.transform.localEulerAngles = this.transform.localEulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_isOnce) return;
        if (other.transform.root.CompareTag("Player"))
        {
            UiSeedBehavior.ObtaineSeed();
            _isOnce = true;

            _GetEffect.transform.position = this.transform.position;
            _GetEffect.SendEvent("OnPlay");

            var pos = this.transform.position;
            pos.y += 1;
            this.transform.DOMove(pos,0.5f);

            this.transform.DORotate(new Vector3(0f, 180f, 0f), 0.25f).OnComplete(OnCompleteRotate);

            //1.5??????
            DOVirtual.DelayedCall(1.0f, ()=> Destroy(gameObject));

            // SE
            PlayerSoundManager.Instance.PlayPlayerSE(PlayerSESoundData.PlayerSE.GetSeed);
        }
    }

    private void OnCompleteRotate(){
        if(this == null) return;
        this.transform.DORotate(new Vector3(0f, 360f, 0f), 0.25f).OnComplete(OnCompleteRotate360);
    }

    private void OnCompleteRotate360(){
        if(this == null) return;
        this.transform.DORotate(new Vector3(0f, 180f, 0f), 0.25f).OnComplete(OnCompleteRotate);
    }
}
