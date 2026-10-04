using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.eclipse.GUI
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeLabel;
        [SerializeField] private Image fillImage;

        private int _shownSeconds = -1;

        public void SetTimer(float currentTime, float maxTime)
        {
            fillImage.fillAmount = maxTime > 0f ? Mathf.Clamp01(currentTime / maxTime) : 0f;

            // Ceil: shows 0:01 until time is really up. Text only rebuilt when the second changes.
            int seconds = Mathf.CeilToInt(Mathf.Max(0f, currentTime));
            if (seconds == _shownSeconds)
                return;

            _shownSeconds = seconds;
            timeLabel.text = $"{seconds / 60}:{seconds % 60:00}";
        }
    }
}
