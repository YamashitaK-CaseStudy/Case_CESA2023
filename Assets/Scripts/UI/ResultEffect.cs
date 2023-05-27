using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultEffect : MonoBehaviour
{
    GameObject _rank;
    GameObject _seed;
    GameObject _menu;
    GameObject _textClear;
    GameObject _textFailed;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < this.transform.childCount; i++){
            var obj = this.transform.GetChild(i).gameObject;
            switch(obj.name){
                case "Rank":
                _rank = obj;
                break;
                case "Seed":
                _seed = obj;
                break;
                case "Menu":
                _menu = obj;
                break;
                case "TextClear":
                _textClear = obj;
                break;
                case "TextFailed":
                _textFailed = obj;
                break;
                default:
                Debug.LogError("なんそれ");
                break;
            }
        }

        _rank.SetActive(false);
        _seed.SetActive(false);
        _menu.SetActive(false);
        _textClear.SetActive(false);
        _textFailed.SetActive(false);
    }

    void ActiveEvaluation(){
        _rank.SetActive(true);
        _seed.SetActive(true);
    }

    void ActiveClear(){
        _textClear.SetActive(true);
    }

    void ActiveFailed(){
        _textFailed.SetActive(true);
        ActiveMenu();
    }

    void ActiveMenu(){
        _menu.SetActive(true);
    }

    public void FinishAnimation(bool clearFlg){
        if(clearFlg){
            SystemSoundManager.Instance.PlayBGM(BGMSoundData.BGM.Result);
            ActiveClear();
            ActiveEvaluation();
        }else{
            ActiveFailed();
        }
    }

    public void FinishEvaluation(){
        ActiveMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
