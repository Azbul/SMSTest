namespace Task2.ViewModels;

using Serilog;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Task2.Helpers;
using Task2.Models;
using Task2.Services;

internal class MainWindowViewModel : INotifyPropertyChanged
{
    private const string ActualValueComment = "Актуальное значение";
    private const string DefaultValueComment = "Значение по умоланию";

    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly ConfigurationService _configurationService;

    public MainWindowViewModel()
    {
        _configurationService = new ConfigurationService();
        EnvironmentVariables = [];
        CloseCommand = new DelegateCommand(Close);
        CollapseCommand = new DelegateCommand(Collapse);
        DragMoveCommand = new DelegateCommand(DragMove);

        LoadVariables();
    }

    private ObservableCollection<EnvironmentVariable>? _environmentVariables;
    public ObservableCollection<EnvironmentVariable> EnvironmentVariables
    {
        get => _environmentVariables ?? [];
        set
        {
            _environmentVariables = value;

            foreach (var variable in _environmentVariables)
            {
                variable.VariableChanged += Variable_VariableChanged;
            }

            OnPropertyChanged(nameof(EnvironmentVariables));
        }
    }

    public ICommand CloseCommand { get; }

    public ICommand CollapseCommand { get; }

    public ICommand DragMoveCommand { get; }

    public void Close() => Application.Current.MainWindow.Close();

    public void Collapse() => Application.Current.MainWindow.WindowState = WindowState.Minimized;

    public void DragMove() => Application.Current.MainWindow.DragMove();

    public void OnPropertyChanged(string prop) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    private void LoadVariables()
    {
        var variableNames = _configurationService.GetEnvironmentVariables();

        foreach (var name in variableNames)
        {
            var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);

            if (value is not null)
            {

                EnvironmentVariables.Add(new(name, value, ActualValueComment));
            }
            else
                EnvironmentVariables.Add(new(name, string.Empty, DefaultValueComment));

            EnvironmentVariables[^1].VariableChanged += Variable_VariableChanged;
        }
    }

    private void Variable_VariableChanged(EnvironmentVariable variable)
    {
        var (name, value) = variable;

        Task.Run(
            () => Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.User));
        
        Log.Information($"Для переменной окружения {variable} установлено " +
            $"{(string.IsNullOrWhiteSpace(value) ? "пустое " : "")}значение {value}");
    }
}
