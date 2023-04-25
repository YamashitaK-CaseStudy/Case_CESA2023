using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public partial class Player : MonoBehaviour
{
	bool isWarp = false;
	bool isFadeOut = false;
	[SerializeField] string fadeObjName;	// フェードのオブジェクト名
	[SerializeField] string warpEnterObjName;	// ワープ前のオブジェクト名
	[SerializeField] string warpExitObjName;	// ワープ後のオブジェクト名
	Fade fade;
	GameObject flag;
	// Start is called before the first frame update
	void StartWarp()
	{
		fade = GameObject.Find(fadeObjName).GetComponent<Fade>();
		flag = GameObject.Find(warpExitObjName);
	}

	// Update is called once per frame
	void UpdateWarp()
	{
		//Debug.Log(this.gameObject.transform.position);
		// ワープ処理に入ってないなら処理しない
		if(!isWarp) return;

		if(fade.FinishFadeOut()){
			// ワープのフラグ削除
			isWarp = false;
			isFadeOut = false;
		}

		if(fade.FinishFadeIn()){
			// フェードアウト開始
			fade.FadeOutStart();
			isFadeOut = true;
		}

		if(!isFadeOut) return;
		// 座標更新
		this.gameObject.transform.position = flag.transform.position;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.collider.name != warpEnterObjName) return;

		fade.FadeInStart();
		isWarp = true;
	}

}
*/