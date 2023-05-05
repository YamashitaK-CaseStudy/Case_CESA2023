using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour {

	[SerializeField] int _rotSpeed = 15;

	/*
	GameObject _object;
	GameObject[] _cloneObj;
	GameObject _toSpinCloneObj;
	 */

	GameObject[] _cloneBaceObjs;	// Clone���̃I�u�W�F�N�g���i�[���Ă����z��
	GameObject[,] _cloneObjs;		// Clone�����I�u�W�F�N�g�̔z��
	GameObject[] _toSpinCloneObjs;  // �񂷗p�̃N���[��

	int _cloneNum = 0;

	protected void StartSettingSpin() {
		//_object = this.transform.Find("Object").gameObject;
	}

	public void StartSpin() {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// �t���O�𗧂Ă�
		_isSpin = true;

	}

	public void StartSpin(Vector3 rotCenter, Vector3 rotAxis) {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// RotableObj�̎q�I�u�W�F�N�g�ł���Object = Clone�������ׂĎ擾
		var childNum = this.transform.childCount;
		_cloneBaceObjs = new GameObject[childNum];
		int itr = 0;
		foreach ( Transform childTransform in this.transform ) {
			_cloneBaceObjs[itr++] = childTransform.gameObject;
		}
		Debug.Log(itr);

		// ��]�̒��S��ݒ�
		_axisCenterWorldPos = rotCenter;

		// ��]����ݒ�
		_rotAxis = rotAxis;

		// �t���O�𗧂Ă�
		_isSpin = true;


		_cloneNum = _cloneBaceObjs.Length;

		_cloneObjs = new GameObject[_cloneNum,3];

		Debug.Log(_cloneBaceObjs.Length);

		/*
		_cloneObj = new GameObject[3];
		for (int i = 0; i < 3; i++){
			_cloneObj[i] = Instantiate(_object) as GameObject;
			_cloneObj[i].transform.parent = _object.transform.parent;
			_cloneObj[i].transform.localPosition = _object.transform.localPosition;
			_cloneObj[i].transform.localScale = _object.transform.localScale;
			_cloneObj[i].transform.rotation = _object.transform.rotation;

			// ��
			// ��]�ړ��p�̃N�H�[�^�j�I��
			var rotQuat = Quaternion.AngleAxis(90 * (i + 1), _rotAxis);

			// �~�^���̈ʒu�v�Z
			var tr = _cloneObj[i].transform;
			var pos = tr.position;

			// �N�H�[�^�j�I����p������]�͌��_����̃I�t�Z�b�g��p����K�v������
			// _axisCenterWorldPos��C�ӎ��̍��W�ɕύX����ΔC�ӎ��̉�]���ł���
			pos -= _axisCenterWorldPos;
			pos = rotQuat * pos;
			pos += _axisCenterWorldPos;

			tr.position = pos;

			// �����X�V
			tr.rotation = rotQuat * tr.rotation;

		}
		 */

		// �I�u�W�F�N�g�𕡐�����
		// 3D�̈ʒu�֌W�I��90���E180���E270����3�̕������K�v
		for ( int i = 0 ; i < _cloneNum ; i++ ) {

			for ( int j = 0 ; j < 3 ; j++ ) {
				_cloneObjs[i,j] = Instantiate(_cloneBaceObjs[i]) as GameObject;
				_cloneObjs[i,j].transform.parent			= _cloneBaceObjs[i].transform.parent;
				_cloneObjs[i,j].transform.localPosition	= _cloneBaceObjs[i].transform.localPosition;
				_cloneObjs[i,j].transform.localScale		= _cloneBaceObjs[i].transform.localScale;
				_cloneObjs[i,j].transform.rotation			= _cloneBaceObjs[i].transform.rotation;

				// ��
				// ��]�ړ��p�̃N�H�[�^�j�I��
				var rotQuat = Quaternion.AngleAxis(90 * (j + 1), _rotAxis);

				// �~�^���̈ʒu�v�Z
				var tr = _cloneObjs[i,j].transform;
				var pos = tr.position;

				// �N�H�[�^�j�I����p������]�͌��_����̃I�t�Z�b�g��p����K�v������
				// _axisCenterWorldPos��C�ӎ��̍��W�ɕύX����ΔC�ӎ��̉�]���ł���
				pos -= _axisCenterWorldPos;
				pos = rotQuat * pos;
				pos += _axisCenterWorldPos;

				tr.position = pos;

				// �����X�V
				tr.rotation = rotQuat * tr.rotation;
			}
		}

		_toSpinCloneObjs = new GameObject[_cloneNum];

		for ( int i = 0 ; i < _cloneNum ; i++ ) {
			// ��]����p�̃R�s�[�𐶐�
			_toSpinCloneObjs[i] = Instantiate(_cloneBaceObjs[i]) as GameObject;
			_toSpinCloneObjs[i].transform.parent =			_cloneBaceObjs[i].transform.parent;
			_toSpinCloneObjs[i].transform.localPosition =	_cloneBaceObjs[i].transform.localPosition;
			_toSpinCloneObjs[i].transform.localScale =		_cloneBaceObjs[i].transform.localScale;
			_toSpinCloneObjs[i].transform.rotation =		_cloneBaceObjs[i].transform.rotation;

			// �R���W�����𖳌��ɂ���
			var cloneParentTransform = _toSpinCloneObjs[i].transform;

			// �q�I�u�W�F�N�g�P�ʂ̏���
			// Clone�̎q�I�u�W�F�N�g��Pf_Parts����������
			// Pf_Part�ɂ��Ă���BoxCollider��Pf_Part�̎q�I�u�W�F�N�g�̓���ChainCollider�𖳌�������K�v������
			foreach ( Transform childTransform in cloneParentTransform ) {

				// �{�b�N�X�R���C�_�[�𖳌�������
				var childBoxCollider = childTransform.gameObject.GetComponent<BoxCollider>();
				childBoxCollider.enabled = false;

				// �`�F�C���R���C�_�[�̃{�b�N�X�R���C�_�[�𖳌�������
				var childChineCollider = childTransform.Find("ChainCollider");
				var childChineColliderBoxCollider = childChineCollider.gameObject.GetComponent<BoxCollider>();
				childChineColliderBoxCollider.enabled = false;

			}
		}

		/*
		// ��]����p�̃R�s�[�𐶐�
		_toSpinCloneObj = Instantiate(_object) as GameObject;
		_toSpinCloneObj.transform.parent = _object.transform.parent;
		_toSpinCloneObj.transform.localPosition = _object.transform.localPosition;
		_toSpinCloneObj.transform.localScale = _object.transform.localScale;
		_toSpinCloneObj.transform.rotation = _object.transform.rotation;

		// �R���W�����𖳌��ɂ���
		var parentTransform = _toSpinCloneObj.transform;

		// �q�I�u�W�F�N�g�P�ʂ̏���
		// Clone�̎q�I�u�W�F�N�g��Pf_Parts����������
		// Pf_Part�ɂ��Ă���BoxCollider��Pf_Part�̎q�I�u�W�F�N�g�̓���ChainCollider�𖳌�������K�v������
		foreach (Transform childTransform in parentTransform){

			// �{�b�N�X�R���C�_�[�𖳌�������
			var childBoxCollider = childTransform.gameObject.GetComponent<BoxCollider>();
			childBoxCollider.enabled = false;

			// �`�F�C���R���C�_�[�̃{�b�N�X�R���C�_�[�𖳌�������
			var childChineCollider = childTransform.Find("ChainCollider");
			var childChineColliderBoxCollider = childChineCollider.gameObject.GetComponent<BoxCollider>();
			childChineColliderBoxCollider.enabled = false;
		}
		 */
	}

	protected void UpdateSpin() {
		if ( _isSpin ) {
			// ���݃t���[���̉�]��������]�̃N�H�[�^�j�I���쐬
			var rotQuat = Quaternion.AngleAxis(_rotSpeed, _rotAxis);

			Debug.Log(_cloneNum);
			for ( int i = 0 ; i < _cloneNum ; i++ ) {
				// �~�^���̈ʒu�v�Z
				var tr = _toSpinCloneObjs[i].transform;
				var pos = tr.position;

				// �N�H�[�^�j�I����p������]�͌��_����̃I�t�Z�b�g��p����K�v������
				// _axisCenterWorldPos��C�ӎ��̍��W�ɕύX����ΔC�ӎ��̉�]���ł���
				pos -= _axisCenterWorldPos;
				pos = rotQuat * pos;
				pos += _axisCenterWorldPos;

				tr.position = pos;

				// �����X�V
				tr.rotation = rotQuat * tr.rotation;

			}
		}
	}
}
