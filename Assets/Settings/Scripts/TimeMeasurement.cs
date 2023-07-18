using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMeasurement : MonoBehaviour{

    //  �A���[�����X�g
    public Dictionary<string, Alarm> _alarmList = new Dictionary<string, Alarm>();

    private void FixedUpdate() {
        
        // �A���[���̍X�V
        foreach(var alarm in _alarmList) {
            alarm.Value.Update();
        }
    }

    // �A���[���̒ǉ�
    public Alarm AddArarm(in string ararmName, float settingTime) {

        _alarmList.Add(ararmName, new Alarm(settingTime));
        return _alarmList[ararmName];
    }


    // �ݒ肵�����Ԃ��߂���Ƌ����Ă����A���[���N���X
    public class Alarm {

        private bool _isTimeStart = false; 
        private bool _isTimeEnd = false; 
        private float _currentTime = 0.0f;
        private float _settingTime;

        public Alarm(float settingTime) {

            _settingTime = settingTime;
        }

        public void Update(){

            if (_isTimeStart) {

                if(_settingTime >= _currentTime) {
                    _currentTime += Time.deltaTime;
                }
                else {
                    _isTimeEnd = true;
                }
            }
            else {
                if (_settingTime <= _currentTime) {
                    _isTimeEnd = true;
                }
            }
        }

        public bool TimeStart {
            get { return _isTimeStart; }
            set { _isTimeStart = value; }
        }

        public bool TimeEnd {
            get { return _isTimeEnd; }
        }

        public float CurrentTime {
            get { return _currentTime; }
        }

        public void ResetTime(float setcurrent = 0.0f) {
            _currentTime = setcurrent;
            _isTimeEnd = false;
        }
    }
}
