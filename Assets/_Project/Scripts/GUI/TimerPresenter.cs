using gishadev.eclipse.Gameplay.GameFlow;
using VContainer.Unity;

namespace gishadev.eclipse.GUI
{
    /// <summary>Mirrors the round timer of <see cref="GameController"/> into <see cref="TimerView"/> every frame.</summary>
    public class TimerPresenter : ITickable
    {
        private readonly TimerView _view;
        private readonly GameController _gameController;

        public TimerPresenter(TimerView view, GameController gameController)
        {
            _view = view;
            _gameController = gameController;
        }

        public void Tick() => _view.SetTimer(_gameController.RemainingTime, _gameController.RoundDuration);
    }
}
