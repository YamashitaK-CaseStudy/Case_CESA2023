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
			Debug.Log("読めてねぇ");
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown("space")){
			FadeInStart();
		}
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

	public bool FinishFadeIn(){
		return !isRunFadeIn;
	}
	public bool FinishFadeOut(){
		return !isRunFadeIn;
	}

	public void FadeIn(){
		alpha += fadeInSpeed * Time.deltaTime;
		if(alpha >= 1.0f){
			alpha = 1.0f;
			isRunFadeIn = false;
			isRunFadeOut = true;
		}
		fadePanel.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
	}

	public void FadeOut(){
		alpha -= fadeOutSpeed * Time.deltaTime;
		if(alpha <= 0.0f){
			alpha = 0.0f;
			isRunFadeOut = false;
		}
		fadePanel.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
	}
}
