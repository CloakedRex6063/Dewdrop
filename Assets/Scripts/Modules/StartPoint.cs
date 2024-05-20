using System;
using System.Collections;
using Main;
using Managers;
using UnityEngine;

namespace Modules
{
    public class StartPoint : MonoBehaviour
    {
        private AudioManager _am;
        private GameManager _gm;
        private InputManager _im;
        private PC _pc;
        public Transform endPoint;
        public float duration = 5f;
        [SerializeField]
        private Outline outline1;
        [SerializeField]
        private Outline outline2;
        [SerializeField]
        private Outline outline3;
        [SerializeField]
        private Outline outline4;

        private void Awake()
        {
            _pc = FindObjectOfType<PC>();
            _im = FindObjectOfType<InputManager>();
            _am = FindObjectOfType<AudioManager>();
            _gm = FindObjectOfType<GameManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(Shift());
            _im.AllowForce(false);
            SoundType type = _gm.GetWorld() == ProceduralManager.World.Normal
                ? SoundType.NatureAmbient
                : SoundType.AlienAmbient;
            _am.FadeOut(type,1f);
            _gm.SwapWorlds();
            _gm.ToggleLighting();
            _am.FadeIn(SoundType.TransitionAmbient,0.5f);
            _am.Play(SoundType.TransitionAmbient);
            _gm.transition = true;
            _gm.outlineRoutine = StartCoroutine(TransitionFlickerCoroutine());
            _gm.pcOutlineRoutine = StartCoroutine(PcTransitionFlicker());
        }
        
        private IEnumerator Drop()
        {
            float elapsedTime = 0;
            Vector3 startingPosition = _pc.transform.position;
            Vector3 targetPosition = endPoint.position;
            while (elapsedTime < duration)
            {
                _pc.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _pc.rb.velocity = Vector3.down * 2;
        }
        
        private IEnumerator Shift()
        {
            float elapsedTime = 0;
            Vector3 startingPosition = _pc.transform.position;
            Vector3 targetPosition = new Vector3(0,transform.position.y-1,0);
            while (elapsedTime < 0.5f)
            {
                _pc.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / 0.5f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            StartCoroutine(Drop());
        }
        
        IEnumerator TransitionFlickerCoroutine()
        {
            float flashTimer = 0f; // timer to keep track of time since last flash
            bool _isVisible = true;
            while (true)
            {
                flashTimer += Time.deltaTime;

                if (flashTimer >= 0.1f)
                {
                    _isVisible = !_isVisible;
                    outline1.enabled = _isVisible;
                    outline2.enabled = _isVisible;
                    outline3.enabled = _isVisible;
                    outline4.enabled = _isVisible;
                    flashTimer = 0f;
                }
                yield return null;
            }
        }

        IEnumerator PcTransitionFlicker()
        {
            float flashTimer = 0f; // timer to keep track of time since last flash
            bool _isVisible = true;
            while (true)
            {
                flashTimer += Time.deltaTime;

                if (flashTimer >= 0.1f)
                {
                    _isVisible = !_isVisible;
                    _pc.mr.enabled = _isVisible;
                    flashTimer = 0f;
                }
                yield return null;
            }
        }
        
    }
}
