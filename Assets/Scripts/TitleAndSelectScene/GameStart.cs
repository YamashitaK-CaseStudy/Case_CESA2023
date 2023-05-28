using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    [SerializeField] Fade _startFade;
    [SerializeField] GameObject _titleInputObj;
    [SerializeField] GameObject _titleCanvasObj;
    // Start is called before the first frame update
    void Start()
    {
        _startFade.FadeOut(3, ActivaTitle);
    }

	private void ActivaTitle(){
        _titleCanvasObj.SetActive(true);
        _titleInputObj.SetActive(true);
	}
}
