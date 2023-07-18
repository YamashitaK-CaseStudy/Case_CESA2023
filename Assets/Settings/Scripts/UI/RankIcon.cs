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
        _nowDispNum = 0;

        _seedScoreIcon.ChangeIcon(_nowDispNum);
        ChangeIcon(_nowDispNum);
    }

    private void Update(){
        if(_isFinish) return;
        if(_isChange){
            _nowDispNum += 1;
            ChangeIcon(_nowDispNum);
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
                    DOVirtual.DelayedCall(0.5f, CheckUnlockStory);
                    return;
                }else if(_seedObtained == 0){
                    _isFinish = true;
                    DOVirtual.DelayedCall(0.5f, CheckUnlockStory);
                    return;
                }
                // タイミング考慮してChangeに移行
                DOVirtual.DelayedCall(waitTime, ()=> _isChange = true);
                _isOnce = true;
            }
        }

    }

    public void TurnUpBGM()
    {
        SystemSoundManager.Instance.BGMFade(0.1f, 0.5f);
    }

    private void ChangeIcon(int score){
        if(score == 5){
            score = 4;
        }
        _image.sprite = _rankIconResourseArray[score];
        _image.transform.localScale = new Vector3(2.0f,2.0f,2.0f);
    }

    private void CheckUnlockStory()
    {
        ref var storyData = ref _storyData._storyArray[StoryArchive.NextUnlockStoryId];
        if (SelectFilmBehavior.SeedScore >= storyData._unlockNum)
        {
            _storyCanvas.GetComponent<PrologueBehv>().StoryPageArray = storyData._storyArray;
            _storyCanvas.SetActive(true);

            StoryArchive.NextUnlockStoryId = StoryArchive.NextUnlockStoryId + 1;

            SystemSoundManager.Instance.BGMFade(0, 0.5f);
            transform.root.GetComponent<UnityEngine.InputSystem.PlayerInput>().SwitchCurrentActionMap("Player");
            return;
        }

        transform.root.gameObject.GetComponent<ResultEffect>().FinishEvaluation();
    }

    [SerializeField] private Sprite[] _rankIconResourseArray = new Sprite[5];
    [SerializeField] private UnityEngine.UI.Image _image;
    [SerializeField] private SeedScoreIcon _seedScoreIcon;
    [SerializeField] private GameObject _storyCanvas;
    [SerializeField] private Story.StoryData _storyData;
}
