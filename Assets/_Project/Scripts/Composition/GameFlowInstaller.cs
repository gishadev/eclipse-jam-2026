using gishadev.eclipse.Gameplay.GameFlow;
using gishadev.eclipse.Gameplay.Salvage;
using VContainer;
using VContainer.Unity;

namespace gishadev.eclipse.Composition
{
    public class GameFlowInstaller : IInstaller
    {
        private readonly Vehicle _vehicle;
        private readonly GameConfig _config;

        public GameFlowInstaller(Vehicle vehicle, GameConfig config)
        {
            _vehicle = vehicle;
            _config = config;
        }

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(_config);
            builder.RegisterComponent(_vehicle);
            // AsSelf: timer/result UI will read State, RemainingTime, RemainingParts.
            builder.RegisterEntryPoint<GameController>().AsSelf();
        }
    }
}
