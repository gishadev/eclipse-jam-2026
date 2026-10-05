using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.eclipse.GUI
{
    /// <summary>Passive view over the SFX/Music settings popup. Driven by <see cref="SettingsPresenter"/>.</summary>
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private GameObject settingsPopup;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;

        [Header("SFX")]
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private TMP_Text sfxAmount;

        [Header("Music")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private TMP_Text musicAmount;

        public event Action OpenClicked;
        public event Action CloseClicked;
        /// <summary>Normalized 0..1, whatever the slider's min/max are.</summary>
        public event Action<float> SfxChanged;
        public event Action<float> MusicChanged;

        public void SetVisible(bool visible) => settingsPopup.SetActive(visible);

        /// <summary>Updates sliders + labels without raising the changed events.</summary>
        public void SetVolumes(float sfx01, float music01)
        {
            sfxSlider.SetValueWithoutNotify(Mathf.Lerp(sfxSlider.minValue, sfxSlider.maxValue, sfx01));
            musicSlider.SetValueWithoutNotify(Mathf.Lerp(musicSlider.minValue, musicSlider.maxValue, music01));
            sfxAmount.text = FormatPercent(sfx01);
            musicAmount.text = FormatPercent(music01);
        }

        private void OnEnable()
        {
            openButton.onClick.AddListener(OnOpenClicked);
            closeButton.onClick.AddListener(OnCloseClicked);
            sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }

        private void OnDisable()
        {
            openButton.onClick.RemoveListener(OnOpenClicked);
            closeButton.onClick.RemoveListener(OnCloseClicked);
            sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
            musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
        }

        private void OnOpenClicked() => OpenClicked?.Invoke();
        private void OnCloseClicked() => CloseClicked?.Invoke();
        private void OnSfxSliderChanged(float _) => SfxChanged?.Invoke(sfxSlider.normalizedValue);
        private void OnMusicSliderChanged(float _) => MusicChanged?.Invoke(musicSlider.normalizedValue);

        private static string FormatPercent(float value01) => $"{Mathf.RoundToInt(value01 * 100f)}%";
    }
}
