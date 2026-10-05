using System;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.eclipse.GUI
{
    /// <summary>Passive view over the win / lose popups. Driven by <see cref="ResultPresenter"/>.</summary>
    public class ResultView : MonoBehaviour
    {
        [SerializeField] private GameObject winPopup;
        [SerializeField] private GameObject lostPopup;
        [SerializeField] private Button winRestartButton;
        [SerializeField] private Button lostRestartButton;

        public event Action RestartClicked;

        public void Hide()
        {
            winPopup.SetActive(false);
            lostPopup.SetActive(false);
        }

        public void ShowResult(bool isWin)
        {
            winPopup.SetActive(isWin);
            lostPopup.SetActive(!isWin);
        }

        private void OnEnable()
        {
            winRestartButton.onClick.AddListener(OnRestartClicked);
            lostRestartButton.onClick.AddListener(OnRestartClicked);
        }

        private void OnDisable()
        {
            winRestartButton.onClick.RemoveListener(OnRestartClicked);
            lostRestartButton.onClick.RemoveListener(OnRestartClicked);
        }

        private void OnRestartClicked() => RestartClicked?.Invoke();
    }
}
