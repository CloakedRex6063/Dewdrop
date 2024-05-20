using UnityEngine;

namespace Obstacles
{
    public class FrogTongue : BaseObstacle
    {
        public float maxThreshold = 10f;
        public float speed = 20f;
        private Vector3 _dir;
        public Transform pivot;
        private void Start()
        {
            _dir = Vector3.right;
        }

        void FixedUpdate()
        {
            pivot.localScale = new Vector3(pivot.localScale.x,0,0) + new Vector3(_dir.x * (speed * Time.fixedDeltaTime),1f,1f);
            
            if(pivot.localScale.x <= 1)
            {
                _dir = Vector3.right;
            }
        
            else if(pivot.localScale.x >= maxThreshold)
            {
                _dir = Vector3.left;
            }
        }
    }
}
