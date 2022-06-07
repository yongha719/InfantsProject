using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM, EFFECT, BUTTON, END
}
public class Clip
{
    public string name;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } = null;

    AudioSource[] audioSources = new AudioSource[(int)SoundType.END];
    List<Clip> Clips = new List<Clip>();

    private float bgm_volume = 0.5f;
    public float BgmVolume
    {
        get => bgm_volume;
        set
        {
            bgm_volume = value;
            audioSources[((int)SoundType.BGM)].volume = BgmVolume;
        }
    }

    private float soundvolume = 0.5f;
    public float SoundVolume
    {
        get => soundvolume;
        set
        {
            soundvolume = value;
            audioSources[(int)SoundType.EFFECT].volume = soundvolume;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            GameObject sound = GameObject.Find("SoundManager");
            string[] soundNames = Enum.GetNames(typeof(SoundType));

            if (sound != null)
            {
                DontDestroyOnLoad(sound);

                for (int i = 0; i < soundNames.Length - 1; i++)
                {
                    GameObject playobj = new GameObject { name = soundNames[i] };
                    audioSources[i] = playobj.AddComponent<AudioSource>();
                    playobj.transform.SetParent(sound.transform);
                }
            }
        }
    }

    public void Play(string name, SoundType soundType = SoundType.EFFECT)
    {
        var clip = Clips.Find((o) => { return o.name == name; });
        if (clip == null)
        {
            throw new NullReferenceException($"Audio Clip {name} Is NULL!!");
        }
        AudioSource audioSource;

        audioSource = audioSources[(int)soundType];
        audioSource.PlayOneShot(clip.clip);
    }
}
