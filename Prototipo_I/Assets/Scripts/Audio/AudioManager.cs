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
    private Dictionary<string, AudioSource> loopingSources = new Dictionary<string, AudioSource>();
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

    public void PlaySFXLoop(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s != null)
        {
            if (loopingSources.ContainsKey(name)) return;
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = s.clip;
            newSource.loop = true;
            newSource.volume = sfxSource.volume;
            newSource.Play();

            loopingSources[name] = newSource;
        }
    }
    public void StopLoop(string name)
    {
        if (!loopingSources.ContainsKey(name)) return;

        loopingSources[name].Stop();
        Destroy(loopingSources[name]);
        loopingSources.Remove(name);
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