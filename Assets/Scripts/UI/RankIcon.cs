using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        var score = SelectFilmBehavior.seedScore;

        _seedScoreIcon.ChangeIcon(score);

        if (score.obtained == 0)
        {
            score.obtained = 1;
        }

        _image.sprite = _rankIconResourseArray[--score.obtained];
    }

    [SerializeField] private Sprite[] _rankIconResourseArray = new Sprite[5];
    [SerializeField] private UnityEngine.UI.Image _image;
    [SerializeField] private SeedScoreIcon _seedScoreIcon;
}
