using gishadev.tools.Events;

namespace gishadev.eclipse.Core.Events
{
    /// <summary>Fired by the money service after the balance changes.</summary>
    public readonly struct MoneyChangedEvent : IEvent
    {
        public readonly int CurrentMoney;

        public MoneyChangedEvent(int currentMoney) => CurrentMoney = currentMoney;
    }
}
