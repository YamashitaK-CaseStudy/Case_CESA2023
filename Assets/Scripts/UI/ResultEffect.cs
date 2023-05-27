using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultEffect : MonoBehaviour
{
    GameObject _rank;
    GameObject _seed;
    GameObject _menu;
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
                default:
                Debug.LogError("なんそれ");
                break;
            }
        }

        _rank.SetActive(false);
        _seed.SetActive(false);
        _menu.SetActive(false);
    }

    void ActiveEvaluation(){
        _rank.SetActive(true);
        _seed.SetActive(true);
    }

    void ActiveMenu(){
        _menu.SetActive(true);
    }

    public void FinishAnimation(){
        ActiveEvaluation();
        SystemSoundManager.Instance.PlayBGM(BGMSoundData.BGM.Result);
    }

    public void FinishEvaluation(){
        ActiveMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
