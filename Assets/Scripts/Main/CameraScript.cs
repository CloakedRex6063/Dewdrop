using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Main
{
    public class CameraScript : MonoBehaviour
    {
        [SerializeField]
        private float defaultCameraSpeed = 1f;
        private float _cameraSpeed;
        private PC _pc;
        private GameManager _gm;
        [SerializeField] 
        private float fasterThreshold = 3;
        [SerializeField] 
        private float deathThreshold = 7.5f;
        [SerializeField] 
        private float startTime = 0.5f;
        private bool _startMov;
        [SerializeField]
        private float speedIncrease = 1f;
        [SerializeField] 
        private float maxCameraSpeed;

        private void Awake()
        {
            _gm = FindObjectOfType<GameManager>();
            _pc = FindObjectOfType<PC>();
        }

        private void Start()
        {
            StartCoroutine(DelayMovement());
        }

        private void FixedUpdate()
        {
            if(!_pc)
                return;
            if (_pc.transform.position.y < transform.position.y - fasterThreshold)
            {
                _cameraSpeed = defaultCameraSpeed > Math.Abs(_pc.GetVelocity()) ? defaultCameraSpeed: -_pc.GetVelocity();
            }
            else
            {
                _cameraSpeed = defaultCameraSpeed;
            }

            if (_pc.transform.position.y > transform.position.y + deathThreshold)
            {
                _gm.GameOver();
            }
        }

        private void Update()
        {
            if (_startMov)
            {
                transform.Translate(0,-_cameraSpeed * Time.deltaTime,0);
            }
        }

        IEnumerator DelayMovement()
        {
            yield return new WaitForSeconds(startTime);
            _startMov = true;
            StartCoroutine(IncreaseSpeed());
        }
        
        IEnumerator IncreaseSpeed()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                defaultCameraSpeed += speedIncrease;
                defaultCameraSpeed = Mathf.Clamp(defaultCameraSpeed,1f,maxCameraSpeed);
            }
        }
    }
}