using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource[] sfxSource;
    [SerializeField] private AudioSource[] bgmSource;

    public bool playBgm;
    private int bgmIndex;

    private bool allowVolume;//允许播放声音,防止在加载时播放声音
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Invoke("AllowVolume", 1f);
    }
    private void Update()
    {
        if (!allowVolume)
        {
            return;
        }
        if (!playBgm)
        {
            StopAllBGM();
        }
        else
        {
            if (!bgmSource[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }
    }

    public void PlaySFX(int _sfxIndex)
    {
        if (_sfxIndex < sfxSource.Length)
        {
            sfxSource[_sfxIndex].pitch = Random.Range(.85f, 1.15f);
            sfxSource[_sfxIndex].Play();
        }
    }
    public void StopSFX(int _sfxIndex)
    {
        if (_sfxIndex < sfxSource.Length)
        {
            sfxSource[_sfxIndex].Stop();
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        if (_bgmIndex < bgmSource.Length)
        {
            bgmIndex = _bgmIndex;
            StopAllBGM();
            bgmSource[bgmIndex].Play();
        }
    }

    public void PlayerRandomBGM()
    {
        bgmIndex = Random.Range(0, bgmSource.Length);
        PlayBGM(bgmIndex);
    }
    public void StopAllBGM()
    {
        foreach (var bgm in bgmSource)
        {
            bgm.Stop();
        }
    }

    public void AllowVolume()
    {
        allowVolume = true;
    }
}
