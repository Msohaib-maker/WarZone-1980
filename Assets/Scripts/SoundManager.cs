using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource FireShotsSFX;
    public AudioSource GameSFX;

    [Header("Volume Settings")]
    [Range(0, 1)] public float musicVolume = 0.5f;
    [Range(0, 1)] public float sfxVolume = 0.5f;

    public void UpdateGameMusic(float volume)
    {
        musicVolume = volume;
        GameSFX.volume = volume;
    }

    public void UpdateFireShotSFX(float volume)
    {
        sfxVolume = volume;
        FireShotsSFX.volume = volume;
    }
}
