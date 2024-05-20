using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        private GameManager _gm;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI hiScoreText;
        public Image hiScoreImage;

        private void Awake()
        {
            _gm = FindObjectOfType<GameManager>();
        }

        public void Retry()
        {
            _gm.Replay();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void MainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}