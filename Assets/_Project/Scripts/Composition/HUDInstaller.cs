using gishadev.eclipse.GUI;
using VContainer;
using VContainer.Unity;

namespace gishadev.eclipse.Composition
{
    /// <summary>In-game HUD: each view is a passive scene component driven by its presenter.</summary>
    public class HUDInstaller : IInstaller
    {
        private readonly MoneyView _moneyView;
        private readonly TimerView _timerView;

        public HUDInstaller(MoneyView moneyView, TimerView timerView)
        {
            _moneyView = moneyView;
            _timerView = timerView;
        }

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterComponent(_moneyView);
            builder.RegisterEntryPoint<MoneyPresenter>();

            builder.RegisterComponent(_timerView);
            builder.RegisterEntryPoint<TimerPresenter>();
        }
    }
}
