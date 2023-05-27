using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BlinklingText : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI _tmp;

    [Header("1ループの長さ(秒単位)")]
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float _duration = 1.0f;

    //開始時の色。
    [Header("ループ開始時の色")]
    [SerializeField]
    Color32 _startColor = new Color32(255, 255, 255, 255);

    //終了(折り返し)時の色。
    [Header("ループ終了時の色")]
    [SerializeField]
    Color32 _endColor = new Color32(255, 255, 255, 64);



    //インスペクターから設定した場合は、GetComponentする必要がなくなる為、Awakeを削除しても良い。
    void Awake()
    {
        if (_tmp == null)
            _tmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _tmp.color = Color.Lerp(_startColor, _endColor, Mathf.PingPong(Time.time / _duration, 1.0f));
    }

    void Enter()
	{
        
	}
}

