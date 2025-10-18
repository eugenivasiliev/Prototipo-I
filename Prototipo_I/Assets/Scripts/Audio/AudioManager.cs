using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private Dictionary<string, float> soundCooldowns = new Dictionary<string, float>();
    private float cooldownTime = 0.15f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s != null)
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s != null)
        {
            if (soundCooldowns.ContainsKey(name) && Time.time - soundCooldowns[name] < cooldownTime) return; // Avoids saturating
            soundCooldowns[name] = Time.time;
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void StopMusic()
    {
        if (musicSource.isPlaying) musicSource.Stop();
    }
    public void StopSFX()
    {
        sfxSource.Stop();
    }
}