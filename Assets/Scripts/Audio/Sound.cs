using Managers;
using UnityEngine;

namespace Audio
{
    [System.Serializable]
    public class Sound
    {
        public SoundType soundName;
        public AudioClip clip;
        [Range(0.1f,3f)]
        public float pitch;
        [Range(0f,1f)]
        public float volume;
        public bool loop;
        [HideInInspector]
        public AudioSource source;
    }
}
