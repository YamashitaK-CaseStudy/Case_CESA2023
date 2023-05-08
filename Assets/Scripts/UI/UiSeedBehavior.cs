using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSeedBehavior : MonoBehaviour
{
    /*静的メンバ*/
    static public void IncreaseTotal()
    {
        ++_totalSeeds;
    }

    static public void ObtaineSeed()
    {
        if (_instance == null)
        {
            return;
        }

        _instance.Obtaine();
    }

    static public int totalSeeds
    {
        get
        {
            return _totalSeeds;
        }
    }

    static private int _totalSeeds = 0;
    static private UiSeedBehavior _instance = null;

    /*公開メンバ*/



    /*内部実装*/

    private void Start()
    {
        _instance = this;

        var canvusRect = GetComponent<RectTransform>().rect;
        for (int i = 0; i < _totalSeeds; ++i)
        {
            var rectTrans = Instantiate(_iconEmpty, transform).GetComponent<RectTransform>();
            rectTrans.anchoredPosition = new Vector2(rectTrans.rect.width / 2 - canvusRect.width / 2 + _offsetFromUpperLeft.x + (i * (rectTrans.rect.width + _iconDistance)), canvusRect.height / 2 - rectTrans.rect.height / 2 - _offsetFromUpperLeft.y);
        }

        int alreadyObtained = SelectFilmBehavior.obtainedCount;

        for (int i = 0; i < alreadyObtained; ++i)
        {
            Obtaine();
        }
    }

    private void OnDestroy()
    {
        _totalSeeds = 0;
        SelectFilmBehavior.obtainedCount = _countObtained;
    }

    private void Obtaine()
    {
        if (_countObtained >= _totalSeeds)
        {
            return;
        }

        transform.GetChild(_countObtained).GetComponent<UnityEngine.UI.Text>().text = "X";
        ++_countObtained;
    }

    [SerializeField] private GameObject _iconSeed;
    [SerializeField] private GameObject _iconEmpty;
    [SerializeField] private float _iconDistance = .1f;
    [SerializeField] private Vector2 _offsetFromUpperLeft;
    private int _countObtained = 0;
}
