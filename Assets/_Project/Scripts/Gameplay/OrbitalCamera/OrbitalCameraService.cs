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
            Vector2 move = _input.OrbitalMovement;
            if (move == Vector2.zero)
                return;

            float dt = Time.deltaTime;
            float horizontal = move.x * _config.HorizontalSpeed * dt * (_config.InvertHorizontal ? -1f : 1f);
            float vertical = move.y * _config.VerticalSpeed * dt * (_config.InvertVertical ? -1f : 1f);

            ref InputAxis h = ref _orbitalFollow.HorizontalAxis;
            h.Value = h.ClampValue(h.Value + horizontal);

            ref InputAxis v = ref _orbitalFollow.VerticalAxis;
            v.Value = v.ClampValue(v.Value + vertical);
        }
    }
}
