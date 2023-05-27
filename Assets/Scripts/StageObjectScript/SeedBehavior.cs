using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SeedBehavior : MonoBehaviour
{
    bool _isOnce = false;
    float _reqTime = 0;
    GameObject _child;
    Vector3 _childPos;
    float ypos;
    private void Awake()
    {
        //‘”‚ð‰Â•Ï’·‚É‚·‚é‚È‚ç
        //UiSeedBehavior.IncreaseTotal();
        _child = this.transform.GetChild(0).gameObject;
        _childPos = _child.transform.localPosition + this.transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_isOnce) return;
        if (other.transform.root.CompareTag("Player"))
        {
            UiSeedBehavior.ObtaineSeed();
            _isOnce = true;
            ypos = other.transform.position.y;
            this.transform.position = new Vector3(this.transform.position.x, ypos + 1f, this.transform.position.z);
            _child.transform.position = _childPos;

            this.transform.DORotate(new Vector3(0f, 180f, 0f), 0.25f).OnComplete(OnCompleteRotate);

            //1.5??????
            DOVirtual.DelayedCall(1.5f, ()=> Destroy(gameObject));
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
