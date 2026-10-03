using gishadev.eclipse.Core.Input;
using Unity.Cinemachine;
using UnityEngine;
using VContainer.Unity;

namespace gishadev.eclipse.Gameplay.OrbitalCamera
{
    /// <summary>
    /// Orbits the Cinemachine camera around its tracking target (the camera rig) by driving
    /// <see cref="CinemachineOrbitalFollow"/> axes. Range/wrap limits come from the axes set on the component.
    /// WASD / RMB-drag → Horizontal + Vertical axes, R/F / scroll → RadialAxis.
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

            // Keyboard: rate-based (per second). Mouse: delta-based (per pixel / per notch), no dt.
            // Each source has its own invert flags.
            Vector2 orbit = _input.OrbitalMovement * new Vector2(_config.HorizontalSpeed, _config.VerticalSpeed) * dt
                * Sign(_config.InvertHorizontal, _config.InvertVertical);
            if (_input.IsOrbitalLookHeld)
                orbit += _input.OrbitalLook * _config.MouseOrbitSensitivity
                    * Sign(_config.InvertMouseHorizontal, _config.InvertMouseVertical);
            Orbit(orbit);

            // Scroll up (+) zooms in, i.e. negative radial step — same sign as R.
            float zoom = _input.OrbitalZooming * _config.ZoomSpeed * dt * Sign(_config.InvertZoom)
                - _input.OrbitalScroll * _config.ScrollZoomStep * Sign(_config.InvertScroll);
            Zoom(zoom);
        }

        private void Orbit(Vector2 degrees)
        {
            if (degrees == Vector2.zero)
                return;

            ref InputAxis h = ref _orbitalFollow.HorizontalAxis;
            h.Value = h.ClampValue(h.Value + degrees.x);

            ref InputAxis v = ref _orbitalFollow.VerticalAxis;
            v.Value = v.ClampValue(v.Value + degrees.y);
        }

        // RadialAxis scales the orbit radius. Multiplicative so zoom feels the same at any distance.
        // Positive step moves away (F), negative moves closer (R / scroll up).
        private void Zoom(float step)
        {
            if (step == 0f)
                return;

            ref InputAxis r = ref _orbitalFollow.RadialAxis;
            r.Value = r.ClampValue(r.Value * Mathf.Exp(step));
        }

        private static float Sign(bool invert) => invert ? -1f : 1f;
        private static Vector2 Sign(bool invertX, bool invertY) => new(Sign(invertX), Sign(invertY));
    }
}
