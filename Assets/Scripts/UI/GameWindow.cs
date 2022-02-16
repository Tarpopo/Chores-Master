using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class GameWindow : CanvasGroupBased
    {
        [Space]
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private Image progressImage;
        [SerializeField] private MyButton restartButton;
        public MyButton RestartButton => restartButton;
        sealed public override void Enable(bool value, float delay = -1, float duration = -1)
        {
            base.Enable(value, delay, duration);
            if (!enabled) return;
            UpdateLevelText();
            ResetProgressBar(); 
        }
        public void UpdateProgressBar(LevelProgress levelProgress)
        {
            progressImage.fillAmount = levelProgress.Progress;
        }
        public void ResetProgressBar()
        {
            progressImage.fillAmount = 0f;
        }
        private void UpdateLevelText()
        {
            currentLevelText.text = string.Format("LEVEL {0}", Statistics.PlayerLevel-1);
        }
    }
}