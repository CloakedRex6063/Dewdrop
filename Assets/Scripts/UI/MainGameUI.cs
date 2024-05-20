using Managers;
using UnityEngine;

namespace UI
{
    public class MainGameUI : MonoBehaviour
    {
        private GameManager _gm;
        
        private void Awake()
        {
            _gm = FindObjectOfType<GameManager>();
        }
        public void Pause()
        {
            _gm.PauseGame();
        }
    }
}
