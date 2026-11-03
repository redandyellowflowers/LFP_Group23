using System;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public SoundScript[] sounds;

    public static AudioManagerScript instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        foreach (SoundScript s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Play("Background");
    }

    public void Play(string name)
    {
        SoundScript s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();

        if (s == null)
        {
            Debug.Log("sound" + name + "is missing");
        }
        return;
    }

    public void PlayOnButtonPress(string name)
    {
        SoundScript s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.clip, 1f);

        if (s == null)
        {
            Debug.Log("sound" + name + "is missing");
        }
        return;
    }

    public void Stop(string name)
    {
        SoundScript s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();

        if (s == null)
        {
            Debug.Log("sound" + name + "is missing");
        }
        return;
    }
}
