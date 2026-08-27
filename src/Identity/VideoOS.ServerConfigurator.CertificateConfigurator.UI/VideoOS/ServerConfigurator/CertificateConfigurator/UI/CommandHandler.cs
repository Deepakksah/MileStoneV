using System;
using System.Windows.Input;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class CommandHandler : ICommand
{
	private readonly Action _action;

	private bool _canExecute = true;

	public bool CanExecute
	{
		get
		{
			return _canExecute;
		}
		set
		{
			if (_canExecute != value)
			{
				_canExecute = value;
				this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public event EventHandler CanExecuteChanged;

	public CommandHandler(Action action)
	{
		_action = action ?? throw new ArgumentNullException("action");
	}

	bool ICommand.CanExecute(object parameter)
	{
		return CanExecute;
	}

	void ICommand.Execute(object parameter)
	{
		_action();
	}
}
