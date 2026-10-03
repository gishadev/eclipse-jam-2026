using System;
using gishadev.eclipse.Core.Events;
using gishadev.eclipse.Gameplay.Economy;
using gishadev.tools.Events;
using VContainer.Unity;

namespace gishadev.eclipse.GUI
{
    /// <summary>Keeps <see cref="MoneyView"/> in sync with <see cref="MoneyService"/>.</summary>
    public class MoneyPresenter : IStartable, IDisposable
    {
        private readonly MoneyView _view;
        private readonly MoneyService _moneyService;
        private readonly IEventBus _eventBus;
        private IDisposable _moneyChangedSubscription;

        public MoneyPresenter(MoneyView view, MoneyService moneyService, IEventBus eventBus)
        {
            _view = view;
            _moneyService = moneyService;
            _eventBus = eventBus;
        }

        public void Start()
        {
            _view.SetMoney(_moneyService.CurrentMoney);
            _moneyChangedSubscription = _eventBus.Subscribe<MoneyChangedEvent>(e => _view.SetMoney(e.CurrentMoney));
        }

        public void Dispose() => _moneyChangedSubscription?.Dispose();
    }
}
