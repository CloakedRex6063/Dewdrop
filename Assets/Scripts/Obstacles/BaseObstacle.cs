using System;
using System.Collections.Generic;
using Main;
using Managers;
using UnityEngine;

namespace Obstacles
{
    public enum ObstacleType
    {
        NonFatal,
        Fatal,
        Special
    }
    public abstract class BaseObstacle : MonoBehaviour
    {
        public ObstacleType obstacleType;
        private PC _pc;
        private GameManager _gm;
        private Collider _collider;
        private bool _bounced;
        private InputManager _iM;

        protected virtual void Awake()
        {
            _iM = FindObjectOfType<InputManager>();
            _pc = FindObjectOfType<PC>();
            _collider = GetComponent<Collider>();
            _gm = FindObjectOfType<GameManager>();
        }

        protected void Behavior()
        {
            switch (obstacleType)
            {
                case ObstacleType.Fatal:
                    if (_gm)
                    {
                        _gm.GameOver();
                    }
                    break;
            
                case ObstacleType.NonFatal:
                    if (_pc.GetVelocity() > 1f && !_bounced)
                    {
                        _collider.material.bounciness = 0;
                        _bounced = true;
                        _gm.Bounce();
                    }
                    _iM.slide = true;
                    break;
                
                case ObstacleType.Special:
                    _pc.Powerup(gameObject);
                    break;
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            Collider other = collision.collider;
            if (other.CompareTag("Player"))
            {
                Behavior();
            }
        }

        private void OnCollisionExit()
        {
            _iM.slide = false;
        }
    }
}