using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour{

	// �I�t�Z�b�g
	[SerializeField] public Vector3 cameraOffset = new Vector3(0.0f,0.0f,0.0f);
	[SerializeField] public Vector3 targetOffset = new Vector3(0.0f,0.0f,0.0f);
	[SerializeField] public string targetName = "Player";

	private Transform target;

	// Start is called before the first frame update
	void Start(){
		target = GameObject.Find(targetName).transform;

		SystemSoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);
	}


    // Update is called once per frame
    void LateUpdate(){
		// 注視点を計算
		
		// 注視点を正面から見るように調整
		this.transform.position = target.transform.position + cameraOffset + targetOffset;

    }

}
