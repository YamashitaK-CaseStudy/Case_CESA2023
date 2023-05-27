using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScoreIcon : MonoBehaviour
{
    public void ChangeIcon(SeedScore seedScore)
    {
        gameObject.GetComponent<UnityEngine.UI.Image>().sprite = _seedIconData.GetIcon(seedScore);
    }
    public void ChangeIcon(int seedScore)
    {
        gameObject.GetComponent<UnityEngine.UI.Image>().sprite = _seedIconData.GetIcon(seedScore);
    }
    
    [SerializeField] private SeedIconData _seedIconData;
}
