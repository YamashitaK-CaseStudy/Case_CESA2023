using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour {

	[SerializeField] int _rotSpeed = 15;

	GameObject _object;
	GameObject[] _cloneObj;
	GameObject _toSpinCloneObj;

	protected void StartSettingSpin() {
		_object = this.transform.Find("Object").gameObject;
	}

	public void StartSpin() {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// �t���O�𗧂Ă�
		_isSpin = true;

		/*
		// �I�u�W�F�N�g�𕡐�����
		var cloneObj = Instantiate(_object) as GameObject;
		cloneObj.transform.parent = _object.transform.parent;
		cloneObj.transform.localPosition = _object.transform.localPosition;
		cloneObj.transform.localScale = _object.transform.localScale;
		cloneObj.transform.rotation = _object.transform.rotation;

		 */
		// ��
		// ���g�̉�]����180���܂킵�����W�������R���W�����̍��W

		/*
		// ��]�ړ��p�̃N�H�[�^�j�I��
		var rotQuat = Quaternion.AngleAxis(180,_rotAxis);

		// �~�^���̈ʒu�v�Z
		var tr = cloneObj.transform;
		var pos = tr.position;

		// �N�H�[�^�j�I����p������]�͌��_����̃I�t�Z�b�g��p����K�v������
		// _axisCenterWorldPos��C�ӎ��̍��W�ɕύX����ΔC�ӎ��̉�]���ł���
		pos -= _axisCenterWorldPos;
		pos = rotQuat * pos;
		pos += _axisCenterWorldPos;

		tr.position = pos;

		// �����X�V
		//tr.rotation =  rotQuat * tr.rotation;

		 */



		/*
		//----------------------------
		// �����R���W�����̍��W��ݒ�
		//----------------------------
		//!{
		// ���g�̉�]����180���܂킵�����W�������R���W�����̍��W

		// ��]�ړ��p�̃N�H�[�^�j�I��
		var rotQuat = Quaternion.AngleAxis(180,_rotAxis);

		// �~�^���̈ʒu�v�Z
		var pretence = _collider.transform.Find("PretenceCollison").gameObject;
		var tr = pretence.transform;
		var pos = tr.position;

		// �N�H�[�^�j�I����p������]�͌��_����̃I�t�Z�b�g��p����K�v������
		// _axisCenterWorldPos��C�ӎ��̍��W�ɕύX����ΔC�ӎ��̉�]���ł���
		pos -= _axisCenterWorldPos;
		pos = rotQuat * pos;
		pos += _axisCenterWorldPos;

		tr.position = pos;

		// �����X�V
		tr.rotation = tr.rotation * rotQuat;

		 */
		//!}
	}

	public void StartSpin(Vector3 rotCenter, Vector3 rotAxis) {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// ��]�̒��S��ݒ�
		_axisCenterWorldPos = rotCenter;

		// ��]����ݒ�
		_rotAxis = rotAxis;

		// �t���O�𗧂Ă�
		_isSpin = true;

		_cloneObj = new GameObject[3];

		// �I�u�W�F�N�g�𕡐�����
		// 3D�̈ʒu�֌W�I��90���E180���E270����3�̕������K�v
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

			// �����_���[�𖳌��ɂ���
			var childMesh = childTransform.Find("P_RotateObject");
			var childMeshRenderer = childMesh.GetComponent<MeshRenderer>();
			childMeshRenderer.enabled = false;

		}



	}



	protected void UpdateSpin() {
		if ( _isSpin ) {
			
			// ���݃t���[���̉�]��������]�̃N�H�[�^�j�I���쐬
			var rotQuat = Quaternion.AngleAxis(_rotSpeed, _rotAxis);

			// �~�^���̈ʒu�v�Z
			var tr = _toSpinCloneObj.transform;
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
