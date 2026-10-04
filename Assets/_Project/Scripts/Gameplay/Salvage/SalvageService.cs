using System;
using System.Collections.Generic;
using gishadev.eclipse.Core.Events;
using gishadev.eclipse.Core.Input;
using gishadev.tools.Events;
using PrimeTween;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace gishadev.eclipse.Gameplay.Salvage
{
    /// <summary>
    /// On salvage click, raycasts from the main camera through the pointer:
    /// a <see cref="Bolt"/> gets unscrewed, a loose <see cref="Part"/> gets stripped out (PrimeTween, then destroyed)
    /// and fires <see cref="PartSalvagedEvent"/>. Loose parts (no bolts left) wobble endlessly until pulled.
    /// Only owner of these rules.
    /// </summary>
    public class SalvageService : IInitializable, IStartable, IDisposable
    {
        private readonly IInputService _input;
        private readonly Camera _camera;
        private readonly IEventBus _eventBus;
        private readonly SalvageConfig _config;
        private readonly Vehicle _vehicle;

        private readonly Dictionary<Part, LooseWobble> _looseWobbles = new();

        public SalvageService(IInputService input, Camera camera, IEventBus eventBus, SalvageConfig config,
            Vehicle vehicle)
        {
            _input = input;
            _camera = camera;
            _eventBus = eventBus;
            _config = config;
            _vehicle = vehicle;
        }

        public void Initialize() => _input.SalvagePressed += OnSalvagePressed;

        // Parts authored without bolts are loose from the start.
        public void Start()
        {
            foreach (Part part in _vehicle.Parts)
                if (part != null && part.IsLoose)
                    StartLooseWobble(part);
        }

        public void Dispose()
        {
            _input.SalvagePressed -= OnSalvagePressed;
            foreach (LooseWobble wobble in _looseWobbles.Values)
                wobble.Tween.Stop();
            _looseWobbles.Clear();
        }

        private void OnSalvagePressed()
        {
            Ray ray = _camera.ScreenPointToRay(_input.PointerPosition);
            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;

            // InParent: colliders may sit on child meshes. Bolt first — bolts are children of parts.
            if (hit.collider.GetComponentInParent<Bolt>() is { } bolt)
                Unscrew(bolt, ray.direction);
            else if (hit.collider.GetComponentInParent<Part>() is { } part)
                TrySalvage(part, hit.normal);
        }

        // Spin around the bolt's axis → small shake → pull out along the axis, then destroy.
        // The bolt keeps holding its part until destroyed, so the part can't be pulled mid-unscrew.
        private void Unscrew(Bolt bolt, Vector3 viewDirection)
        {
            Transform t = bolt.transform;
            Part part = bolt.GetComponentInParent<Part>();
            DisableColliders(t);

            // Bolt axis is local Z, but its sign isn't authored — flip it to point out of the part.
            // Not hit.normal: that's the bolt's own capsule normal, ~perpendicular to the axis when
            // the side is clicked, so its sign is random.
            Vector3 axis = t.forward;
            if (Vector3.Dot(axis, OutwardHint(bolt, part, viewDirection)) < 0f)
                axis = -axis;

            Quaternion startRotation = t.rotation;
            Vector3 endPosition = t.position + axis * _config.BoltPullDistance;
            Vector3 shakeStrength = Vector3.one * _config.BoltShakeStrength;

            Sequence.Create(Tween.Custom(t, 0f, 360f * _config.BoltTurns, _config.BoltUnscrewDuration,
                    (target, angle) => target.rotation = Quaternion.AngleAxis(angle, axis) * startRotation))
                .Chain(Tween.ShakeLocalPosition(t, shakeStrength, _config.BoltShakeDuration, _config.BoltShakeFrequency))
                .Chain(Tween.Position(t, endPosition, _config.BoltPullDuration, _config.BoltPullEase))
                .OnComplete(() =>
                {
                    // Destroy is deferred, so the part still sees this bolt — exclude it explicitly.
                    if (part != null && IsHeldOnlyBy(part, bolt))
                        StartLooseWobble(part);
                    Object.Destroy(bolt.gameObject);
                });
        }

        // Logic happens immediately (money, win check); the shake + yank is only visual.
        private void TrySalvage(Part part, Vector3 surfaceNormal)
        {
            if (!part.IsLoose)
                return;

            Transform t = part.transform;
            StopLooseWobble(part);
            DisableColliders(t);
            _eventBus.Fire(new PartSalvagedEvent(part.SalvagePrice));

            Vector3 shakeStrength = Vector3.one * _config.PartShakeStrength;
            Sequence.Create(Tween.ShakeLocalPosition(t, shakeStrength, _config.PartShakeDuration, _config.PartShakeFrequency))
                .Chain(Tween.Position(t, t.position + surfaceNormal * _config.PartYankDistance,
                    _config.PartYankDuration, _config.PartYankEase))
                .OnComplete(t, target => Object.Destroy(target.gameObject));
        }

        // A bolt sits on its part's surface, so part centre → bolt points out of that face.
        // No part above the bolt: fall back to "towards the camera" (a clickable bolt faces it).
        private static Vector3 OutwardHint(Bolt bolt, Part part, Vector3 viewDirection) =>
            part != null ? bolt.transform.position - part.transform.position : -viewDirection;

        private static bool IsHeldOnlyBy(Part part, Bolt bolt)
        {
            foreach (Bolt other in part.Bolts)
                if (other != null && other != bolt)
                    return false;
            return true;
        }

        // Endless small rotation rattle; rotation so it doesn't fight the position-based strip-out.
        private void StartLooseWobble(Part part)
        {
            if (_looseWobbles.ContainsKey(part))
                return;

            Transform t = part.transform;
            Tween tween = Tween.ShakeLocalRotation(t, Vector3.one * _config.LooseWobbleStrength,
                _config.LooseWobbleCycleDuration, _config.LooseWobbleFrequency, enableFalloff: false, cycles: -1);
            _looseWobbles.Add(part, new LooseWobble(tween, t.localRotation));
        }

        private void StopLooseWobble(Part part)
        {
            if (!_looseWobbles.Remove(part, out LooseWobble wobble))
                return;

            wobble.Tween.Stop();
            part.transform.localRotation = wobble.RestRotation;
        }

        private static void DisableColliders(Transform root)
        {
            foreach (Collider c in root.GetComponentsInChildren<Collider>())
                c.enabled = false;
        }

        private readonly struct LooseWobble
        {
            public readonly Tween Tween;
            public readonly Quaternion RestRotation;

            public LooseWobble(Tween tween, Quaternion restRotation)
            {
                Tween = tween;
                RestRotation = restRotation;
            }
        }
    }
}
