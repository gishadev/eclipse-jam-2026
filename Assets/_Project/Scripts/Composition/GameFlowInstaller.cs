using gishadev.eclipse.Gameplay.GameFlow;
using gishadev.eclipse.Gameplay.Salvage;
using gishadev.eclipse.Visuals;
using VContainer;
using VContainer.Unity;

namespace gishadev.eclipse.Composition
{
    public class GameFlowInstaller : IInstaller
    {
        private readonly Vehicle _vehicle;
        private readonly GameConfig _config;
        private readonly PlatformAnimationHandler _platformAnimation;

        public GameFlowInstaller(Vehicle vehicle, GameConfig config, PlatformAnimationHandler platformAnimation)
        {
            _vehicle = vehicle;
            _config = config;
            _platformAnimation = platformAnimation;
        }

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(_config);
            builder.RegisterComponent(_vehicle);
            builder.RegisterComponent(_platformAnimation);
            // AsSelf: timer/result UI will read State, RemainingTime, RemainingParts.
            builder.RegisterEntryPoint<GameController>().AsSelf();
        }
    }
}
