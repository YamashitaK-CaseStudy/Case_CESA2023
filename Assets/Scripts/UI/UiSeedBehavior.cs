using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSeedBehavior : MonoBehaviour
{
    /*静的メンバ*/

    //全体数可変長
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

    //    //start()以降

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
        //全体数可変長
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

    /*公開メンバ*/



    /*内部実装*/

    private void Start()
    {
        _instance = this;

        var canvusRect = GetComponent<RectTransform>().rect;


        //固定の場合
        SeedScore score = SelectFilmBehavior.seedScore;
        score.obtained = 0;
        _iconSeedScore.ChangeIcon(score);
        _totalSeeds = (int)score.total + SeedIconData.MIN_TOTAL;

        //可変の場合。種オブジェクトのAwake()で_totalSeedsを加算する。
        //全体数可変長
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

        //全体数可変長
        //score.total = (SeedIconData.TotalCountType)(_totalSeeds - SeedIconData.MIN_TOTAL);
        score.obtained = _countObtained;
        SelectFilmBehavior.seedScore = score;

        //_totalSeeds = 0;//全体数可変長
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
