using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.7f;

    public enum BGMType
    {
        Normal,
        HighLevel,
        Creepy,
        Ending
    }

    public enum SFXType
    {
        GetIngredient,
        CoffeeShot,
        CustomerAngry,
        CustomerHappy
    }
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM Clips")]
    public AudioClip normalBGM;
    public AudioClip highLevelBGM;
    public AudioClip creepyBGM;
    public AudioClip endingBGM;

    [Header("SFX Clips")]
    public AudioClip getIngredientSFX;
    public AudioClip coffeeShotSFX;
    public AudioClip customerAngrySFX;
    public AudioClip customerHappySFX;

    Dictionary<BGMType, AudioClip> bgmMap;
    Dictionary<SFXType, AudioClip> sfxMap;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;


        bgmMap = new Dictionary<BGMType, AudioClip>
        {
            { BGMType.Normal, normalBGM },
            { BGMType.HighLevel, highLevelBGM },
            { BGMType.Creepy, creepyBGM },
            { BGMType.Ending, endingBGM }
        };

        sfxMap = new Dictionary<SFXType, AudioClip>
        {
            { SFXType.GetIngredient, getIngredientSFX },
            { SFXType.CoffeeShot, coffeeShotSFX },
            { SFXType.CustomerAngry, customerAngrySFX },
            { SFXType.CustomerHappy, customerHappySFX }
        };
    }

    // =========================
    // BGM
    // =========================
    public void PlayBGM(BGMType type)
    {
        if (!bgmMap.ContainsKey(type)) return;

        AudioClip clip = bgmMap[type];
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // =========================
    // SFX
    // =========================
    public void PlaySFX(SFXType type)
    {
        if (!sfxMap.ContainsKey(type)) return;

        sfxSource.PlayOneShot(sfxMap[type]);
    }
    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        sfxSource.volume = sfxVolume;
    }
}
