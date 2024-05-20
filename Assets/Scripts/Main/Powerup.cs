using System;
using System.Collections;
using UnityEngine;

namespace Main
{
    enum PowerupType
    {
        ScoreMultiplier,
        DistanceBoost,
        Invincibility,
        CancelSlowdown
    }

    enum ComboType
    {
        Combo1,
        Combo2,
        Combo3,
        Combo4
    }
    public class Powerup : MonoBehaviour
    {
        private PC _pc;
        public float boostDuration = 0.5f;
        private bool _isVisible = true;
        private float flashInterval = 0.1f;
        private float _invincibilityProgress;
        private float _scoreProgress;
        private Coroutine _previousScoreCoroutine;
        private Coroutine _previousInvincibilityCoroutine;
        [HideInInspector]
        public int scoreMultiplier;

        // Start is called before the first frame update
        void Start()
        {
            _pc = GetComponent<PC>();
        }

        public float GetInvincibilityProgress()
        {
            return _invincibilityProgress;
        }
        
        public float GetScoreProgress()
        {
            return _scoreProgress;
        }
        
        public void UseCombo(int pickups)
        {
            scoreMultiplier = pickups + 1; 
            switch (pickups)
            {
                case 1:
                    _pc.AddScore(50f);
                    UsePowerup(PowerupType.Invincibility);
                    UsePowerup(PowerupType.ScoreMultiplier,2);
                    break;
                case 2:
                    _pc.AddScore(100f);
                    UsePowerup(PowerupType.DistanceBoost,0,5);
                    UsePowerup(PowerupType.ScoreMultiplier,3);
                    break;
                case 3:
                    _pc.AddScore(150f);
                    UsePowerup(PowerupType.DistanceBoost,0,10);
                    UsePowerup(PowerupType.ScoreMultiplier,4);
                    break;
                default:
                    _pc.AddScore(pickups * 50f);
                    UsePowerup(PowerupType.DistanceBoost,0,(5 * pickups)-5);
                    UsePowerup(PowerupType.ScoreMultiplier,pickups+1);
                    break;
            }
        }

        public void ResetPowerups()
        {
            _pc.scoreMultiplier = 1;
        }

        void UsePowerup(PowerupType type, int multiplier = 0,int boost = 0)
        {
            switch (type)
            {
                case PowerupType.ScoreMultiplier:
                    _pc.scoreMultiplier = multiplier;
                    if (_previousScoreCoroutine != null)
                    {
                        StopCoroutine(_previousScoreCoroutine);
                    }
                    _previousScoreCoroutine = StartCoroutine(ScoreProgressTracker(_pc.comboTimer));
                    break;
                case PowerupType.DistanceBoost:
                    StartCoroutine(DistanceBoost(boost));
                    break;
                case PowerupType.Invincibility:
                    if (_previousInvincibilityCoroutine != null)
                    {
                        StopCoroutine(_previousInvincibilityCoroutine);
                    }
                    _previousInvincibilityCoroutine = StartCoroutine(Invincibility(5f));
                    StartCoroutine(InvincibilityProgressTracker(5f));
                    break;
                case PowerupType.CancelSlowdown:
                    break;
            }
        }

        IEnumerator Invincibility(float invincibilityDuration)
        {
            Physics.IgnoreLayerCollision(0,6);
            StartCoroutine(InvincibilityCoroutine(invincibilityDuration));
            yield return new WaitForSeconds(invincibilityDuration);
            StopCoroutine(InvincibilityCoroutine(invincibilityDuration));
            _pc.mr.enabled = true;
            Physics.IgnoreLayerCollision(0,6,false);
        }

        IEnumerator DistanceBoost(int boost)
        {
            UsePowerup(PowerupType.Invincibility);
            _pc.rb.velocity = Vector3.zero;
            _pc.rb.useGravity = false;
            float elapsedTime = 0;
            Vector3 startingPosition = _pc.transform.position;
            Vector3 target = new Vector3(0,transform.position.y - boost,0);
            
            while (elapsedTime < boostDuration)
            {
                transform.position = Vector3.Lerp(startingPosition, target,elapsedTime / boostDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _pc.rb.useGravity = true;
            _pc.rb.velocity = Vector3.down * 2;
        }

        IEnumerator InvincibilityCoroutine(float invincibilityDuration)
        {
            float invincibilityTimer = 0f; // timer to keep track of invincibility duration
            float flashTimer = 0f; // timer to keep track of time since last flash

            while (invincibilityTimer < invincibilityDuration)
            {
                flashTimer += Time.deltaTime;

                if (flashTimer >= flashInterval)
                {
                    _isVisible = !_isVisible;
                    _pc.mr.enabled = _isVisible;
                    flashTimer = 0f;
                }

                invincibilityTimer += Time.deltaTime;

                yield return null;
            }
        }
        
        private IEnumerator InvincibilityProgressTracker(float duration)
        {
            float timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                _invincibilityProgress = Mathf.Lerp(1f, 0f, timeElapsed / duration);
                yield return null;
            }
            _invincibilityProgress = 0f;
        }
        
        private IEnumerator ScoreProgressTracker(float duration)
        {
            float timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                _scoreProgress = Mathf.Lerp(1f, 0f, timeElapsed / duration);
                yield return null;
            }
            _scoreProgress = 0f;
        }
    }
}
