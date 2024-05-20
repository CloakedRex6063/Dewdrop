using Managers;
using UnityEngine;

namespace Modules
{
    public class MidPoint : MonoBehaviour
    {
        private BoxCollider _collider;
        private ProceduralManager _pm;
    
        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<BoxCollider>();
            _pm = FindObjectOfType<ProceduralManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            _pm.UseProcGen();
        }
    }
}
