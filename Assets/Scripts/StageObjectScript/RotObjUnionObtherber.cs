using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotObjUnionObtherber : MonoBehaviour{

	// ��]�I�u�W�F�N�g�̍��̑f��
	private List<GameObject> _unionMaterials = new List<GameObject>();
	private List<GameObject> _unionChildObj = new List<GameObject>();
	public bool _isUseUnion;
	GameObject _parentObj = null;
	Player _playerObj = null;
	HitStopController _ctrl;
	[SerializeField] private RotObjObserver _rotObserver = null;

	void Start(){
		_playerObj = GameObject.FindWithTag("Player").GetComponent<Player>();
	}

	void FixedUpdate() {
		// if(_ctrl == null){
		//     _ctrl = this.gameObject.GetComponent<HitStopController>();
		// }else{
		//     if(_ctrl._isHitStop) return;
		// }
		// if(!_isUseUnion) return;

		// if (_unionMaterials.Count < 2) {
		//     return;
		// }

		// // ���̃I�u�W�F�N�g�̑Ώۂł͖����I�u�W�F�N�g����菜��
		// for (int i = _unionMaterials.Count - 1; i >= 0; i--) {
		//     if (_unionMaterials[i].GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) {
		//         _unionMaterials.RemoveAt(i);
		//     }
		// }

		// if (_unionMaterials.Count < 2) {
		//     return;
		// }

		// // ��]�I�u�W�F�N�g���~�܂��Ă邩�m�F
		// foreach (var rotobj in _unionMaterials) {

		//     if (rotobj.GetComponent<RotatableObject>()._isRotating) {
		//                 Debug.Log("��]���Ă���...");
		//         return;
		//     }
		// }

		// // ���̌��̉�]�I�u�W�F�N�g�̎q��(Objects)���擾����
		// var basechildObjects = _unionMaterials[0].transform.GetChild(0).gameObject;

		// // ��]�I�u�W�F�N�g�ɂԂ牺�����Ă�Object�I�u�W�F�N�g���擾����
		// List<GameObject> objects = new List<GameObject>();
		// for (int i = 1; i < _unionMaterials.Count; i++) {

		//     for(int j = 0; j < _unionMaterials[i].transform.childCount; j++) {

		//         for (int k = 0; k < _unionMaterials[i].transform.GetChild(j).childCount; k++) {
		//             objects.Add(_unionMaterials[i].transform.GetChild(j).GetChild(k).gameObject);
		//         }
		//     }

		//     // ��ɂȂ�����]�I�u�W�F�N�g���폜����
		//     Destroy(_unionMaterials[i].gameObject);
		// }

		// // �e��ύX
		// foreach(var obj in objects) {
		//    obj.transform.parent = basechildObjects.transform;
		// }

		// // ���̂����u�Ԃ�
		// Debug.Log("���̂���");
		// var rotObj = basechildObjects.transform.root.GetComponent<RotatableObject>();
		// rotObj.ChildCountUpdate();  // �q���̍X�V
		// rotObj.FinishUnion();       // ���̂��I�������̂�ʒm����
		// _rotObserver.SetLastRotateRotObj(rotObj);   // �I�u�U�[�o�[�ɍŌ�ɉ�]�����I�u�W�F�N�g�Ƃ��ēo�^

		// _unionMaterials.Clear();
	}

	// ���̑f�ނ̒ǉ�
	public void AddunionMaterial(GameObject material) {
		if(!_unionMaterials.Contains(material)){
			_unionMaterials.Add(material);
			for(int i = 0; i < _unionMaterials.Count; i++){
				Debug.Log("����������ǉ��Z���");
				Debug.Log(_unionMaterials[i].name);
			}
			//ObjectUnion();
			Debug.Log("���Ƃ����");
			Debug.Log(_unionMaterials.Count);
		}
	}

	// ���̑f�ނ̒ǉ�
	public void AddUnionChildObj(GameObject material){
		if(!_unionChildObj.Contains(material)){
			_unionChildObj.Add(material);
		}
	}

	// ���̑f�ނ̐e��ݒ肷��
	public void SetUnionParentObj(GameObject obj){
		if(obj.GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) return;
		_parentObj = obj;
	}

	// ���̏���
	// ��]���I�������^�C�~���O�ŌĂяo��
	public void Union(){
		// ���̂�����ׂ��I�u�W�F�N�g���Ȃ��ꍇ�������s��Ȃ�
		if(_unionChildObj.Count <= 0 && _parentObj == null) return;

		// �O�̂��߂Ɏ�ʂ��m�F���ΏۈȊO�����m���ꂽ�ꍇ���̃I�u�W�F�N�g����菜��
		for (int i = _unionChildObj.Count - 1; i >= 0; i--) {
			if (_unionChildObj[i].GetComponent<RotObjkinds>()._RotObjKind != RotObjkinds.ObjectKind.UnionRotObject) {
				_unionChildObj.RemoveAt(i);
			}
		}

		// ���̂�����ׂ��I�u�W�F�N�g���Ȃ��ꍇ�������s��Ȃ�
		if(_unionChildObj.Count <= 0 && _parentObj == null) return;

		// ��]�I�u�W�F�N�g�ɂԂ牺�����Ă�Object�I�u�W�F�N�g���擾����
		List<GameObject> objects = new List<GameObject>();
		for (int i = 0; i < _unionChildObj.Count; i++) {
			if(_parentObj.gameObject == _unionChildObj[i]) continue;
			for(int j = 0; j < _unionChildObj[i].transform.childCount; j++) {
				for(int k = 0; k < _unionChildObj[i].transform.GetChild(j).transform.childCount; k++){
					objects.Add(_unionChildObj[i].transform.GetChild(j).transform.GetChild(k).gameObject);
				}
			}
			// ��ɂȂ�����]�I�u�W�F�N�g���폜����
			Destroy(_unionChildObj[i].gameObject);
		}

		// �e��ύX
		foreach(var obj in objects) {
			obj.transform.parent = _parentObj.transform.GetChild(0).transform;
		}

		// ���̂����u�Ԃ�
		Debug.Log("���̂���");
		var rotObj = _parentObj.transform.root.GetComponent<RotatableObject>();
		rotObj.ChildCountUpdate();	// �q���̍X�V
		rotObj.FinishUnion();		// ���̂��I�������̂�ʒm����
		rotObj.StartUpUnionFX();
		var playerAxisParts = _playerObj.GetAxisParts();
		foreach(var obj in objects){
			if(obj == playerAxisParts){
				_playerObj.BlockLoad(obj);
				break;
			}
		}
		DeleteData();
		_rotObserver.SetLastRotateRotObj(rotObj);   // �I�u�U�[�o�[�ɍŌ�ɉ�]�����I�u�W�F�N�g�Ƃ��ēo�^
		return;
	}


    public void DeleteData(){
        // �����̍Ō�Ɏq���ɂ���C�u�W�F�N�g�̃f�[�^���폜
		_unionChildObj.Clear();
		_parentObj = null;
    }
}