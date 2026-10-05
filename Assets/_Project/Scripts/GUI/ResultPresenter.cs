using System;
using Cysharp.Threading.Tasks;
using gishadev.eclipse.Core.Events;
using gishadev.tools.Events;
using gishadev.tools.SceneLoading;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace gishadev.eclipse.GUI
{
    /// <summary>
    /// Shows the win / lose popup on <see cref="RoundEndedEvent"/>; Restart reloads the current scene
    /// (with the scene loader's fade), which rebuilds the Game scope and starts a fresh round.
    /// </summary>
    public class ResultPresenter : IStartable, IDisposable
    {
        private readonly ResultView _view;
        private readonly IEventBus _eventBus;
        private readonly ISceneLoader _sceneLoader;
        private IDisposable _roundEndedSubscription;

        public ResultPresenter(ResultView view, IEventBus eventBus, ISceneLoader sceneLoader)
        {
            _view = view;
            _eventBus = eventBus;
            _sceneLoader = sceneLoader;
        }

        public void Start()
        {
            _view.Hide();
            _view.RestartClicked += OnRestartClicked;
            _roundEndedSubscription = _eventBus.Subscribe<RoundEndedEvent>(e => _view.ShowResult(e.IsWin));
        }

        public void Dispose()
        {
            _view.RestartClicked -= OnRestartClicked;
            _roundEndedSubscription?.Dispose();
        }

        // SceneLoader ignores repeated calls while a load is running, so double clicks are safe.
        private void OnRestartClicked() =>
            _sceneLoader.LoadScene(SceneManager.GetActiveScene().name).Forget();
    }
}
