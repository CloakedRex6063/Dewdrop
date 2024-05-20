using System;
using Main;
using Managers;
using UnityEngine;

namespace Modules
{
    public class EndPoint : MonoBehaviour
    {
        private AudioManager _am;
        private GameManager _gm;
        private InputManager _im;
        private PC _pc;

        private void Awake()
        {
            _pc = FindObjectOfType<PC>();
            _im = FindObjectOfType<InputManager>();
            _am = FindObjectOfType<AudioManager>();
            _gm = FindObjectOfType<GameManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_gm.transition) return;
            _pc.mr.enabled = true;
            StopCoroutine(_gm.outlineRoutine);
            StopCoroutine(_gm.pcOutlineRoutine);
            _pc.rb.useGravity = true;
            SoundType type = _gm.GetWorld() == ProceduralManager.World.Normal
                ? SoundType.NatureAmbient
                : SoundType.AlienAmbient;
            _am.FadeOut(SoundType.TransitionAmbient,1f);
            _am.FadeIn(type,0.5f);
            _am.Play(type);
            _gm.transition = false;
            _im.AllowForce(true);
        }
    }
}
