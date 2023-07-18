using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberFontImage : MonoBehaviour
{
    /*consts*/
    public enum NumberImage
    {
        _0,
        _1,
        _2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        _HYPHEN,
    }


    /*ŒöŠJ*/
    public NumberImage numberImage
    {
        get
        {
            return _numberImage;
        }
        set
        {
            _numberImage = value;
            UpdateImage();
        }
    }


    /*”ñŒöŠJ*/

    private void UpdateImage()
    {
        _imageCmp.sprite = _spriteArray[(int)_numberImage];
    }

    void OnValidate()
    {
        UpdateImage();
    }

    [SerializeField] private NumberImage _numberImage;
    [SerializeField] private Sprite[] _spriteArray = new Sprite[(int)NumberImage._HYPHEN + 1];
    [SerializeField] private Image _imageCmp;
}
