using System;
using System.Collections.Generic;
using Main;
using UI;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] 
        private GameObject mainMenu;
        [SerializeField] 
        private GameObject pauseMenu;
        [SerializeField] 
        private GameObject gameOverMenu;
        [SerializeField] 
        private GameObject gameUiMenu;

        private GameManager _gm;
        private List<GameObject> _ui = new();

        private void Awake()
        {
            _gm = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            _ui.Add(mainMenu);
            _ui.Add(pauseMenu);
            _ui.Add(gameOverMenu);
            _ui.Add(gameUiMenu);
        }

        public void SetScore(float score)
        {
            gameOverMenu.GetComponent<GameOverUI>().scoreText.text = score.ToString();
        }
        
        public void SetHiScore(float score)
        {
            gameOverMenu.GetComponent<GameOverUI>().hiScoreText.text = score > 0 ? "Hi-Score: " + score.ToString(): "";
            gameOverMenu.GetComponent<GameOverUI>().hiScoreImage.enabled = score > 0;
            mainMenu.GetComponent<MainMenuUI>().hiScoreText.text = score > 0 ? "Hi-Score: " + score.ToString(): "";
            mainMenu.GetComponent<MainMenuUI>().hiScoreImage.enabled = score > 0;
        }
        public void TurnOffAllUI()
        {
            foreach (var t in _ui)
            {
                t.SetActive(t == gameUiMenu);
            }
        }

        public void SetMainMenuActive()
        {
            foreach (var t in _ui)
            {
                if (t == mainMenu)
                {
                    t.SetActive(true);
                    MainMenuUI main= t.GetComponent<MainMenuUI>();
                    _gm.MuteCheck(main.unMuteSprite,main.muteSprite,main.audioIconComponent); 
                    _gm.AdToggleCheck(main.adsIconComponent,main.onSprite,main.offSprite,main.adsTextComponent);
                }
                else
                {
                    t.SetActive(false);
                }
            }
        }
    
        public void SetPauseMenuActive()
        {
            foreach (var t in _ui)
            {
                if (t == pauseMenu)
                {
                    t.SetActive(true);
                    PauseMenuUI pause= t.GetComponent<PauseMenuUI>();
                    _gm.MuteCheck(pause.unMuteSprite,pause.muteSprite,pause.audioIconComponent); 
                }
                else
                {
                    t.SetActive(false);
                }
            }
        }
        
        public void SetPauseMenuInactive()
        {
            pauseMenu.SetActive(false);
        }

        public void SetGameOverMenuActive()
        {
            foreach (var t in _ui)
            {
                t.SetActive(t == gameOverMenu);
            }
        }
    }
}
