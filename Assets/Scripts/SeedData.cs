using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SeedData : ScriptableObject
{
    public int GetDefaultTotalCount(int world, int stage)
    {
        int index = stage - 1 + (world - 1) * SelectFilmBehavior.MAX_STAGE;
        if (index >= defaultTotalCountList.Count)
        {
            Debug.LogError("SeedData：そのステージのデータはないぜ");
            return 0;
        }
        return defaultTotalCountList[index];
    }

    [HideInInspector]
    public List<int> defaultTotalCountList;
}
