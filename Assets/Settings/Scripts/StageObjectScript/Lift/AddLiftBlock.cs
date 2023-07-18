using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddLiftBlock : MonoBehaviour
{
    [SerializeField] private int _addRightBlockNum; 
    [SerializeField] private int _addLeftBlockNum; 
    [SerializeField] private int _addUpBlockNum; 
    [SerializeField] private int _addDownBlockNum;

    // ���t�g�R���|�[�l���g
    private Lift _lift;
   
    public void ApplyInspector() {

        // �e���烊�t�g���擾����
        _lift = InitGetLiftComponent();

        var pos = this.transform.localPosition;

        // �E
        for (int i = 0; i < _addRightBlockNum; i++) {

            // �������������[�J���ʒu
            var calPos = new Vector3(pos.x + i + 1, pos.y, 0);

            // �����\�Ȃ̂����f
            RideObjPosSameToSet(calPos, _lift);
        }

        // ��
        for (int i = 0; i < _addLeftBlockNum; i++) {

            // �������������[�J���ʒu
            var calPos = new Vector3(pos.x - i - 1, pos.y, 0);

            // �����\�Ȃ̂����f
            RideObjPosSameToSet(calPos, _lift);
        }

        // ��
        for (int i = 0; i < _addUpBlockNum; i++) {

            // �������������[�J���ʒu
            var calPos = new Vector3(pos.x, pos.y + i + 1, 0);

            // �����\�Ȃ̂����f
            RideObjPosSameToSet(calPos, _lift);
        }

        // ��
        for (int i = 0; i < _addDownBlockNum; i++) {

            // �������������[�J���ʒu
            var calPos = new Vector3(pos.x, pos.y - i - 1, 0);

            // �����\�Ȃ̂����f
            RideObjPosSameToSet(calPos, _lift);
        }
    }

    private Lift InitGetLiftComponent() {

         _lift = this.transform.root.GetComponent<Lift>();

        if(_lift == null) {
            return _lift;
            Debug.Log("���t�g�̃C���X�^���X������܂���");
        }
        else {
            return _lift;
        }
    }



    public void BlockDesroy() {

        DestroyImmediate(this.gameObject);
    }




    private void RideObjPosSameToSet(in Vector3 fuePos, Lift lift) {

        bool isCreate = true;

        for(int i = 0;i < lift.GetRideBlocks.transform.childCount; i++) {
            var pos = lift.GetRideBlocks.transform.GetChild(i).localPosition;
            if(fuePos == pos) {
                isCreate = false;
                return;
            }
        }

        if (isCreate) {
            var r_rideObj = Instantiate(lift.GetCopyRideBlock, lift.GetRideBlocks.transform);
            r_rideObj.transform.localPosition = fuePos;
        }
    }

}
