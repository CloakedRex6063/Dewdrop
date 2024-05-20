using System;
using Audio;
using Managers;
using UnityEngine;

namespace Obstacles
{
    public class BeeMovement : MonoBehaviour
    {
        public float maxThreshold = 2f;
        public float speed = 1f;
        private AudioManager _aM;
        
        //Up at the top with your variables:
        private Vector3 _dir = Vector3.left;

        private void Awake()
        {
            _aM = FindObjectOfType<AudioManager>();
        }

        //Your Update function
        void FixedUpdate()
        {
            transform.Translate(_dir * (speed * Time.deltaTime));
 
            if(transform.position.x <= -maxThreshold)
            {
                _dir = Vector3.right;
            }
        
            else if(transform.position.x >= maxThreshold)
            {
                _dir = Vector3.left;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player"))
                return;
            _aM.Play(SoundType.Bees);
        }
    }
}
