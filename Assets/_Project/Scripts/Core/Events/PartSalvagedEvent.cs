using gishadev.tools.Events;

namespace gishadev.eclipse.Core.Events
{
    /// <summary>Fired by the salvage service after a part is removed from a vehicle.</summary>
    public readonly struct PartSalvagedEvent : IEvent
    {
        public readonly int Price;

        public PartSalvagedEvent(int price) => Price = price;
    }
}
