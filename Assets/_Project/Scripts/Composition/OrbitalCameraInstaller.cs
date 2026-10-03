using EclipseJam.Gameplay.OrbitalCamera;
using Unity.Cinemachine;
using VContainer;
using VContainer.Unity;

namespace EclipseJam.Composition
{
    public class OrbitalCameraInstaller : IInstaller
    {
        private readonly CinemachineOrbitalFollow _orbitalFollow;
        private readonly OrbitalCameraConfig _config;

        public OrbitalCameraInstaller(CinemachineOrbitalFollow orbitalFollow, OrbitalCameraConfig config)
        {
            _orbitalFollow = orbitalFollow;
            _config = config;
        }

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(_config);
            builder.RegisterComponent(_orbitalFollow);
            builder.RegisterEntryPoint<OrbitalCameraService>();
        }
    }
}
