using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RankIcon : MonoBehaviour
{
    //public enum RankType
    //{
    //    C = 1,
    //    B,
    //    A,
    //    S,
    //    SS
    //}

    private int _seedObtained = 0;
    private int _nowDispNum = 0;
    private float _reqTime = 0f;
    private bool _isChange = false;
    private bool _isFinish = false;
    private bool _isOnce = false;
    DOTween aa;

    private void Start()
    {
        var score = UiSeedBehavior.seedScore;
        _seedObtained = score.obtained;
        _nowDispNum = _seedObtained;
        Debug.Log(_seedObtained);

        _seedScoreIcon.ChangeIcon(_nowDispNum);
        if (_nowDispNum == 0)
        {
            _nowDispNum = 1;
        }
        ChangeIcon(_nowDispNum - 1);
    }

    private void Update(){
        if(_isFinish) return;
        if(_isChange){
            _nowDispNum += 1;
            Debug.Log(_nowDispNum);
            ChangeIcon(_nowDispNum - 1);
            _seedScoreIcon.ChangeIcon(_nowDispNum);
            _isOnce= false;
            _isChange = false;
        }else{
            if(!_isOnce){
                // 待ち時間を計算
                var waitTime = 0.5f + _nowDispNum * 0.1f;
                // 演出
                this.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), waitTime);
                _seedScoreIcon.transform.DORotate(Vector3.up * 180f, waitTime / 2f);
                DOVirtual.DelayedCall(0.25f, ()=> _seedScoreIcon.transform.DORotate(Vector3.up * 0f, waitTime / 2f));
                // 終了しているかどうか確認
                if(_nowDispNum == _seedObtained){
                    _isFinish = true;
                    DOVirtual.DelayedCall(1.1f, ()=> this.transform.root.gameObject.GetComponent<ResultEffect>().FinishEvaluation());
                    return;
                }else if(_seedObtained == 0){
                    _isFinish = true;
                    DOVirtual.DelayedCall(1.1f, ()=> this.transform.root.gameObject.GetComponent<ResultEffect>().FinishEvaluation());
                    return;
                }
                // タイミング考慮してChangeに移行
                DOVirtual.DelayedCall(waitTime, ()=> _isChange = true);
                _isOnce = true;
            }
        }

    }

    private void ChangeIcon(int score){
        _image.sprite = _rankIconResourseArray[score];
        _image.transform.localScale = new Vector3(2.0f,2.0f,2.0f);
    }

    [SerializeField] private Sprite[] _rankIconResourseArray = new Sprite[5];
    [SerializeField] private UnityEngine.UI.Image _image;
    [SerializeField] private SeedScoreIcon _seedScoreIcon;
}
