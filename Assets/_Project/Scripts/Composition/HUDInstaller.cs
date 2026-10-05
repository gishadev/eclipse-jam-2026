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
        private readonly ResultView _resultView;
        private readonly StartView _startView;
        private readonly SettingsView _settingsView;

        public HUDInstaller(MoneyView moneyView, TimerView timerView, ResultView resultView, StartView startView,
            SettingsView settingsView)
        {
            _moneyView = moneyView;
            _timerView = timerView;
            _resultView = resultView;
            _startView = startView;
            _settingsView = settingsView;
        }

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterComponent(_moneyView);
            builder.RegisterEntryPoint<MoneyPresenter>();

            builder.RegisterComponent(_timerView);
            builder.RegisterEntryPoint<TimerPresenter>();

            builder.RegisterComponent(_resultView);
            builder.RegisterEntryPoint<ResultPresenter>();

            builder.RegisterComponent(_startView);
            builder.RegisterEntryPoint<StartPresenter>();

            builder.RegisterComponent(_settingsView);
            builder.RegisterEntryPoint<SettingsPresenter>();
        }
    }
}
