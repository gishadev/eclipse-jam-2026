using System;
using gishadev.tools.Audio;
using VContainer.Unity;

namespace gishadev.eclipse.GUI
{
    /// <summary>
    /// Opens/closes the settings popup and binds its sliders to <see cref="IAudioManager"/> SFX/Music volume.
    /// Volumes live in the Project scope, so they survive scene reloads.
    /// </summary>
    public class SettingsPresenter : IStartable, IDisposable
    {
        private readonly SettingsView _view;
        private readonly IAudioManager _audioManager;

        public SettingsPresenter(SettingsView view, IAudioManager audioManager)
        {
            _view = view;
            _audioManager = audioManager;
        }

        public void Start()
        {
            _view.SetVisible(false);
            RefreshView();

            _view.OpenClicked += OnOpenClicked;
            _view.CloseClicked += OnCloseClicked;
            _view.SfxChanged += _audioManager.SetSFXVolume;
            _view.MusicChanged += _audioManager.SetMusicVolume;
            _audioManager.VolumeChanged += RefreshView;
        }

        public void Dispose()
        {
            _view.OpenClicked -= OnOpenClicked;
            _view.CloseClicked -= OnCloseClicked;
            _view.SfxChanged -= _audioManager.SetSFXVolume;
            _view.MusicChanged -= _audioManager.SetMusicVolume;
            _audioManager.VolumeChanged -= RefreshView;
        }

        private void OnOpenClicked()
        {
            RefreshView();
            _view.SetVisible(true);
        }

        private void OnCloseClicked() => _view.SetVisible(false);

        // AudioManager is the source of truth; labels/sliders follow its VolumeChanged.
        private void RefreshView() =>
            _view.SetVolumes(_audioManager.SFXVolumePercentage, _audioManager.MusicVolumePercentage);
    }
}
