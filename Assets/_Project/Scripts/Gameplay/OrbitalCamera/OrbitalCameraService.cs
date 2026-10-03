using EclipseJam.Core.Input;
using Unity.Cinemachine;
using UnityEngine;
using VContainer.Unity;

namespace EclipseJam.Gameplay.OrbitalCamera
{
    /// <summary>
    /// Orbits the Cinemachine camera around its tracking target (the camera rig) by driving
    /// <see cref="CinemachineOrbitalFollow"/> axes. Range/wrap limits come from the axes set on the component.
    /// Zoom → RadialAxis, mouse orbit → same axes from another input source.
    /// </summary>
    public class OrbitalCameraService : ITickable
    {
        private readonly IInputService _input;
        private readonly CinemachineOrbitalFollow _orbitalFollow;
        private readonly OrbitalCameraConfig _config;

        public OrbitalCameraService(IInputService input, CinemachineOrbitalFollow orbitalFollow,
            OrbitalCameraConfig config)
        {
            _input = input;
            _orbitalFollow = orbitalFollow;
            _config = config;
        }

        public void Tick()
        {
            float dt = Time.deltaTime;
            Orbit(_input.OrbitalMovement, dt);
            Zoom(_input.OrbitalZooming, dt);
        }

        private void Orbit(Vector2 move, float dt)
        {
            if (move == Vector2.zero)
                return;

            float horizontal = move.x * _config.HorizontalSpeed * dt * (_config.InvertHorizontal ? -1f : 1f);
            float vertical = move.y * _config.VerticalSpeed * dt * (_config.InvertVertical ? -1f : 1f);

            ref InputAxis h = ref _orbitalFollow.HorizontalAxis;
            h.Value = h.ClampValue(h.Value + horizontal);

            ref InputAxis v = ref _orbitalFollow.VerticalAxis;
            v.Value = v.ClampValue(v.Value + vertical);
        }

        // RadialAxis scales the orbit radius. Multiplicative so zoom feels the same at any distance.
        // Positive input (F) moves away, negative (R) moves closer.
        private void Zoom(float zoom, float dt)
        {
            if (zoom == 0f)
                return;

            float step = zoom * _config.ZoomSpeed * dt * (_config.InvertZoom ? -1f : 1f);

            ref InputAxis r = ref _orbitalFollow.RadialAxis;
            r.Value = r.ClampValue(r.Value * Mathf.Exp(step));
        }
    }
}
