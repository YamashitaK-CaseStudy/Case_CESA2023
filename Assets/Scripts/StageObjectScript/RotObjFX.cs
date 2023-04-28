using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour
{
	private RotObjChildFX[] _childParticle;
	private int _childParticleNum;
	void SettingPartical(){
		_childParticleNum = this.gameObject.transform.GetChild(0).childCount;
		// 配列を設定
		_childParticle = new RotObjChildFX[_childParticleNum];
		for(int i = 0; i < _childParticleNum; i++){
			_childParticle[i] = this.gameObject.transform.GetChild(0).GetChild(i).GetComponent<RotObjChildFX>();
		}
	}
	// トレイルパーティクルの起動
	void PlayPartical(){
		for(int i = 0; i < _childParticleNum; i++){
			_childParticle[i].PlayPartical();
		}
	}
	// トレイルパーティクルの停止
	void StopPartical(){
		for(int i = 0; i < _childParticleNum; i++){
			_childParticle[i].StopPartical();
		}
	}
}
