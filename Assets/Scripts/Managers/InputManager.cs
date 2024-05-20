using System;
using Main;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace Managers
{
    [RequireComponent(typeof(ParticleSystem))]
    public class InputManager : MonoBehaviour
    {
        private PC _pc;
        private GameManager _gm;
        public bool slide;
        [SerializeField]
        private bool _allowInput = true;
        [SerializeField]
        private bool _allowForce = true;
        [SerializeField]
        private ParticleSystem normalTouchVFX;
        [SerializeField]
        private ParticleSystem alienTouchVFX;
        public Vector3 vfxPosition;

        private void Awake()
        {
            _gm = FindObjectOfType<GameManager>();
            _pc = FindObjectOfType<PC>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_allowInput) return;
            if (_allowForce)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ForceCheck();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _pc.AddScore(100f);
                }
            }
        }

        //Check if the
        void ForceCheck()
        {
            // Set up a raycast
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            LayerMask layerMask = LayerMask.GetMask("Force");
            
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask)) 
            {
                Transform parent = hitInfo.collider.gameObject.transform.parent;
                if (parent != null && parent.CompareTag("Player"))
                {
                    vfxPosition = hitInfo.point;
                    PlayVfx();
                    if (slide)
                    {
                        _pc.Slide(hitInfo.point);
                    }
                    else
                    {
                        _pc.ApplyForce(hitInfo.point);
                    }
                }
            }
        }
        
        public void AllowInput(bool toggle)
        {
            _allowInput = toggle;
        }
        
        public void AllowForce(bool toggle)
        {
            _allowForce = toggle;
        }

        void PlayVfx()
        {
            ParticleSystem vfx;
            switch (_gm.GetWorld())
            {
                case ProceduralManager.World.Normal:
                    vfx = Instantiate(normalTouchVFX);
                    break;
                case ProceduralManager.World.Alien:
                    vfx = Instantiate(alienTouchVFX);
                    break;
                default:
                    vfx = Instantiate(normalTouchVFX);
                    break;
            }
            
            // Set the position of the VFX
            vfx.transform.position = vfxPosition;
            // Play the VFX
            vfx.Play();
            Destroy (vfx.gameObject, vfx.main.duration); 
        }
    }
}