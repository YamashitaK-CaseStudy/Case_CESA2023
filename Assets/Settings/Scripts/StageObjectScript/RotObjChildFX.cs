using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjChildFX : MonoBehaviour
{
	[SerializeField] ParticleSystem _particle;
	ParticleSystem _particleObj;
	void Start()
	{
		_particleObj = Instantiate(_particle,this.transform.position,Quaternion.identity);
		_particleObj.name = "trailParticle";
		_particleObj.gameObject.transform.parent = this.transform;
	}

	public void PlayPartical(){
		if(_particleObj == null) return;
		_particleObj.Play();
	}
	//トレイルパーティクルの停止
	public void StopPartical(){
		if(_particleObj == null) return;
		_particleObj.Stop();
	}
}
