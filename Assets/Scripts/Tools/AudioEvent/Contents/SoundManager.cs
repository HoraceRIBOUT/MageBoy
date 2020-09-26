using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FafaTools.Audio
{
    public class SoundManager : SingletonMono<SoundManager>
    {
        private AudioSource audioSourceDefault;
        private AudioSource audioSourceMusic;
        private AudioSource audioSourceUI;
        private AudioSource audioSourceEffects;

#pragma warning disable 0649
        [SerializeField]
        private AudioDatabase m_SoundDataDase;

        [SerializeField]
        private AudioClip musicIntro, musicLoop;
#pragma warning restore 0649 
        private void Awake()
        {
            GameObject audioSourceDefault_GO = new GameObject("SM_AudioSource_Default");
            audioSourceDefault_GO.transform.parent = transform;
            audioSourceDefault = audioSourceDefault_GO.AddComponent<AudioSource>();
            audioSourceDefault.playOnAwake = false;
            audioSourceDefault.spatialBlend = 0;

            GameObject audioSourceMusic_GO = new GameObject("SM_AudioSource_Music");
            audioSourceMusic_GO.transform.parent = transform;
            audioSourceMusic = audioSourceMusic_GO.AddComponent<AudioSource>();
            audioSourceMusic.playOnAwake = false;
            audioSourceMusic.spatialBlend = 0;

            GameObject audioSourceUI_GO = new GameObject("SM_AudioSource_UI");
            audioSourceUI_GO.transform.parent = transform;
            audioSourceUI = audioSourceUI_GO.AddComponent<AudioSource>();
            audioSourceUI.playOnAwake = false;
            audioSourceUI.spatialBlend = 0;

            GameObject audioSourceEffects_GO = new GameObject("SM_AudioSource_Effects");
            audioSourceEffects_GO.transform.parent = transform;
            audioSourceEffects = audioSourceEffects_GO.AddComponent<AudioSource>();
            audioSourceEffects.playOnAwake = false;
            audioSourceEffects.spatialBlend = 0;
        }

        private void Start()
        {
            audioSourceMusic.volume = 0.4f;
            audioSourceMusic.clip = musicIntro;
            audioSourceMusic.Play();
            audioSourceMusic.loop = false;
        }

        private void Update()
        {
            if (!audioSourceMusic.isPlaying && audioSourceMusic.clip == musicIntro)
            {
                audioSourceMusic.clip = musicLoop;
                audioSourceMusic.Play();
                audioSourceMusic.loop = true;
            }
        }

        public void PlaySound(AudioFieldEnum sound)
        {
            AudioSourceType sourceType = m_SoundDataDase.GetAudioEvent(sound).audioSourceType;
            m_SoundDataDase?.PlaySound(sound, GetAudioSource(sourceType));
        }

        public AudioSource GetAudioSource(AudioSourceType audioSourceType)
        {
            switch (audioSourceType)
            {
                case AudioSourceType.Music:
                    return audioSourceMusic;
                case AudioSourceType.UI:
                    return audioSourceUI;
                case AudioSourceType.Effects:
                    return audioSourceEffects;
                default:
                    return audioSourceDefault;
            }
        }

        public void SetVolume(AudioSourceType audioSourceType, float volume)
        {
            GetAudioSource(audioSourceType).volume = volume;
        }

        public void FadeInVolume(AudioSourceType audioSourceType, float time)
        {
            AudioSource source = GetAudioSource(audioSourceType);
            DOTween.To(() => source.volume, x => source.volume = x, 1, time);
        }

        public void FadeOutVolume(AudioSourceType audioSourceType, float time)
        {
            AudioSource source = GetAudioSource(audioSourceType);
            DOTween.To(() => source.volume, x => source.volume = x, 0, time);
        }

        public void StopAudioSource(AudioSourceType audioSourceType)
        {
            GetAudioSource(audioSourceType).Stop();
        }


        public enum AudioSourceType
        {
            Default = 0,
            Music = 1,
            UI = 2,
            Effects = 3
        }
    }
}
