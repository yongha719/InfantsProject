using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SoundType
{
    BGM, SE, BUTTON, END
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private float bgmVolume = 0.5f;
    public float BGM_Volume
    {
        get => bgmVolume;
        set
        {
            bgmVolume = value;
            audioSources[(int)SoundType.BGM].volume = BGM_Volume;
        }
    }
    private bool bgmMute = false;
    public bool BGM_Mute
    {
        get => bgmMute;
        set
        {
            bgmMute = value;
            audioSources[(int)SoundType.BGM].mute = bgmMute;
        }
    }
    private float seVolume = 0.5f;
    public float SEVolume
    {
        get => seVolume;
        set
        {
            seVolume = value;
            audioSources[(int)SoundType.SE].volume = seVolume;
            audioSources[(int)SoundType.BUTTON].volume = seVolume;
        }
    }
    private bool seMute = false;
    public bool SEMute
    {
        get => seMute;
        set
        {
            seMute = value;
            audioSources[((int)SoundType.SE)].mute = seMute;
            audioSources[((int)SoundType.BUTTON)].mute = seMute;
        }
    }

    AudioSource[] audioSources = new AudioSource[(int)SoundType.END];
    Dictionary<string, AudioClip> Clips = new Dictionary<string, AudioClip>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            string[] soundnames = Enum.GetNames(typeof(SoundType));

            for (int i = 0; i < soundnames.Length - 1; i++)
            {
                GameObject playobj = new GameObject { name = soundnames[i] };
                audioSources[i] = playobj.AddComponent<AudioSource>();
                playobj.transform.SetParent(transform);
            }

            audioSources[(int)SoundType.BGM].loop = true;

            foreach (var clip in Resources.LoadAll<AudioClip>("Sound"))
            {
                Clips[clip.name] = clip;
            }
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        BGM_Volume = PlayerPrefs.HasKey(SPrefsKey.BGM_VOLUME) ? PlayerPrefs.GetFloat(SPrefsKey.BGM_VOLUME) : bgmVolume;
        if (PlayerPrefs.HasKey(SPrefsKey.BGM_MUTE))
            BGM_Mute = PlayerPrefs.GetInt(SPrefsKey.BGM_MUTE) == 1;

        SEVolume = PlayerPrefs.HasKey(SPrefsKey.SE_VOLUME) ? PlayerPrefs.GetFloat(SPrefsKey.SE_VOLUME) : SEVolume;
        if (PlayerPrefs.HasKey(SPrefsKey.SE_MUTE))
            SEMute = PlayerPrefs.GetInt(SPrefsKey.SE_MUTE) == 1;

        play("BGM", SoundType.BGM);
    }

    public static void AddButtonClickSound(Button[] buttons)
    {
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() =>
            {
                Play(SsoundName.BUTTON_CLICK, SoundType.BUTTON);
                print("Button Click");
            });
        }
    }
    public static void Play(string name, SoundType soundType = SoundType.SE) => Instance.play(name, soundType);
    private void play(string name, SoundType soundType = SoundType.SE)
    {
        AudioSource audioSource = audioSources[(int)soundType];

        if (audioSource == null)
            Debug.Assert(false, $"AudioSource is Null\nSoundType is {soundType}");

        if (Clips.TryGetValue(name, out AudioClip audioClip))
        {
            if (soundType == SoundType.BGM)
            {
                if (audioSource.isPlaying)
                    return;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
        else
            Debug.Assert(false, $"AudioClip {name} is not find");


        print($"{name} Play ");
    }
}
