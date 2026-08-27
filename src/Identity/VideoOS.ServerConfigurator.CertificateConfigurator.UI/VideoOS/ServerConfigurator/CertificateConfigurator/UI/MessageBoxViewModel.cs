using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class MessageBoxViewModel : INotifyPropertyChanged
{
	private TaskCompletionSource<MessageBoxResult> _result = new TaskCompletionSource<MessageBoxResult>();

	public static MessageBoxViewModel None => new MessageBoxViewModel();

	public string Title { get; private set; }

	public string Message { get; private set; }

	public string OkButtonText { get; private set; }

	public string CancelButtonText { get; private set; }

	public bool IsWarning { get; private set; }

	public ICommand OkCommand { get; }

	public ICommand CancelCommand { get; }

	public Task<MessageBoxResult> Response => _result.Task;

	public event PropertyChangedEventHandler PropertyChanged;

	private MessageBoxViewModel()
	{
		_result.SetResult(MessageBoxResult.None);
	}

	public MessageBoxViewModel(string title, string message, string okButtonText, string cancelButtonText, bool isWarning)
	{
		Title = title;
		Message = message;
		OkButtonText = okButtonText;
		CancelButtonText = cancelButtonText;
		IsWarning = isWarning;
		OkCommand = new CommandHandler(delegate
		{
			SetResult(MessageBoxResult.OK);
		});
		CancelCommand = new CommandHandler(delegate
		{
			SetResult(MessageBoxResult.Cancel);
		});
	}

	private void SetResult(MessageBoxResult result)
	{
		_result.SetResult(result);
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Response"));
	}
}
