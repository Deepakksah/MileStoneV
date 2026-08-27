using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class AsyncDelegateCommand : IAsyncCommand, ICommand
{
	private Func<object, Task> _asyncExecute;

	private readonly Predicate<object> _canExecute;

	public event EventHandler CanExecuteChanged
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

	public AsyncDelegateCommand(Func<object, Task> asyncExecute)
		: this(asyncExecute, null)
	{
	}

	public AsyncDelegateCommand(Func<object, Task> asyncExecute, Predicate<object> canExecute)
	{
		_asyncExecute = asyncExecute;
		_canExecute = canExecute;
	}

	public bool CanExecute(object parameter)
	{
		if (_canExecute == null)
		{
			return true;
		}
		return _canExecute(parameter);
	}

	public async void Execute(object parameter)
	{
		await ExecuteAsync(parameter);
	}

	public async Task ExecuteAsync(object parameter)
	{
		await _asyncExecute(parameter).ConfigureAwait(continueOnCapturedContext: false);
	}
}
