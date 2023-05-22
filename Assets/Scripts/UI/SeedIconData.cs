using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteList
{
    public List<Sprite> _spriteList;
}

public class SeedIconData : ScriptableObject
{
    /*�萔*/
    public const int MAX_TOTAL = 5;
    public const int MIN_TOTAL = 3;
    public enum TotalCountType
    {
        THREE,
        FOUR,
        FIVE,
        MAX_TYPE
    }

    /*���J*/
    public Sprite GetIcon(SeedScore seedScore)
    {
        if (seedScore.obtained < 0)
        {
            seedScore.obtained = 0;
        }

        int intTotal = (int)seedScore.total + SeedIconData.MIN_TOTAL;
        if (seedScore.obtained > intTotal)
        {
            seedScore.obtained = intTotal;
        }

        return _spriteArray[(int)seedScore.total]._spriteList[seedScore.obtained];
    }

    /*����J*/
    [SerializeField] private SpriteList[] _spriteArray;
}
