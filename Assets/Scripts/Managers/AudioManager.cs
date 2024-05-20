using System;
using Audio;
using UnityEngine;

namespace Managers
{
    public enum SoundType
    {
        NatureAmbient,
        TransitionAmbient,
        AlienAmbient,
        Bees,
        Frog,
        Bounce,
        Death
    } 
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;
        private Sound natureAmbient;
        private Sound transition;
        private Sound alienAmbient;
        private Sound death;
        private Sound frog;
        private Sound bee;
        private Sound bounce;
        
        private void Awake()
        {
            AddSound();
            Play(SoundType.NatureAmbient);
        }

        public void Play(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.NatureAmbient:
                    natureAmbient.source.Play();
                    break;
                case SoundType.Bees:
                    bee.source.Play();
                    break;
                case SoundType.Death:
                    death.source.Play();
                    break;
                case SoundType.Bounce:
                    bounce.source.Play();
                    break;
                case SoundType.TransitionAmbient:
                    transition.source.Play();
                    break;
                case SoundType.AlienAmbient:
                    alienAmbient.source.Play();
                    break;
                case SoundType.Frog:
                    frog.source.Play();
                    break;
            }
        }
        
        public void Stop(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.NatureAmbient:
                    natureAmbient.source.Stop();
                    break;
                case SoundType.Bees:
                    bee.source.Stop();
                    break;
                case SoundType.Death:
                    death.source.Stop();
                    break;
                case SoundType.Bounce:
                    bounce.source.Stop();
                    break;
                case SoundType.TransitionAmbient:
                    transition.source.Stop();
                    break;
                case SoundType.AlienAmbient:
                    alienAmbient.source.Stop();
                    break;
                case SoundType.Frog:
                    frog.source.Stop();
                    break;
            }
        }

        public void FadeOut(SoundType soundType,float duration)
        {
            switch (soundType)
            {
                case SoundType.NatureAmbient:
                    StartCoroutine(FadeAudioSource.StartFade(natureAmbient.source,duration, 0f));
                    break;
                case SoundType.TransitionAmbient:
                    StartCoroutine(FadeAudioSource.StartFade(transition.source, duration, 0f));
                    break;
                case SoundType.AlienAmbient:
                    StartCoroutine(FadeAudioSource.StartFade(alienAmbient.source, duration, 0f));
                    break;
            }
        }
        
        public void FadeIn(SoundType soundType,float duration)
        {
            switch (soundType)
            {
                case SoundType.NatureAmbient:
                    StartCoroutine(FadeAudioSource.StartFade(natureAmbient.source,duration, 0.4f));
                    break;
                case SoundType.TransitionAmbient:
                    StartCoroutine(FadeAudioSource.StartFade(transition.source, duration, 0.2f));
                    break;
                case SoundType.AlienAmbient:
                    StartCoroutine(FadeAudioSource.StartFade(alienAmbient.source, duration, 0.4f));
                    break;
            }
        }

        void AddSound()
        {
            foreach (var t in sounds)
            {
                t.source = gameObject.AddComponent<AudioSource>();
                t.source.clip = t.clip;
                t.source.pitch = t.pitch;
                t.source.volume = t.volume;
                t.source.loop = t.loop;
                switch (t.soundName)
                {
                    case SoundType.NatureAmbient:
                        natureAmbient = t;
                        break;
                    case SoundType.Bees:
                        bee = t;
                        break;
                    case SoundType.Death:
                        death = t;
                        break;
                    case SoundType.Bounce:
                        bounce = t;
                        break;
                    case SoundType.TransitionAmbient:
                        transition = t;
                        break;
                    case SoundType.AlienAmbient:
                        alienAmbient = t;
                        break;
                    case SoundType.Frog:
                        frog = t;
                        break;
                }
            }
        }

        public void Mute()
        { 
            AudioSource[] audioSources= FindObjectsOfType<AudioSource>();
            foreach (var t in audioSources)
            {
                t.mute = true;
            }
        }
        
        public void UnMute()
        { 
            AudioSource[] audioSources= FindObjectsOfType<AudioSource>();
            foreach (var t in audioSources)
            {
                t.mute = false;
            }
        }
    }
}
