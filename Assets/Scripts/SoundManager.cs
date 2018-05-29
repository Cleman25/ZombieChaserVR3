using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    public const string SFX_VOL = "sfxVol";
    public const string VOICE_VOL = "voiceVol";
    public const string MUSIC_VOL = "musicVol";
    public const string MASTER_VOL = "masterVol";

    const float DELAY_FOR_TEST_AUDIO = 1f;

    [SerializeField]
    AudioClip _testDialogueClip;
    public AudioClip testDialogueClip
    {
        get { return _testDialogueClip; }
    }

    [SerializeField]
    AudioClip _testSFXClip;
    public AudioClip testSFXClip
    {
        get { return _testSFXClip; }
    }

    [SerializeField] bool disableLogging;

    [Header("Audio Mixers")]
    [SerializeField]
    AudioMixer _masterMixer;
    public AudioMixerGroup dialogueChannel {
        get { return _masterMixer.FindMatchingGroups("Voices") [0]; }
    }

    public AudioMixerGroup sfxChannel {
        get { return _masterMixer.FindMatchingGroups("Sfx") [0]; }
    }

    [Header("Audio Sources")]
    public AudioSource MusicSource;
    public AudioSource zombieSource;
    public AudioSource ambientSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    private AudioSource[] audioSources = new AudioSource[4];

    [Header("Music")]
    public AudioClip MenuMusic;
    public AudioClip levelMusicCalm;
    public AudioClip levelMusicIntense;
    public AudioClip GameOverMusic;
    public AudioClip WinningMusic;

    // Guard Dialogue Type
    public enum DialogueType {
        Patrol,
        Alerted,
        Chasing,
        Stunned,
        Recovering
    }

    public Canvas AudioPanel;

    void Start() {
        LoadSavedSettings();
    }

    private void LoadSavedSettings() {
        //SetVolume(MASTER_VOL, SavePlayerPrefs.instance.GetVolume(MASTER_VOL));
        //SetVolume(VOICE_VOL, SavePlayerPrefs.instance.GetVolume(VOICE_VOL));
        //SetVolume(SFX_VOL, SavePlayerPrefs.instance.GetVolume(SFX_VOL));
        //SetVolume(MUSIC_VOL, SavePlayerPrefs.instance.GetVolume(MUSIC_VOL));
    }

    void Update() {
    }

    // Generic Play Functions
    public void PlaySound(AudioClip clip, AudioSource source) {

        if (source.isPlaying) { return; }
        
        source.clip = clip;
        source.Play();
    }

    public void PlaySound(AudioClip clip, AudioSource source, bool shouldInterrupt) {
        if (source.isPlaying && !shouldInterrupt) { return; }

        // Debug.Log("Source Object: " + source.gameObject + " Clip: " + clip);
        source.clip = clip;
        source.Play();
    }
    public void PlayMusic(AudioClip clip) {
        MusicSource.clip = clip;
        MusicSource.Play();
        //MusicLayer2Source.volume = 0;
        //MusicLayer2Source.Play();
    }

    public void PlaySound(AudioClip clip) {
        //SoundSource.clip = clip;
        //SoundSource.Play();
    }

    // Play Next Sound Function - Will play the next sound in the array and cycle back to the front once the end is reached
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