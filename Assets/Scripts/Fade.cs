using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
	[SerializeField] float fadeInSpeed;
	[SerializeField] float fadeOutSpeed;
	float alpha = 0.0f;
	bool isRunFadeIn = false, isRunFadeOut = false;
	GameObject fadePanel;
	// Start is called before the first frame update
	void Start()
	{
		fadePanel = transform.GetChild(0).gameObject;
		if(fadePanel == null){
			Debug.LogError("Fadeのオブジェクトが読めてない");
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(isRunFadeIn){
			FadeIn();
			return;
		}
		if(isRunFadeOut){
			FadeOut();
			return;
		}
	}
	public void FadeInStart(){
		isRunFadeIn = true;
	}

	public void FadeOutStart(){
		isRunFadeOut = true;
		Debug.Log(isRunFadeOut);
	}

	public bool FinishFadeIn(){
		if(alpha == 1.0f) return true;
		return false;
	}

	public bool FinishFadeOut(){
		if(alpha == 0.0f) return true;
		return false;
	}

	private void FadeIn(){
		alpha += fadeInSpeed * Time.deltaTime;
		if(alpha >= 1.0f){
			alpha = 1.0f;
			isRunFadeIn = false;
		}
		fadePanel.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
	}

	private void FadeOut(){
		alpha -= fadeOutSpeed * Time.deltaTime;
		if(alpha <= 0.0f){
			alpha = 0.0f;
			isRunFadeOut = false;
		}
		fadePanel.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
	}
}
