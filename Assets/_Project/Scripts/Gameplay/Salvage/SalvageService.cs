using System;
using gishadev.eclipse.Core.Events;
using gishadev.eclipse.Core.Input;
using gishadev.tools.Events;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace gishadev.eclipse.Gameplay.Salvage
{
    /// <summary>
    /// On salvage click, raycasts from the main camera through the pointer:
    /// a <see cref="Bolt"/> gets unscrewed (destroyed for now), a loose <see cref="Part"/> gets salvaged
    /// and fires <see cref="PartSalvagedEvent"/>. Only owner of these rules.
    /// </summary>
    public class SalvageService : IInitializable, IDisposable
    {
        private readonly IInputService _input;
        private readonly Camera _camera;
        private readonly IEventBus _eventBus;

        public SalvageService(IInputService input, Camera camera, IEventBus eventBus)
        {
            _input = input;
            _camera = camera;
            _eventBus = eventBus;
        }

        public void Initialize() => _input.SalvagePressed += OnSalvagePressed;

        public void Dispose() => _input.SalvagePressed -= OnSalvagePressed;

        private void OnSalvagePressed()
        {
            Ray ray = _camera.ScreenPointToRay(_input.PointerPosition);
            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;

            // InParent: colliders may sit on child meshes. Bolt first — bolts are children of parts.
            if (hit.collider.GetComponentInParent<Bolt>() is { } bolt)
                Unscrew(bolt);
            else if (hit.collider.GetComponentInParent<Part>() is { } part)
                TrySalvage(part);
        }

        private static void Unscrew(Bolt bolt) => Object.Destroy(bolt.gameObject);

        private void TrySalvage(Part part)
        {
            if (!part.IsLoose)
                return;

            int price = part.SalvagePrice;
            Object.Destroy(part.gameObject);
            _eventBus.Fire(new PartSalvagedEvent(price));
        }
    }
}
