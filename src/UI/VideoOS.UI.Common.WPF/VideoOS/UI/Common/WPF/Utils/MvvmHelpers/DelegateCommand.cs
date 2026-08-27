using System;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class DelegateCommand : ICommand
{
	private static readonly DelegateCommand _empty = new DelegateCommand(delegate
	{
	}, null)
	{
		Enabled = false
	};

	private bool _enabled = true;

	public static DelegateCommand Empty => _empty;

	public Action<object> ExecuteDelegate { get; private set; }

	public Func<object, bool> CanExecuteDelegate { get; private set; }

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			if (_enabled != value)
			{
				_enabled = value;
				CommandManager.InvalidateRequerySuggested();
			}
		}
	}

	private event EventHandler _canExecuteChanged;

	public event EventHandler CanExecuteChanged
	{
		add
		{
			_canExecuteChanged += value;
			CommandManager.RequerySuggested += value;
		}
		remove
		{
			_canExecuteChanged -= value;
			CommandManager.RequerySuggested -= value;
		}
	}

	public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
	{
		ExecuteDelegate = execute;
		CanExecuteDelegate = canExecute;
	}

	public DelegateCommand(Action<object> execute)
		: this(execute, null)
	{
	}

	public bool CanExecute(object parameter)
	{
		if (_enabled)
		{
			if (CanExecuteDelegate != null)
			{
				return CanExecuteDelegate(parameter);
			}
			return true;
		}
		return false;
	}

	public void Execute(object parameter)
	{
		ExecuteDelegate(parameter);
	}

	public void FireCanExecuteChanged()
	{
		if (this._canExecuteChanged != null)
		{
			OnCanExecuteChanged();
		}
	}

	protected virtual void OnCanExecuteChanged()
	{
		this._canExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}
