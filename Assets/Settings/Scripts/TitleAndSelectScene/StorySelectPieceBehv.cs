using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySelectPieceBehv : MonoBehaviour
{

    public StoryPage Story
    {
        set
        {
            _image.sprite = value._image;
            _text.text = value._storyText;
        }
    }

    public Sprite Image
    {
        set
        {
            _image.sprite = value;
        }
    }

    public string Text
    {
        set
        {
            _text.text = value;
        }
    }

    [SerializeField] private UnityEngine.UI.Image _image;
    [SerializeField] private UnityEngine.UI.Text _text;
}
