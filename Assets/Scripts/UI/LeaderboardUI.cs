using Managers;
using UnityEngine;

namespace UI
{
    public class LeaderboardUI : MonoBehaviour
    {
        private GameManager _gm;

        private void Awake()
        {
            _gm = FindObjectOfType<GameManager>();
        }

        public void Back()
        {
            _gm.ShowMainMenu();
        }
    }
}
