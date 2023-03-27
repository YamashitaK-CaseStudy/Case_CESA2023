using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour {

	GameObject _mesh;
	GameObject _collider;
	GameObject _object;

	protected void StartSettingSpin() {
		_object = this.transform.Find("Object").gameObject;
		


	}

	public void StartSpin() {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// �t���O�𗧂Ă�
		_isSpin = true;

		// �I�u�W�F�N�g�𕡐�����
		var cloneObj = Instantiate(_object) as GameObject;
		cloneObj.transform.parent = _object.transform.parent;
		cloneObj.transform.localPosition = _object.transform.localPosition;
		cloneObj.transform.localScale = _object.transform.localScale;

		var cloneMesh = cloneObj.transform.Find("Mesh");

		var color = cloneMesh.GetComponent<MeshRenderer>().material.color;

		color.a = 0.5f;

		cloneMesh.GetComponent<MeshRenderer>().material.color = color;

		// ��
		// ���g�̉�]����180���܂킵�����W�������R���W�����̍��W

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
		tr.rotation = tr.rotation * rotQuat;




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

		// �I�u�W�F�N�g�𕡐�����
		var cloneObj = Instantiate(_object) as GameObject;
		cloneObj.transform.parent = _object.transform.parent;
		cloneObj.transform.localPosition = _object.transform.localPosition;
		cloneObj.transform.localScale = _object.transform.localScale;


		// ��
		// ���g�̉�]����180���܂킵�����W�������R���W�����̍��W

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
		tr.rotation = tr.rotation * rotQuat;


	}

	protected void UpdateSpin() {
		if ( _isSpin ) {
			/*
			// ���݃t���[���̉�]��������]�̃N�H�[�^�j�I���쐬
			var rotQuat = Quaternion.AngleAxis(155, _rotAxis);

			// �~�^���̈ʒu�v�Z
			var tr = _mesh.transform;
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

		}
	}
   
}
