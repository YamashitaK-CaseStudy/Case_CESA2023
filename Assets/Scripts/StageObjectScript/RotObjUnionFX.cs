using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public partial class RotObjUnion : MonoBehaviour
{
	[SerializeField] private VisualEffect _effect;		// 発生させるエフェクト
	[SerializeField] private ParticleSystem _partical;		// 発生させるエフェクト
	[SerializeField] private string _StartEventName;	// 発生させるエフェクトのイベント名を入れる
	private void EffectGenerate(GameObject contactObj){
		if(_effect == null && _partical == null) return;
		if(contactObj.transform.root.gameObject.tag != "RotateObject") return;
		if(this.transform.root.gameObject == contactObj.transform.root.gameObject) return;
		if(contactObj.name == "Pf_Parts") return;
		SetGeneratePosition(contactObj);
	}

	private void SetGeneratePosition(GameObject contactObj){
		int generateAxis = 0;
		Vector3 generatePos = new Vector3(0,0,0);	// 座標
		Vector3 generateAngle = new Vector3(0,0,0);	// 角度

		// 相手と自分のワールド座標系の比較
		var pos1 = this.transform.position;
		var pos2 = contactObj.transform.position;
		Debug.Log(pos1);
		Debug.Log(pos2);
		// 同じ座標系の軸を所得
		if(pos1.x >= pos2.x - 0.5f && pos1.x <= pos2.x + 0.5f){
			generateAxis = 1;	// Y軸に発生
		}

		if(pos1.y >= pos2.y - 0.5f && pos1.y <= pos2.y + 0.5f){
			generateAxis = 2;	// X軸に発生
		}

		// 同じではないほうの座標を比較して発生する座標を決定する
		switch(generateAxis){
			case 1:
			Debug.Log("y軸に生成するで");
				if(pos1.y < pos2.y){
					Debug.Log("上に生成するで");
					generateAngle = new Vector3(0,0,0);
					generatePos.y += 0.5f;
				}else{
					Debug.Log("下に生成するで");
					generateAngle = new Vector3(180,0,0);
					generatePos.y -= 0.5f;
				}
			break;
			case 2:
			Debug.Log("x軸に生成するで");
				if(pos1.x < pos2.x){
					generateAngle = new Vector3(0,0,90);
					generatePos.x += 0.5f;
				}else{
					generateAngle = new Vector3(0,0,270);
					generatePos.x -= 0.5f;
				}
			break;
			default:
				Debug.LogError("Error");
			return;
		}
		generatePos.z = -0.5f;
		// 最終的な角度と座標を格納する
		_effect.transform.position = generatePos + this.transform.position;
		_effect.transform.eulerAngles = generateAngle;
		_effect.SendEvent(_StartEventName);
	}
}
