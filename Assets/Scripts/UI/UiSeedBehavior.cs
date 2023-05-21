using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSeedBehavior : MonoBehaviour
{
    /*�ÓI�����o*/

    //�S�̐��ϒ�
    //static public void IncreaseTotal()
    //{
    //    if (_totalSeeds >= SeedIconData.MAX_TOTAL)
    //    {
    //        return;
    //    }
    //    ++_totalSeeds;

    //    if (_instance == null)
    //    {
    //        return;
    //    }

    //    //start()�ȍ~

    //    if (_totalSeeds > SeedIconData.MIN_TOTAL)
    //    {
    //        SeedScore score;
    //        score.total = (SeedIconData.TotalCountType)(_totalSeeds - SeedIconData.MIN_TOTAL);
    //        score.obtained = _instance._countObtained;
    //        _instance._iconSeedScore.ChangeIcon(score);
    //    }
    //}

    static public void ObtaineSeed()
    {
        if (_instance == null)
        {
            return;
        }

        _instance.IncreaseCount();

        SeedScore score;
        //�S�̐��ϒ�
        //if (_totalSeeds <= SeedIconData.MIN_TOTAL)
        //{
        //    score.total = SeedIconData.TotalCountType.THREE;
        //}
        //else
        //{
        //    score.total = (SeedIconData.TotalCountType)(_totalSeeds - SeedIconData.MIN_TOTAL);
        //}

        score.total = (SeedIconData.TotalCountType)(_totalSeeds - SeedIconData.MIN_TOTAL);
        score.obtained = _instance._countObtained;

        _instance._iconSeedScore.ChangeIcon(score);

    }

    static private int _totalSeeds = 0;
    static private UiSeedBehavior _instance = null;

    /*���J�����o*/



    /*��������*/

    private void Start()
    {
        _instance = this;

        var canvusRect = GetComponent<RectTransform>().rect;


        //�Œ�̏ꍇ
        SeedScore score = SelectFilmBehavior.seedScore;
        score.obtained = 0;
        _iconSeedScore.ChangeIcon(score);
        _totalSeeds = (int)score.total + SeedIconData.MIN_TOTAL;

        //�ς̏ꍇ�B��I�u�W�F�N�g��Awake()��_totalSeeds�����Z����B
        //�S�̐��ϒ�
        //if (_totalSeeds > SeedIconData.MIN_TOTAL)
        //{
        //    SeedScore score;
        //    score.total = (SeedIconData.TotalCountType)(_totalSeeds - SeedIconData.MIN_TOTAL);
        //    score.obtained = _countObtained;
        //    _iconSeedScore.ChangeIcon(score);
        //}
    }

    private void OnDestroy()
    {
        SeedScore score = SelectFilmBehavior.seedScore;

        if(score.obtained >= _countObtained)
        {
            return;
        }

        //�S�̐��ϒ�
        //score.total = (SeedIconData.TotalCountType)(_totalSeeds - SeedIconData.MIN_TOTAL);
        score.obtained = _countObtained;
        SelectFilmBehavior.seedScore = score;

        //_totalSeeds = 0;//�S�̐��ϒ�
    }

    private void IncreaseCount()
    {
        if (_countObtained >= _totalSeeds)
        {
            return;
        }

        ++_countObtained;
    }

    [SerializeField] private SeedScoreIcon _iconSeedScore;
    private int _countObtained = 0;
}
