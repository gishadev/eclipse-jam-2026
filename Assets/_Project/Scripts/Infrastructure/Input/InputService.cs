using System;
using System.Collections.Generic;
using gishadev.eclipse.Core.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace gishadev.eclipse.Infrastructure.Input
{
    /// <summary>
    /// Wraps the generated <see cref="EclipseInput"/> actions. Owns the instance: enables it on build,
    /// disposes it with the scope.
    /// </summary>
    public class InputService : IInputService, IInitializable, IDisposable
    {
        private readonly EclipseInput _input = new();
        private readonly List<RaycastResult> _uiHits = new();

        public Vector2 OrbitalMovement => _input.Game.OrbitalMovement.ReadValue<Vector2>();
        public float OrbitalZooming => _input.Game.OrbitalZooming.ReadValue<float>();
        public Vector2 OrbitalLook => _input.Game.OrbitalLook.ReadValue<Vector2>();
        public bool IsOrbitalLookHeld => _input.Game.OrbitalLookHold.IsPressed();
        public float OrbitalScroll => _input.Game.OrbitalScroll.ReadValue<float>();
        public Vector2 PointerPosition => _input.Game.PointerPosition.ReadValue<Vector2>();
        public bool IsGameInputEnabled => _input.Game.enabled;

        public event Action PausePressed;
        public event Action StartPressed;
        public event Action SalvagePressed;

        public void Initialize()
        {
            _input.General.Pause.performed += OnPausePerformed;
            _input.General.Start.performed += OnStartPerformed;
            _input.Game.Salvage.performed += OnSalvagePerformed;
            _input.Enable();
        }

        // Raycasts the EventSystem directly: IsPointerOverGameObject is unreliable inside Input System callbacks.
        public bool IsPointerOverUI()
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
                return false;

            var pointer = new PointerEventData(eventSystem) { position = PointerPosition };
            _uiHits.Clear();
            eventSystem.RaycastAll(pointer, _uiHits);
            return _uiHits.Count > 0;
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
            _input.General.Start.performed -= OnStartPerformed;
            _input.Game.Salvage.performed -= OnSalvagePerformed;
            _input.Disable();
            _input.Dispose();
        }

        private void OnPausePerformed(InputAction.CallbackContext _) => PausePressed?.Invoke();
        private void OnStartPerformed(InputAction.CallbackContext _) => StartPressed?.Invoke();
        private void OnSalvagePerformed(InputAction.CallbackContext _) => SalvagePressed?.Invoke();
    }
}
