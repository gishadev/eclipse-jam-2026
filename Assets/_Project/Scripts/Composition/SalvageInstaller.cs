using gishadev.eclipse.Gameplay.Salvage;
using VContainer;
using VContainer.Unity;

namespace gishadev.eclipse.Composition
{
    public class SalvageInstaller : IInstaller
    {
        private readonly SalvageConfig _config;

        public SalvageInstaller(SalvageConfig config) => _config = config;

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(_config);
            builder.RegisterEntryPoint<SalvageService>();
        }
    }
}
