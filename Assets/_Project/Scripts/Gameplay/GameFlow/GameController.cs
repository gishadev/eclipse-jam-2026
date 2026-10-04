using System;
using gishadev.eclipse.Core.Events;
using gishadev.eclipse.Core.Input;
using gishadev.eclipse.Gameplay.Salvage;
using gishadev.tools.Events;
using UnityEngine;
using VContainer.Unity;

namespace gishadev.eclipse.Gameplay.GameFlow
{
    public enum RoundState
    {
        Playing,
        Won,
        Lost,
    }

    /// <summary>
    /// Owns the round: win when every part of the <see cref="Vehicle"/> is salvaged,
    /// lose when <see cref="GameConfig.RoundDuration"/> runs out. Fires <see cref="RoundEndedEvent"/> once.
    /// </summary>
    public class GameController : IStartable, ITickable, IDisposable
    {
        private readonly Vehicle _vehicle;
        private readonly GameConfig _config;
        private readonly IEventBus _eventBus;
        private readonly IInputService _input;
        private IDisposable _partSalvagedSubscription;

        public RoundState State { get; private set; }
        public float RemainingTime { get; private set; }
        public int RemainingParts { get; private set; }

        public GameController(Vehicle vehicle, GameConfig config, IEventBus eventBus, IInputService input)
        {
            _vehicle = vehicle;
            _config = config;
            _eventBus = eventBus;
            _input = input;
        }

        public void Start()
        {
            State = RoundState.Playing;
            RemainingTime = _config.RoundDuration;
            RemainingParts = CountParts();

            // Input lives in the Project scope, so a previous round may have left it disabled.
            _input.SetGameInputEnabled(true);
            _partSalvagedSubscription = _eventBus.Subscribe<PartSalvagedEvent>(OnPartSalvaged);

            if (RemainingParts == 0)
                Debug.LogWarning($"{nameof(GameController)}: vehicle has no parts assigned — round can't be won.", _vehicle);
        }

        public void Tick()
        {
            if (State != RoundState.Playing)
                return;

            RemainingTime = Mathf.Max(0f, RemainingTime - Time.deltaTime);
            if (RemainingTime <= 0f)
                EndRound(RoundState.Lost);
        }

        public void Dispose() => _partSalvagedSubscription?.Dispose();

        // Counted via events, not by polling the vehicle: Destroy is deferred, so a just-salvaged
        // part is still alive in the frame it's salvaged.
        private void OnPartSalvaged(PartSalvagedEvent _)
        {
            if (State != RoundState.Playing)
                return;

            RemainingParts--;
            if (RemainingParts <= 0)
                EndRound(RoundState.Won);
        }

        private void EndRound(RoundState result)
        {
            State = result;
            _input.SetGameInputEnabled(false);
            _eventBus.Fire(new RoundEndedEvent(result == RoundState.Won));
            Debug.Log($"Round ended: {result}");
        }

        private int CountParts()
        {
            int count = 0;
            foreach (Part part in _vehicle.Parts)
                if (part != null)
                    count++;
            return count;
        }
    }
}
