using gishadev.eclipse.Gameplay.OrbitalCamera;
using gishadev.eclipse.Gameplay.Salvage;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace gishadev.eclipse.Composition
{
    /// <summary>
    /// Scope of the Game scene: lives while the scene is loaded. Parent is the root
    /// <see cref="ProjectLifetimeScope"/> (picked up from VContainerSettings automatically).
    /// Level systems, spawners and UI presenters go here — one installer line per feature.
    /// </summary>
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Camera mainCamera;

        [Header("Orbital Camera")]
        [SerializeField] private CinemachineOrbitalFollow orbitalFollow;
        [SerializeField] private OrbitalCameraConfig orbitalCameraConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(mainCamera);

            new OrbitalCameraInstaller(orbitalFollow, orbitalCameraConfig).Install(builder);
            builder.RegisterEntryPoint<SalvageService>();
        }
    }
}
