namespace Task2.Helpers;

using System.Windows.Input;

class DelegateCommand(Action execute, Func<bool>? canExecute) : ICommand
{    
    public DelegateCommand(Action execute)
        : this(execute, () => true) { }
 
    public event EventHandler? CanExecuteChanged
    {
        add
        {
            CommandManager.RequerySuggested += value;
        }

        remove
        {
            CommandManager.RequerySuggested -= value;
        }
    }

    public bool CanExecute(object? parameter) => canExecute?.Invoke() ?? true;
    public void Execute(object? parameter) => execute();
}
