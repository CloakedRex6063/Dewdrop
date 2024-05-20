using System;
using System.Collections;
using Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public enum GameStates
    {
        MainMenu,
        Gameplay,
        GameOver,
        Paused
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameStates _currentState;
        // Coroutine to flicker the transition outline
        public Coroutine outlineRoutine = null;
        public Coroutine pcOutlineRoutine = null;
        private Ads _ads;
        private InputManager _iM;
        private GameInstance _gI;
        private PC _pc;
        private UIManager _uM;
        private AudioManager _aM;
        private ProceduralManager _pM;
        public bool transition;
        [SerializeField] 
        private float startTimer;
        [SerializeField]
        private TextMeshProUGUI score;
        [SerializeField] 
        private ParticleSystem normalDeathVFX;
        [SerializeField]
        private GameObject normalLighting;
        [SerializeField]
        private GameObject alienLighting;

        private void Awake()
        {
            _ads = FindObjectOfType<Ads>();
            _iM = GetComponent<InputManager>();
            _uM = GetComponent<UIManager>();
            _aM = GetComponent<AudioManager>();
            _pM = GetComponent<ProceduralManager>();
            _gI = FindObjectOfType<GameInstance>();
            _pc = FindObjectOfType<PC>();
        }

        private void Start()
        {
            _ads.LoadInterstitialAd();
            ToggleLighting();
            if (_gI.ReturnRestartValue())
            {
                _gI.ToggleRequestRestart(false);
                StartGame();
            }
            else
            {
                StartCoroutine(ShowMainMenuTimer());
            }
        }

        private void FixedUpdate()
        {
            if(!_pc)
                return;
            _pc.SwitchMaterial(GetWorld());
        }

        private void Update()
        {
            if(!_pc)
                return;
            score.text = _pc.CalculateScore().ToString();
        }

        public void ToggleLighting()
        {
            switch (GetWorld())
            {
                case ProceduralManager.World.Normal:
                    normalLighting.SetActive(true);
                    alienLighting.SetActive(false);
                    break;
                case ProceduralManager.World.Alien:
                    alienLighting.SetActive(true);
                    normalLighting.SetActive(false);
                    break;
            }
        }
        
        private void SwitchGameState(GameStates newState)
        {
            _currentState = newState;
            switch (_currentState)
            {
                case GameStates.MainMenu:
                    _iM.AllowInput(false);
                    _ads.LoadInterstitialAd();
                    Time.timeScale = 0;
                    _uM.SetHiScore(_gI.GetHiScore());
                    break;
                case GameStates.Gameplay:
                    _iM.AllowInput(true);
                    _ads.LoadInterstitialAd();
                    _uM.TurnOffAllUI();
                    Time.timeScale = 1;
                    break;
                case GameStates.GameOver:
                    if (_gI.ads)
                    {
                        _ads.ShowInterstitialAd();  
                    }
                    _iM.AllowInput(false);
                    _uM.SetGameOverMenuActive();
                    _uM.SetScore(_pc.CalculateScore());
                    _uM.SetHiScore(_pc.CalculateScore() > _gI.GetHiScore() ?_pc.CalculateScore() : _gI.GetHiScore());
                    _gI.SetHiScore(Mathf.RoundToInt(_pc.CalculateScore() > _gI.GetHiScore() ?_pc.CalculateScore() : _gI.GetHiScore()));
                    _aM.Play(SoundType.Death);
                    SoundType type = _pM.currentWorld == ProceduralManager.World.Normal
                        ? SoundType.NatureAmbient
                        : SoundType.AlienAmbient;
                    _aM.FadeOut(type,0.5f);
                    StartCoroutine(DeathSlowdown());
                    break;
                case GameStates.Paused:
                    _iM.AllowInput(false);
                    _uM.SetPauseMenuActive();
                    if (_gI.ads)
                    {
                        _ads.ShowInterstitialAd();  
                    }
                    Time.timeScale = 0;
                    break;
                default:
                    _iM.AllowInput(false);
                    break;
            }
        }

        private IEnumerator DeathSlowdown()
        {
            Time.timeScale = 0.4f;
            yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 0f;
        }
        public void Mute(Sprite unMuteSprite,Sprite muteSprite, Image audioIconComponent)
        {
            _gI.mute= !_gI.mute;
            if (_gI.mute)
            {
                _aM.Mute();
                audioIconComponent.sprite = muteSprite;
            }
            else
            {
                _aM.UnMute();
                audioIconComponent.sprite = unMuteSprite;
            }
        }

        public void AdToggle(Image adsIconComponent, Sprite onSprite, Sprite offSprite, TextMeshProUGUI adsTextComponent)
        {
            _gI.ads = !_gI.ads;
            adsIconComponent.sprite = _gI.ads ? onSprite: offSprite;
            adsIconComponent.rectTransform.anchoredPosition = _gI.ads ? new Vector2(40,0) : new Vector2(-40,0);
            adsTextComponent.text = _gI.ads ? "Ads Enabled" : "Ads Disabled";
        }
        
        public void MuteCheck(Sprite unMuteSprite,Sprite muteSprite, Image audioIconComponent)
        {
            if (_gI.mute)
            {
                _aM.Mute();
                audioIconComponent.sprite = muteSprite;
            }
            else
            {
                _aM.UnMute();
                audioIconComponent.sprite = unMuteSprite;
            }
        }
        
        public void AdToggleCheck(Image adsIconComponent, Sprite onSprite, Sprite offSprite, TextMeshProUGUI adsTextComponent)
        {
            adsIconComponent.sprite = _gI.ads ? onSprite: offSprite;
            adsIconComponent.rectTransform.anchoredPosition = _gI.ads ? new Vector2(40,0) : new Vector2(-40,0);
            adsTextComponent.text = _gI.ads ? "Ads Enabled" : "Ads Disabled";
        }
        public void PauseGame()
        {
            SwitchGameState(GameStates.Paused);
        }
    
        public void UnPauseGame()
        {
            _uM.SetPauseMenuInactive();
            StartGame();
        }

        public void GameOver()
        {
            SwitchGameState(GameStates.GameOver);
            PlayDeathVfx();
            Destroy(_pc.gameObject);
            _uM.SetGameOverMenuActive();
        }
        
        public void ShowMainMenu()
        {
            SwitchGameState(GameStates.MainMenu);
            _uM.SetMainMenuActive();
        }
        
        public void Replay()
        {
            _gI.ToggleRequestRestart(true);
        }

        public void StartGame()
        {
            SwitchGameState(GameStates.Gameplay);
        }

        public IEnumerator ShowMainMenuTimer()
        {
            yield return new WaitForSeconds(0.01f);
            ShowMainMenu();
        }

        public void Bounce()
        {
            _aM.Play(SoundType.Bounce);
        }

        public ProceduralManager.World GetWorld()
        {
            return _pM.currentWorld;
        }

        public void SwapWorlds()
        {
            _pM.SwapWorlds();
        }

        void PlayDeathVfx()
        {
            ParticleSystem vfx;
            vfx = Instantiate(normalDeathVFX);

            // Set the position of the VFX
            vfx.transform.position = _pc.transform.position;
            // Play the VFX
            vfx.Play();
            Destroy (vfx.gameObject, vfx.main.duration); 
        }
    }
}