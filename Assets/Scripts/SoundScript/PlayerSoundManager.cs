using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource _gameSeAudioSource;

    [SerializeField] List<PlayerSESoundData> _gameSeSoundDatas;

    public float _masterVolume = 1;
    public float _seMasterVolume = 1;

    public static PlayerSoundManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayPlayerSE(PlayerSESoundData.PlayerSE se)
    {
        PlayerSESoundData data = _gameSeSoundDatas.Find(data => data._playerSe == se);
        _gameSeAudioSource.volume = data._playerSeVolume * _seMasterVolume * _masterVolume;
        _gameSeAudioSource.PlayOneShot(data._playerSeAudioClip);
    }
}

[System.Serializable]
public class PlayerSESoundData
{
    // ‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
    public enum PlayerSE
    {
        Move,
        Lock,
        Jump,
        Rotation
    }

    public PlayerSE _playerSe;
    public AudioClip _playerSeAudioClip;
    [Range(0, 1)]
    public float _playerSeVolume = 1;
}