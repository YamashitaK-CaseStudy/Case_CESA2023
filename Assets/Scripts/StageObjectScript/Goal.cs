using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] List<GameObject> _shatter;
    [SerializeField] Cinemachine.CinemachineVirtualCamera _resultVCam;
	bool _isGoalEffect = false;
	float _rotRequirdTime = 0;
	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.layer == LayerMask.NameToLayer("PlayerRotCollider")) return;
		if(other.transform.root.gameObject.tag != "Player") return;

    SuzumuraTomoki.SceneManager.missionClear = true;

		_isGoalEffect = true;
		SuzumuraTomoki.SceneManager.playerInput.Disable();
		_resultVCam.Priority = 11;

		GameSoundManager.Instance.PlayGameSE(GameSESoundData.GameSE.Shutter);
	}

	public void Update(){

		if(_isGoalEffect){
			_rotRequirdTime += Time.deltaTime;

			for(int i = 0; i < _shatter.Count; i++){
				var pos = _shatter[i].transform.position;
				pos.y += Time.deltaTime;
				_shatter[i].transform.position = pos;
			}

			if(_rotRequirdTime > 3){
        SuzumuraTomoki.SceneManager.playerInput.Enable();
				SuzumuraTomoki.SceneManager.LoadResult();
			}
		}
 }

}
