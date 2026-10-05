using System;
using gishadev.eclipse.Core.Events;
using gishadev.eclipse.Core.Input;
using gishadev.eclipse.Gameplay.Salvage;
using gishadev.eclipse.Visuals;
using gishadev.tools.Audio;
using gishadev.tools.Events;
using UnityEngine;
using VContainer.Unity;

namespace gishadev.eclipse.Gameplay.GameFlow
{
    public enum RoundState
    {
        WaitingToStart,
        Playing,
        Won,
        Lost,
    }

    /// <summary>
    /// Owns the round: win when every part of the <see cref="Vehicle"/> is salvaged,
    /// lose when <see cref="GameConfig.RoundDuration"/> runs out. Starts on the first StartPressed
    /// (<see cref="RoundStartedEvent"/>), ends once (<see cref="RoundEndedEvent"/>).
    /// </summary>
    public class GameController : IStartable, ITickable, IDisposable
    {
        private readonly Vehicle _vehicle;
        private readonly GameConfig _config;
        private readonly IEventBus _eventBus;
        private readonly IInputService _input;
        private readonly IAudioManager _audioManager;
        private readonly PlatformAnimationHandler _platformAnimation;
        private IDisposable _partSalvagedSubscription;

        public RoundState State { get; private set; }
        public float RemainingTime { get; private set; }
        public float RoundDuration => _config.RoundDuration;
        public int RemainingParts { get; private set; }

        public GameController(Vehicle vehicle, GameConfig config, IEventBus eventBus, IInputService input,
            IAudioManager audioManager, PlatformAnimationHandler platformAnimation)
        {
            _vehicle = vehicle;
            _config = config;
            _eventBus = eventBus;
            _input = input;
            _audioManager = audioManager;
            _platformAnimation = platformAnimation;
        }

        // Round is prepared but waits for StartPressed (press-to-start popup); timer doesn't run yet.
        public void Start()
        {
            State = RoundState.WaitingToStart;
            RemainingTime = _config.RoundDuration;
            RemainingParts = CountParts();

            // Input lives in the Project scope, so a previous round may have left it in any state.
            _input.SetGameInputEnabled(false);
            _input.StartPressed += OnStartPressed;
            _partSalvagedSubscription = _eventBus.Subscribe<PartSalvagedEvent>(OnPartSalvaged);

            if (RemainingParts == 0)
                Debug.LogWarning($"{nameof(GameController)}: vehicle has no parts assigned — round can't be won.", _vehicle);
            
            _audioManager.PlayMusic(MusicAudioEnum.GAME);
        }

        public void Tick()
        {
            if (State != RoundState.Playing)
                return;

            RemainingTime = Mathf.Max(0f, RemainingTime - Time.deltaTime);
            if (RemainingTime <= 0f)
            {
                _audioManager.PlaySFX(SFXAudioEnum.CRUSHING);
                _platformAnimation.PlayCrush();
                EndRound(RoundState.Lost);
            }
        }

        public void Dispose()
        {
            _input.StartPressed -= OnStartPressed;
            _partSalvagedSubscription?.Dispose();
        }

        private void OnStartPressed()
        {
            if (State != RoundState.WaitingToStart)
                return;

            _input.StartPressed -= OnStartPressed;
            State = RoundState.Playing;
            _input.SetGameInputEnabled(true);
            _eventBus.Fire(new RoundStartedEvent());
        }

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
