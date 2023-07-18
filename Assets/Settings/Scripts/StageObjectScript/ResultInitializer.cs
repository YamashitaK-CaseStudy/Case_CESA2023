using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultInitializer : MonoBehaviour
{
    void Start()
    {
        if (SuzumuraTomoki.SceneManager.missionFailed)
        {
            _win.SetActive(false);
            _lose.SetActive(true);

            SuzumuraTomoki.SceneManager.missionFailed = false;
            return;
        }

        _win.SetActive(true);
        _lose.SetActive(false);
        //switch (SelectFilmBehavior.seedScore.obtained)
        //{
        //    case 0:
        //    case 1:
        //    case 2:
        //        _win.SetActive(false);
        //        _lose.SetActive(true);
        //        break;
        //    default:
        //        _win.SetActive(true);
        //        _lose.SetActive(false);
        //        break;
        //}

    }

    [SerializeField] private GameObject _win;
    [SerializeField] private GameObject _lose;
}
