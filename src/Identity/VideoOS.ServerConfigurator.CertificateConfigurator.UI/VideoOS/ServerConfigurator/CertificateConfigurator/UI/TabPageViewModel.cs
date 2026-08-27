using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public abstract class TabPageViewModel : INotifyPropertyChanged
{
	private readonly ISynchronizationService _synchronizationService;

	private MessageBoxViewModel _messageBoxViewModel = MessageBoxViewModel.None;

	private string _errorMessage;

	private string _successMessage;

	private string _warningMessage;

	private bool _isBusy;

	public MessageBoxViewModel MessageBoxViewModel
	{
		get
		{
			return _messageBoxViewModel;
		}
		private set
		{
			SetProperty(ref _messageBoxViewModel, value, "MessageBoxViewModel");
		}
	}

	public bool IsBusy
	{
		get
		{
			return _isBusy;
		}
		protected set
		{
			SetProperty(ref _isBusy, value, "IsBusy");
		}
	}

	public string ErrorMessage
	{
		get
		{
			return _errorMessage;
		}
		protected set
		{
			SetProperty(ref _errorMessage, value, "ErrorMessage");
		}
	}

	public string SuccessMessage
	{
		get
		{
			return _successMessage;
		}
		protected set
		{
			SetProperty(ref _successMessage, value, "SuccessMessage");
		}
	}

	public string WarningMessage
	{
		get
		{
			return _warningMessage;
		}
		protected set
		{
			SetProperty(ref _warningMessage, value, "WarningMessage");
		}
	}

	public virtual Visibility Visibility => Visibility.Visible;

	public ICommand ClearSuccessMessage { get; }

	public ICommand ClearErrorMessage { get; }

	public ICommand ClearWarningMessage { get; }

	public event PropertyChangedEventHandler PropertyChanged;

	protected TabPageViewModel(ISynchronizationService synchronizationService)
	{
		_synchronizationService = synchronizationService;
		ClearSuccessMessage = new CommandHandler(delegate
		{
			SuccessMessage = string.Empty;
		});
		ClearErrorMessage = new CommandHandler(delegate
		{
			ErrorMessage = string.Empty;
		});
		ClearWarningMessage = new CommandHandler(delegate
		{
			WarningMessage = string.Empty;
		});
	}

	public virtual void ShowErrorOrWarning(int errorMessageCode)
	{
	}

	protected Task<MessageBoxResult> ShowMessage(MessageBoxViewModel viewModel)
	{
		MessageBoxViewModel = viewModel ?? MessageBoxViewModel.None;
		return MessageBoxViewModel.Response;
	}

	protected void SafeCall(Action action)
	{
		if (!_synchronizationService.IsSynchronizationRequired())
		{
			action?.Invoke();
		}
		else
		{
			_synchronizationService.Invoke(action);
		}
	}

	protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
	{
		if (!object.Equals(field, value))
		{
			field = value;
			OnPropertyChanged(propertyName);
		}
	}

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
