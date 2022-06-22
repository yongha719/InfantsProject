using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SoundType
{
    BGM, EFFECT, BUTTON, END
}

public static class SoundName
{
    public const string BUTTONCLICK = "ButtonClick";
    public const string MISTAKE = "Mistake";
    public const string APPEAR = "Appear";
    public const string FADE = "Fade";
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } = null;

    AudioSource[] audioSources = new AudioSource[(int)SoundType.END];
    Dictionary<string, AudioClip> Clips = new Dictionary<string, AudioClip>();

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
            audioSources[(int)SoundType.BUTTON].volume = soundvolume;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            string[] soundnames = Enum.GetNames(typeof(SoundType));

            DontDestroyOnLoad(this);

            for (int i = 0; i < soundnames.Length - 1; i++)
            {
                GameObject playobj = new GameObject { name = soundnames[i] };
                audioSources[i] = playobj.AddComponent<AudioSource>();
                playobj.transform.SetParent(transform);
            }

            audioSources[(int)SoundType.BGM].loop = true;

            AudioClip[] audioClip = Resources.LoadAll<AudioClip>("Sound");

            foreach (var clip in audioClip)
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
        play("BGM", SoundType.BGM);
    }

    public static void AddButtonClick(Button[] buttons)
    {
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() =>
            {
                //Play(SoundName.BUTTONCLICK, SoundType.BUTTON);
                print("Button Click");
            });
        }
    }
    public static void Play(string name, SoundType soundType = SoundType.EFFECT) => Instance.play(name, soundType);
    private void play(string name, SoundType soundType = SoundType.EFFECT)
    {
        AudioSource audioSource = audioSources[(int)soundType];

        if (audioSource == null)
            throw new NullReferenceException($"AudioSource is Null!!\n name is {name} soundtype is {soundType}");

        if (Clips.TryGetValue(name, out AudioClip audioClip) == false)
            throw new KeyNotFoundException($"AudioClip {audioClip.name} is not find!!");

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

        print($"{audioClip.name} Play ");
    }
}
