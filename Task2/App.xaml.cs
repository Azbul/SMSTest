using Serilog;
using System.IO;
using System.Windows;

namespace Task2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        $"test-sms-wpf-app-{DateTime.Now:yyyyMMdd}.log"),
                    rollingInterval: RollingInterval.Day
                )
                .CreateLogger();

            Log.Information("Приложение запущено.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Log.CloseAndFlush();
        }
    }

}
