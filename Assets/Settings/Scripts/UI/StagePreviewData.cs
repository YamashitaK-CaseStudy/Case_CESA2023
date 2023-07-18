using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePreviewData : ScriptableObject
{
    public Sprite GetSprite(int world, int stage)
    {
        if (world > SelectFilmBehavior.MAX_WORLD | world < 1
            | stage > SelectFilmBehavior.MAX_STAGE | stage < 1)
        {
            Debug.Log("StagePreviewData : ƒf[ƒ^‚ª‘¶Ý‚µ‚Ü‚¹‚ñ " + world + "-" + stage);
        }
        return _worldArray[world - 1]._spriteList[stage - 1];
    }

    [SerializeField] private SpriteList[] _worldArray = new SpriteList[SelectFilmBehavior.MAX_WORLD];
}
