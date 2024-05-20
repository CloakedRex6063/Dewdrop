using System;
using UnityEngine;

namespace Managers
{
    public class GameInstance : MonoBehaviour
    {
        private bool _requestedRestart;
        private int _score;
        public bool ads;
        public bool mute;
        
        private void Awake()
        {
            _score = PlayerPrefs.GetInt("HiScore");
            GameInstance[] objs = FindObjectsOfType<GameInstance>();

            if (objs.Length > 1)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        private void SaveGame()
        {
            PlayerPrefs.SetInt("HiScore", _score);
            PlayerPrefs.Save();
        }

        public void SetHiScore(int newScore)
        {
            _score = newScore;
            SaveGame();
        }

        public int GetHiScore()
        {
            return _score;
        }

        private void Start()
        {
            Application.targetFrameRate = 120;
        }

        public void ToggleRequestRestart(bool value)
        {
            _requestedRestart = value;
        }

        public bool ReturnRestartValue()
        {
            return _requestedRestart;
        }
    }
}
