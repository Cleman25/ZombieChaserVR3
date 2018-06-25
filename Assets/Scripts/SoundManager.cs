﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
    static SoundManager _instance = null;
    public const string SFX_VOL = "sfxVol";
    public const string VOICE_VOL = "voiceVol";
    public const string MUSIC_VOL = "bgVol";
    public const string AMB_VOL = "ambienceVol";
    public const string MASTER_VOL = "masterVol";

    const float DELAY_FOR_TEST_AUDIO = 1f;

    [SerializeField] bool disableLogging;

    [Header("Audio Mixers")]
    [SerializeField]
    AudioMixer _masterMixer;
    public AudioMixerGroup backgroundChannel {
        get { return _masterMixer.FindMatchingGroups("Background") [0]; }
    }

    public AudioMixerGroup sfxChannel {
        get { return _masterMixer.FindMatchingGroups("SFX") [0]; }
    }

    public AudioMixerGroup ambienceChannel {
        get { return _masterMixer.FindMatchingGroups("Ambience")[0]; }
    }

    public AudioMixerGroup zombieChannel {
        get { return _masterMixer.FindMatchingGroups("Zombies")[0]; }
    }

    [Header("Audio Sources")]
    public AudioSource backgroundSource;
    public AudioSource zombiesSource;
    public AudioSource ambienceSource;
    public AudioSource sfxSource;

    private AudioSource[] audioSources = new AudioSource[4];

    [Header("Music")]
    public AudioClip ambienceMusic;
    public AudioClip WinningMusic;
    public AudioClip zombieSOund;

    // Guard Dialogue Type
    public enum DialogueType {
        Patrol,
        Alerted,
        Chasing,
        Stunned,
        Recovering
    }

    public Canvas AudioPanel;

    void Awake() {
        if(instance) {
            DestroyImmediate(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static SoundManager instance {
        get { return _instance; }
        set { _instance = value; }
    }

    // Generic Play Functions
    public void PlaySound(AudioClip clip, AudioSource source) {

        if (source.isPlaying) { return; }
        
        source.clip = clip;
        source.Play();
    }

    public void PlaySound(AudioClip clip, AudioSource source, bool shouldInterrupt) {
        if (source.isPlaying && !shouldInterrupt) { return; }
        source.clip = clip;
        source.Play();
    }

    public void PlayMusic(AudioClip clip) {
        backgroundSource.clip = clip;
        backgroundSource.Play();
    }

    public void PlayMusic(AudioClip clip, AudioSource source) {
        source.clip = clip;
        source.Play();
    }
    
    public void PlayNextSound(AudioSource source, AudioClip[] clips, ref int clipsIndex, bool shouldInterrupt) {
        if (source.isPlaying && !shouldInterrupt) { return; }

        if (clipsIndex >= clips.Length) { clipsIndex = 0; }

        source.clip = clips[clipsIndex];
        source.Play();

        clipsIndex++;
    }

    public void SetVolume(string key, float volume) {
        _masterMixer.SetFloat(key, volume);
    }

    public float GetVolume(string key) {
        float volume = 0;
        _masterMixer.GetFloat(key, out volume);
        return volume;
    }

    public void PauseAllAudio() {
        foreach (AudioSource source in FindObjectsOfType<AudioSource>()) {
            source.Pause();
        }
    }

    public void UnPauseAllAudio() {
        foreach (AudioSource source in FindObjectsOfType<AudioSource>()) {
            source.Play();
        }
    }
}
