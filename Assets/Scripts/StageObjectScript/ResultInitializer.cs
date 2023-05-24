using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SuzumuraTomoki.SceneManager.missionFailed)
        {
            _win.SetActive(false);
            _lose.SetActive(true);
            _uiScore.SetActive(false);
            _uiRank.SetActive(false);

            SuzumuraTomoki.SceneManager.missionFailed = false;
            return;
        }

        switch (SelectFilmBehavior.seedScore.obtained)
        {
            case 0:
            case 1:
            case 2:
                _win.SetActive(false);
                _lose.SetActive(true);
                break;
            default:
                _win.SetActive(true);
                _lose.SetActive(false);
                break;
        }
    }

    [SerializeField] private GameObject _win;
    [SerializeField] private GameObject _lose;
    [SerializeField] private GameObject _uiScore;
    [SerializeField] private GameObject _uiRank;
}
