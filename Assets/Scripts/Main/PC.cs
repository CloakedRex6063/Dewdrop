using System;
using System.Collections;
using Managers;
using Obstacles;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Main
{
    public class PC : MonoBehaviour
    {
        /*
         * Components
         */
        
        // Rigidbody of the pc
        [HideInInspector]
        public Rigidbody rb;

        [SerializeField]
        private Material dropMat;
        
        [SerializeField]
        private Material slugMat;

        [HideInInspector]
        public MeshRenderer mr;

        // Coroutine to use the combo timer
        Coroutine _comboRoutine = null;
        
        // Powerup component
        [HideInInspector]
        public Powerup powerupComponent;
        
        public GameObject floatingScore;

        // Input Manager
        private InputManager _iM;

        /*
         * Constants
         */
        
        // Force multiplier which controls how much force is applied
        public float defaultForceMultiplier;
        
        // Force multiplier which controls how much force is applied
        public float forceMultiplier;
        
        // Stores the initial pos from which to start counting the distance travelled
        private Vector3 _initialPos;
        
        // Stores the amount of pickups the PC has gotten
        private int _pickups;
        
        // Stores the timer after which the pickups reset to 0
        public float comboTimer = 10;
        
        // Max velocity while falling downwards
        [SerializeField]
        private float maxNegVelocity = 4f;
        
        [SerializeField]
        private float maxPosVelocity = 2f;

        // Max velocity while moving upwards
        [SerializeField]
        private float maxSlideVelocity = 2f;

        // Score
        private float _score;
        
        // Basic Multiplier for the score
        [SerializeField]
        private float scoreConstant = 5f;
        
        private float _previousDistance;
        
        // Additional Multiplier for the score
        public float scoreMultiplier = 1f;
        
        // Time after which score popup gets destroys
        public float floatingScoreDuration = 1f;

        // Surface normal between the drop and the obstacle
        private Vector3 _surfaceNormal;
        private Vector3 previousPosition;
        private float previousTime;
        private Vector3 velocity;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            _initialPos = transform.position;
            powerupComponent = GetComponent<Powerup>();
            _iM = FindObjectOfType<InputManager>();
            mr = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            previousPosition = transform.position;
            previousTime = Time.time;
        }

        private void FixedUpdate()
        {
            if (_iM.slide)
            {
                float x = Mathf.Clamp(rb.velocity.x, -maxSlideVelocity, maxSlideVelocity);
                float y = Mathf.Clamp(rb.velocity.y, -maxSlideVelocity, maxSlideVelocity);
                float z = Mathf.Clamp(rb.velocity.z, -maxSlideVelocity, maxSlideVelocity);
                rb.velocity = new Vector3(x, y, z);
            }
            else
            {
                float x = Mathf.Clamp(rb.velocity.x, -maxNegVelocity, maxPosVelocity);
                float y = Mathf.Clamp(rb.velocity.y, -maxNegVelocity, maxPosVelocity);
                float z = Mathf.Clamp(rb.velocity.z, -maxNegVelocity, maxPosVelocity);
                rb.velocity = new Vector3(x, y, z);
            }
            CalculateVelocity();
            CalculateScore();
        }

        public float GetDistance()
        {
            return Math.Abs(transform.position.y - _initialPos.y);
        }

        public void AddScore(float score)
        {
            _score += score;
            Vector3 position = transform.position;
            position += new Vector3(Random.Range(-0.5f, 0.5f), 1.5f, -2f);
            var fS = Instantiate(floatingScore, position,Quaternion.identity);
            fS.GetComponent<FloatingText>().textComponent.text = score.ToString();
            Destroy(fS,floatingScoreDuration);
        }
        
        public float CalculateScore()
        {
            // Calculate the score based on the difference between the new distance and previous distance
            _score += (GetDistance() - _previousDistance) * scoreMultiplier * scoreConstant;

            // Update the previous distance to the current distance for the next frame
            _previousDistance = GetDistance();
            return Mathf.Round(_score);
        }

        public void Powerup(GameObject obstacle)
        {
            if (_comboRoutine != null)
            {
                StopCoroutine(_comboRoutine);
            }
            Destroy(obstacle);
            _pickups++;
            powerupComponent.UseCombo(_pickups);
            _comboRoutine = StartCoroutine(ComboTimer());
        }

        public float GetVelocity()
        {
            return velocity.y;
        }
        
        private IEnumerator ComboTimer()
        {
            int currentPickups = _pickups;
            yield return new WaitForSeconds(comboTimer);
            _pickups = 0;
            powerupComponent.ResetPowerups();
        }

        public void ApplyForce(Vector3 hitPoint)
        {
            // Calculate the direction of the tap relative to the ball's position
            Vector3 tapDirection = transform.position - hitPoint;
            tapDirection.Normalize();
            Vector3 force;
            if (tapDirection.y > 0.5f && rb.velocity.y < -2f)
            {
                forceMultiplier = defaultForceMultiplier * -(rb.velocity.y/2);
                force = new Vector3(tapDirection.x * defaultForceMultiplier, tapDirection.y * forceMultiplier,
                    tapDirection.z * defaultForceMultiplier);
            }
            else
            {
                forceMultiplier = defaultForceMultiplier;
                force = tapDirection * forceMultiplier;
            }
            rb.AddForce(force, ForceMode.Impulse);
        }
        
        public void Slide(Vector3 hitPoint)
        {
            // Calculate the direction of the tap relative to the ball's position
            Vector3 tapDirection = transform.position - hitPoint;
            tapDirection.Normalize();
            tapDirection.y = 0f;
            rb.AddForce(tapDirection * (forceMultiplier/2), ForceMode.Impulse);
        }

        public void SwitchMaterial(ProceduralManager.World currentWorld)
        {
            mr.material = currentWorld == ProceduralManager.World.Normal ? dropMat : slugMat;
        }

        public void CalculateVelocity()
        {
            // Get the current position and time
            Vector3 currentPosition = transform.position;
            float currentTime = Time.time;

            // Calculate the distance the object has moved since the last frame
            Vector3 displacement = currentPosition - previousPosition;

            // Calculate the time elapsed since the last frame
            float deltaTime = currentTime - previousTime;

            // Calculate the velocity by dividing displacement by deltaTime
            velocity = displacement / deltaTime;

            // Set the previousPosition and previousTime variables to the current position and time for the next frame
            previousPosition = currentPosition;
            previousTime = currentTime;
        }
    }
}
