using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem Instance { get; private set; } = null;

    public Dictionary<string, AudioClip> SoundSources = new Dictionary<string, AudioClip>();

    void Awake()
    {
        Instance = this;
    }

}
public static class SoundManager
{
    private static SoundSystem soundSystem => SoundSystem.Instance;

}
