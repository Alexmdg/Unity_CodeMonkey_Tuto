using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public float Volume { get; private set; }
    private AudioSource audiosource;

    private const string PLAYER_PREF_MUSIC_VOLUME = "MusicVolume";

    private void Awake()
    {
        Instance = this;
        audiosource = GetComponent<AudioSource>();
        Volume = PlayerPrefs.GetFloat(PLAYER_PREF_MUSIC_VOLUME, 0.3f);
        audiosource.volume = Volume;
    }

    public void ChangeVolume()
    {
        Volume += 0.1f;
        if (Volume > 1f)
        {
            Volume = 0f;
        }
        audiosource.volume = Volume;
        PlayerPrefs.SetFloat(PLAYER_PREF_MUSIC_VOLUME, Volume);
        PlayerPrefs.Save();
    }
}
