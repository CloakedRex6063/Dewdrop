using Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class ScoreProgress : MonoBehaviour
    {
        private Image _progressBar;
        private Powerup _powerupComponent;
        [SerializeField] 
        private TextMeshProUGUI multiplier;
        [SerializeField] 
        private TextMeshProUGUI x;


        private void Awake()
        {
            _powerupComponent = FindObjectOfType<Powerup>();
            _progressBar = GetComponent<Image>();
        }

        private void FixedUpdate()
        {
            ToggleVisibility(_powerupComponent.GetScoreProgress() != 0);
            UpdateScoreProgressBar();
            UpdateScoreMultiplier();
        }

        private void UpdateScoreProgressBar()
        {
            _progressBar.fillAmount = _powerupComponent.GetScoreProgress();
        }

        private void UpdateScoreMultiplier()
        {
            multiplier.text = _powerupComponent.scoreMultiplier.ToString();
        }

        private void ToggleVisibility(bool toggle)
        {
            _progressBar.enabled = toggle;
            multiplier.enabled = toggle;
            x.enabled = toggle;
        }
    }
}
