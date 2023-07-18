using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    [SerializeField] Fade _startFade;
    [SerializeField] GameObject _titleInputObj;
    [SerializeField] GameObject _titleCanvasObj;

    static bool _isStarted = false;
	// Start is called before the first frame update

	void Start()
    {
		if (_isStarted)
		{
            _startFade.FadeOut(0, ActivaTitle);
            return;
        }

        _startFade.FadeOut(3, ActivaTitle);
        SystemSoundManager.Instance.PlayBGMWithFade(BGMSoundData.BGM.Title, 0.1f, 3);
        
        _isStarted = true;
    }

	private void ActivaTitle(){
        _titleCanvasObj.SetActive(true);
        _titleInputObj.SetActive(true);
	}
}
