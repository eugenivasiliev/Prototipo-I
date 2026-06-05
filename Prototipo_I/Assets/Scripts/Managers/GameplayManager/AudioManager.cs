using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils;
using FMODUnity;
using FMOD.Studio;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Serializable]
        public struct NamedEvent
        {
            public string name;
            public EventReference reference;
        }

        public NamedEvent[] ambientEvents, musicEvents, sfxEvents;
        public EventInstance ambientInstance, musicInstance, sfxInstance;
        public AudioSource musicSource, sfxSource;

        private Dictionary<string, float> soundCooldowns = new Dictionary<string, float>();
        private Dictionary<string, EventInstance> loopingSources = new Dictionary<string, EventInstance>();
        [SerializeField] private float cooldownTime = 0.15f;
        private void Awake()
        {
            InitSingleton();
        }

        public void PlayAmbientEvent(string name)
        {
            ambientInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ambientInstance = RuntimeManager.CreateInstance(
                Array.Find(ambientEvents, e => e.name == name).reference
            );
            ambientInstance.start();
        }

        public void PlayMusicEvent(string name)
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance = RuntimeManager.CreateInstance(
                Array.Find(musicEvents, e => e.name == name).reference
            );
            musicInstance.start();
        }

        public void PlaySFXEvent(string name)
        {
            sfxInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            sfxInstance = RuntimeManager.CreateInstance(
                Array.Find(sfxEvents, e => e.name == name).reference
            );
            sfxInstance.start();
        }

        public void PlaySFXLoop(string name)
        {
            if (loopingSources.ContainsKey(name)) return;

            EventInstance instance = RuntimeManager.CreateInstance(
                Array.Find(sfxEvents, e => e.name == name).reference
            );
            instance.start();
            loopingSources.Add(name, instance);
        }
        public void StopSFXLoop(string name)
        {
            if (!loopingSources.ContainsKey(name)) return;

            loopingSources[name].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            loopingSources.Remove(name);
        }
        public void StopMusic()
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        public void StopSFX()
        {
            sfxInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}