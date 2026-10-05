using System;
using gishadev.eclipse.Core.Events;
using gishadev.tools.Events;
using VContainer.Unity;

namespace gishadev.eclipse.GUI
{
    /// <summary>Shows the press-to-start popup on scene load, hides it on <see cref="RoundStartedEvent"/>.</summary>
    public class StartPresenter : IStartable, IDisposable
    {
        private readonly StartView _view;
        private readonly IEventBus _eventBus;
        private IDisposable _roundStartedSubscription;

        public StartPresenter(StartView view, IEventBus eventBus)
        {
            _view = view;
            _eventBus = eventBus;
        }

        public void Start()
        {
            _view.SetVisible(true);
            _roundStartedSubscription = _eventBus.Subscribe<RoundStartedEvent>(_ => _view.SetVisible(false));
        }

        public void Dispose() => _roundStartedSubscription?.Dispose();
    }
}
