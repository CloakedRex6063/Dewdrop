using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        private GameManager _gm;
        private bool _mute;
        public Image audioIconComponent;
        public Sprite muteSprite;
        public Sprite unMuteSprite;

        private void Awake()
        {
            _gm = FindObjectOfType<GameManager>();
        }

        public void UnPause()
        {
            _gm.UnPauseGame();
        }
        
        public void MainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void Mute()
        {
            _gm.Mute(unMuteSprite,muteSprite,audioIconComponent);
        }
    }
}
