using System;
using System.Windows.Input;

namespace VideoOS.Platform.UI.Commands;

public class DelegateCommand : ICommand
{
	private readonly Action<object> _execute;

	private bool __isEnabled = true;

	public bool IsEnabled
	{
		get
		{
			return __isEnabled;
		}
		set
		{
			if (__isEnabled != value)
			{
				__isEnabled = value;
				this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public event EventHandler CanExecuteChanged;

	public DelegateCommand(Action<object> execute)
	{
		_execute = execute;
	}

	public bool CanExecute(object parameter)
	{
		return IsEnabled;
	}

	public void Execute(object parameter)
	{
		_execute(parameter);
	}
}
