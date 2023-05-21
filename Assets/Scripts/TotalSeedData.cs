using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalSeedData : ScriptableObject
{
    public SeedIconData.TotalCountType GetDefaultTotalCount(int world, int stage)
    {
        int index = stage - 1 + (world - 1) * SelectFilmBehavior.MAX_STAGE;
        if (index > defaultTotalCountList.Count | index < 0)
        {
            Debug.LogError("SeedData：データが存在しませんでした " + world + "-" + stage);
            return SeedIconData.TotalCountType.THREE;
        }
        return (SeedIconData.TotalCountType)defaultTotalCountList[index] - SeedIconData.MIN_TOTAL;
    }

    [HideInInspector]
    public List<int> defaultTotalCountList;
}
