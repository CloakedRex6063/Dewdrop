using Main;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class InvincibilityProgress : MonoBehaviour
    {
        private Image _progressBar;
        [SerializeField]
        private Image icon;
        private Powerup _powerupComponent;


        private void Awake()
        {
            _powerupComponent = FindObjectOfType<Powerup>();
            _progressBar = GetComponent<Image>();
        }

        private void FixedUpdate()
        {
            ToggleVisibility(_powerupComponent.GetInvincibilityProgress() != 0);
            UpdateInvincibilityProgressBar();
        }

        public void UpdateInvincibilityProgressBar()
        {
            _progressBar.fillAmount = _powerupComponent.GetInvincibilityProgress();
        }
        
        private void ToggleVisibility(bool toggle)
        {
            _progressBar.enabled = toggle;
            icon.enabled = toggle;
        }
    }
}
