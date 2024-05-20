using System;
using UnityEngine;

namespace Obstacles
{
    public class Flower : BaseObstacle
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Behavior();
            }
        }
    }
}
