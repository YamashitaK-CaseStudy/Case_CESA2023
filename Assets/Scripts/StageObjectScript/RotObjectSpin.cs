using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour {

	GameObject _mesh;
	GameObject _collider;

	private float _spinSpeed = 2; // �P�ʉ�]�ɕK�v�Ȏ���[sec]

	protected void StartSettingSpin() {
		_mesh = this.transform.Find("Mesh").gameObject;
		_collider = this.transform.Find("Collider").gameObject;
		
	}

	public void StartSpin() {
		if ( _isSpin || _isRotating ) {
			return;
		}

		// �t���O�𗧂Ă�
		_isSpin = true;

		//----------------------------
		// �����R���W�����̍��W��ݒ�
		//----------------------------
		//!{
		// ���g�̉�]����180���܂킵�����W�������R���W�����̍��W

		// ��]�ړ��p�̃N�H�[�^�j�I��
		var rotQuat = Quaternion.AngleAxis(180,_selfRotAxis);

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

		//!}
	}

	protected void UpdateSpin() {
		if ( _isSpin ) {

			// ���݃t���[���̉�]��������]�̃N�H�[�^�j�I���쐬
			var rotQuat = Quaternion.AngleAxis(155, _selfRotAxis);

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

		}
	}
   
}
