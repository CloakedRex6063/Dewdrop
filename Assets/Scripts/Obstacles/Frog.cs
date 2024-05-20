using Audio;
using Managers;
using UnityEngine;

namespace Obstacles
{
    public class Frog : BaseObstacle
    {
        private AudioManager _aM;

        protected override void Awake()
        {
            base.Awake();
            _aM = FindObjectOfType<AudioManager>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player"))
                return;
            _aM.Play(SoundType.Frog);
        }
    }
}
