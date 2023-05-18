using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SystemSoundManager : MonoBehaviour {
    [SerializeField] AudioSource _bgmAudioSource;
    [SerializeField] AudioSource _seAudioSource;

    [SerializeField] List<BGMSoundData> _bgmSoundDatas;
    [SerializeField] List<SystemSESoundData> _seSoundDatas;

    public float _masterVolume = 1;
    public float _bgmMasterVolume = 1;
    public float _seMasterVolume = 1;

    public static SystemSoundManager Instance {
        get; private set;
    }

    private void Awake() {
        if ( Instance == null ) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(BGMSoundData.BGM bgm) {
        BGMSoundData data = _bgmSoundDatas.Find(data => data._bgm == bgm);
        _bgmAudioSource.clip = data._audioClip;
        _bgmAudioSource.volume = data._volume * _bgmMasterVolume * _masterVolume;
        _bgmAudioSource.Play();
    }


    public void PlaySE(SystemSESoundData.SystemSE se) {
        SystemSESoundData data = _seSoundDatas.Find(data => data._systemSe == se);
        _seAudioSource.volume = data._systemSeVolume * _seMasterVolume * _masterVolume;
        _seAudioSource.PlayOneShot(data._systemSeAudioClip);
    }
}

[System.Serializable]
public class BGMSoundData {
         
    // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
    public enum BGM {
        Title,
        StageSelect,
        MainGame,
        Result,
    }

    public BGM _bgm;
    public AudioClip _audioClip;
    [Range(0, 1)]
    public float _volume = 1;
}

[System.Serializable]
public class SystemSESoundData {
    // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
    public enum SystemSE {
        Enter,
        Cancel,
        Select,
        Error,
    }

    public SystemSE _systemSe;
    public AudioClip _systemSeAudioClip;
    [Range(0, 1)]
    public float _systemSeVolume = 1;
}