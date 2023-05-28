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
    Vector3 _childPos;
    float ypos;
    [SerializeField] VisualEffect _GetEffect;
    private void Awake()
    {
        //�������ϒ��ɂ���Ȃ�
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

            _GetEffect.transform.position = this.transform.position;
            _GetEffect.SendEvent("OnPlay");

            ypos = other.transform.position.y;
            var pos = new Vector3(this.transform.position.x, ypos + 1f, this.transform.position.z);
            this.transform.DOMove(pos,0.5f);
            _child.transform.position = _childPos;

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
