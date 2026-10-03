using System;
using EclipseJam.Core.Input;
using gishadev.eclipse;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace EclipseJam.Infrastructure.Input
{
    /// <summary>
    /// Wraps the generated <see cref="EclipseInput"/> actions. Owns the instance: enables it on build,
    /// disposes it with the scope.
    /// </summary>
    public class InputService : IInputService, IInitializable, IDisposable
    {
        private readonly EclipseInput _input = new();

        public Vector2 OrbitalMovement => _input.Game.OrbitalMovement.ReadValue<Vector2>();
        public float OrbitalZooming => _input.Game.OrbitalZooming.ReadValue<float>();
        public bool IsGameInputEnabled => _input.Game.enabled;

        public event Action PausePressed;

        public void Initialize()
        {
            _input.General.Pause.performed += OnPausePerformed;
            _input.Enable();
        }

        public void SetGameInputEnabled(bool enabled)
        {
            if (enabled)
                _input.Game.Enable();
            else
                _input.Game.Disable();
        }

        public void Dispose()
        {
            _input.General.Pause.performed -= OnPausePerformed;
            _input.Disable();
            _input.Dispose();
        }

        private void OnPausePerformed(InputAction.CallbackContext _) => PausePressed?.Invoke();
    }
}
