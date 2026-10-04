using gishadev.tools.Events;

namespace gishadev.eclipse.Core.Events
{
    /// <summary>Fired once by the game controller when the round is won or lost.</summary>
    public readonly struct RoundEndedEvent : IEvent
    {
        public readonly bool IsWin;

        public RoundEndedEvent(bool isWin) => IsWin = isWin;
    }
}
