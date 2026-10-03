using System;
using gishadev.eclipse.Core.Events;
using gishadev.tools.Events;
using VContainer.Unity;

namespace gishadev.eclipse.Gameplay.Economy
{
    /// <summary>
    /// Owns the player's money. App-lifetime (Project scope), so it survives scene reloads.
    /// Earns from <see cref="PartSalvagedEvent"/>.
    /// </summary>
    public class MoneyService : IInitializable, IDisposable
    {
        private readonly IEventBus _eventBus;
        private IDisposable _partSalvagedSubscription;

        public int CurrentMoney { get; private set; }

        public MoneyService(IEventBus eventBus) => _eventBus = eventBus;

        public void Initialize() =>
            _partSalvagedSubscription = _eventBus.Subscribe<PartSalvagedEvent>(OnPartSalvaged);

        public void Dispose() => _partSalvagedSubscription?.Dispose();

        private void OnPartSalvaged(PartSalvagedEvent e) => CurrentMoney += e.Price;
    }
}
