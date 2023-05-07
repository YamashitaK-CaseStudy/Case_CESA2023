using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RotatableObject : MonoBehaviour
{

	[SerializeField] int _rotSpeed = 15;

	private GameObject[] _cloneBaceObjs;        // Clone���̃I�u�W�F�N�g�̔z��
	private GameObject[,] _cloneObjs;           // Clone�����I�u�W�F�N�g�̔z��
	private GameObject[] _toSpinCloneObjs;  // �񂷗p�̃N���[���̔z��

	private List<GameObject> _originObjList;    // �N���[�����̃I�u�W�F�N�g���X�g

	private int _originNum = 0;
	private int _listLength = 0;

	public void StartFunc(){
		_originObjList = new List<GameObject>();
	}

	public void StartSpin()
	{
		if (_isSpining || _isRotating)
		{
			return;
		}

		// �t���O�𗧂Ă�
		_isSpining = true;

	}

	public void StartSpin(Vector3 rotCenter, Vector3 rotAxis)
	{
		if (_isSpining || _isRotating)
		{
			return;
		}

		AddCloneOriginToList();

		// ��]�̒��S��ݒ�
		_axisCenterWorldPos = rotCenter;

		// ��]����ݒ�
		_rotAxis = rotAxis;

		// �t���O�𗧂Ă�
		_isSpining = true;

		// �N���[���p��GameObject���m��
		// �N���[��1�ɑ΂���,90���E180���E270���p��3�K�v
		_cloneObjs = new GameObject[_originNum, 3];

		//Debug.Log(_cloneBaceObjs.Length);

		CreateClone();

		CreateCloneToSpin();
	}


	// ������]���I��
	public void EndSpin()
	{
		if (_isSpining == false || _isRotating == true)
		{
			return;
		}

		_isSpining = false;


		foreach (GameObject cloneObj in _cloneObjs)
		{
			Destroy(cloneObj);
		}

		foreach (GameObject toSpinCloneObj in _toSpinCloneObjs)
		{
			Destroy(toSpinCloneObj);
		}

	}

	// ������]�̍X�V����
	protected void UpdateSpin()
	{
		if (_isSpining)
		{

			// ���݃t���[���̉�]��������]�̃N�H�[�^�j�I���쐬
			var rotQuat = Quaternion.AngleAxis(_rotSpeed, _rotAxis);

			Debug.Log(_originNum);
			for (int i = 0; i < _originNum; i++)
			{
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


	// Clone�������ׂĎ擾���Ĕz��Ɋi�[���鏈��
	// ���łɒǉ��ς݂̏ꍇ�̓X�L�b�v
	// MEMO�F���΃M�~�b�N�̌��ˍ����ő�(��)�ɑΉ�
	protected void AddCloneOriginToList(){

		_originNum = this.transform.childCount; // ���ۂ̃N���[�����̐�
		_listLength = _originObjList.Count;  // �����_�̃N���[�������i�[���Ă���z��̒������擾���Ƃ�

		Debug.Log(_listLength);

		// �N���[���̐��ɕύX���Ȃ���Ώ������X�L�b�v
		if (_listLength == this.transform.childCount)
		{
			return;
		}
		// �n�߂ČĂ΂ꂽ��
		else if (_listLength == 0)
		{
			foreach (Transform childTransform in this.transform)
			{
				_originObjList.Add(childTransform.gameObject);
			}
		}
		// �N���[���̐��ɕύX���������ꍇ
		// ���̌����ɂ��Ă͑z���N���肦�Ȃ����ߍl�����Ȃ�
		else if (_listLength > 0)
		{
			// �ǉ����鐔���v�Z
			var addObjNum = this.transform.childCount - _listLength;

			for (int itr = _originNum - addObjNum; itr < _originNum; itr++)
			{
				_originObjList.Add(this.transform.GetChild(itr).gameObject);
			}
		}
		else
		{
			Debug.Log("����`�̏�������F�N���[�����̐����O��̍�����]���猸�����Ă��܂�");
		}

		return;

	}

	// �N���[����������
	protected void CreateClone()
	{
		// �N���[���p��GameObject���m��
		// �N���[��1�ɑ΂���,90���E180���E270���p��3�K�v
		_cloneObjs = new GameObject[_originNum, 3];

		// �I�u�W�F�N�g�𕡐�����
		// 3D�̈ʒu�֌W�I��90���E180���E270����3�̕������K�v
		for (int i = 0; i < _originNum; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				

				_cloneObjs[i, j] = Instantiate(_originObjList[i]) as GameObject;
				_cloneObjs[i, j].transform.parent = _originObjList[i].transform.parent;
				_cloneObjs[i, j].transform.localPosition = _originObjList[i].transform.localPosition;
				_cloneObjs[i, j].transform.localScale = _originObjList[i].transform.localScale;
				_cloneObjs[i, j].transform.rotation = _originObjList[i].transform.rotation;

				// ��
				// ��]�ړ��p�̃N�H�[�^�j�I��
				var rotQuat = Quaternion.AngleAxis(90 * (j + 1), _rotAxis);

				// �~�^���̈ʒu�v�Z
				var tr = _cloneObjs[i, j].transform;
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

	// ��]�p�̃N���[���𐶐�
	private void CreateCloneToSpin()
	{
		// �܂킷�p�̃N���[����ǉ��Ő���
		_toSpinCloneObjs = new GameObject[_originNum];

		for (int i = 0; i < _originNum; i++)
		{
			// ��]����p�̃R�s�[�𐶐�
			_toSpinCloneObjs[i] = Instantiate(_originObjList[i]) as GameObject;
			_toSpinCloneObjs[i].transform.parent = _originObjList[i].transform.parent;
			_toSpinCloneObjs[i].transform.localPosition = _originObjList[i].transform.localPosition;
			_toSpinCloneObjs[i].transform.localScale = _originObjList[i].transform.localScale;
			_toSpinCloneObjs[i].transform.rotation = _originObjList[i].transform.rotation;


			// �R���W�����𖳌��ɂ��鏈��
			// Clone�̎q�I�u�W�F�N�g��Pf_Parts����������
			// Pf_Part�ɂ��Ă���BoxCollider��Pf_Part�̎q�I�u�W�F�N�g�̓���ChainCollider�𖳌�������K�v������
			foreach (Transform childTransform in _toSpinCloneObjs[i].transform)
			{

				// �{�b�N�X�R���C�_�[�𖳌�������
				var childBoxCollider = childTransform.gameObject.GetComponent<BoxCollider>();
				childBoxCollider.enabled = false;

				// �`�F�C���R���C�_�[�̃{�b�N�X�R���C�_�[�𖳌�������
				var childChineCollider = childTransform.Find("ChainCollider");
				var childChineColliderBoxCollider = childChineCollider.gameObject.GetComponent<BoxCollider>();
				childChineColliderBoxCollider.enabled = false;

			}
		}
	}
}
