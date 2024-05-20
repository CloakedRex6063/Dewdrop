using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        private GameManager _gm;
        private bool _mute;
        public Sprite unMuteSprite;
        public Sprite muteSprite;
        public Image audioIconComponent;
        public Sprite onSprite;
        public Sprite offSprite;
        public Image adsIconComponent;
        public TextMeshProUGUI adsTextComponent;
        public TextMeshProUGUI hiScoreText;
        public Image hiScoreImage;

        private void Awake()
        {
            _gm = FindObjectOfType<GameManager>();
        }

        public void Play()
        {
            _gm.StartGame();
        }

        public void Mute()
        {
            _gm.Mute(unMuteSprite,muteSprite,audioIconComponent);
        }

        public void AdsToggle()
        {
            _gm.AdToggle(adsIconComponent,onSprite,offSprite,adsTextComponent);
        }
    }
}
