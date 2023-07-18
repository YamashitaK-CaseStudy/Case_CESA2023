using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
	// Start is called before the first frame update
	// インスタンス
	//　ポーズした時に表示するUI
	[SerializeField] private GameObject pauseUIPrefub;
	private GameObject pauseUIInst;
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// 入力されたかどうか
		bool isChange = false;
		// 入力追加必要なここに
		if(Input.GetKeyDown ("space")) isChange = true;

		// 入力入ってなければ抜ける
		if(!isChange) return;

		if(pauseUIInst == null){
			Open();		// 開く
		}else {
			Close();	// 閉じる
		}
	}

	// ポーズ開く
	void Open(){
		Time.timeScale = 0.0f;
		pauseUIInst = GameObject.Instantiate(pauseUIPrefub);
	}
	// ポーズ閉じる
	void Close(){
		Time.timeScale = 1.0f;
		Destroy(pauseUIInst);
	}

}

