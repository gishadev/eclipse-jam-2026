using gishadev.eclipse.GUI;
using VContainer;
using VContainer.Unity;

namespace gishadev.eclipse.Composition
{
    public class MoneyGUIInstaller : IInstaller
    {
        private readonly MoneyView _view;

        public MoneyGUIInstaller(MoneyView view) => _view = view;

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterComponent(_view);
            builder.RegisterEntryPoint<MoneyPresenter>();
        }
    }
}
