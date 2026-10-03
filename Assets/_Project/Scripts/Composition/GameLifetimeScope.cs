using EclipseJam.Gameplay.OrbitalCamera;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EclipseJam.Composition
{
    /// <summary>
    /// Scope of the Game scene: lives while the scene is loaded. Parent is the root
    /// <see cref="ProjectLifetimeScope"/> (picked up from VContainerSettings automatically).
    /// Level systems, spawners and UI presenters go here — one installer line per feature.
    /// </summary>
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Orbital Camera")]
        [SerializeField] private CinemachineOrbitalFollow orbitalFollow;
        [SerializeField] private OrbitalCameraConfig orbitalCameraConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            new OrbitalCameraInstaller(orbitalFollow, orbitalCameraConfig).Install(builder);
        }
    }
}
