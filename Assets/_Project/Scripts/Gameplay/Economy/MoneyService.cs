using System;
using gishadev.eclipse.Core.Events;
using gishadev.tools.Audio;
using gishadev.tools.Events;
using VContainer.Unity;

namespace gishadev.eclipse.Gameplay.Economy
{
    /// <summary>
    /// Owns the player's money. App-lifetime (Project scope), so it survives scene reloads.
    /// Earns from <see cref="PartSalvagedEvent"/>, announces changes with <see cref="MoneyChangedEvent"/>.
    /// </summary>
    public class MoneyService : IInitializable, IDisposable
    {
        private readonly IEventBus _eventBus;
        private readonly IAudioManager _audioManager;
        private IDisposable _partSalvagedSubscription;

        public int CurrentMoney { get; private set; }

        public MoneyService(IEventBus eventBus, IAudioManager audioManager)
        {
            _eventBus = eventBus;
            _audioManager = audioManager;
        }

        public void Initialize()
        {
            _partSalvagedSubscription = _eventBus.Subscribe<PartSalvagedEvent>(OnPartSalvaged);
        }

        public void Dispose()
        {
            _partSalvagedSubscription?.Dispose();
        }

        private void OnPartSalvaged(PartSalvagedEvent e)
        {
            CurrentMoney += e.Price;
            _eventBus.Fire(new MoneyChangedEvent(CurrentMoney));
            _audioManager.PlaySFX(SFXAudioEnum.CASH_REGISTER);
        }
    }
}