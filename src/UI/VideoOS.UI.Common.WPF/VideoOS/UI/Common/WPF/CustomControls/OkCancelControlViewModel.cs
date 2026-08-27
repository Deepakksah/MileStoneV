using System.Windows;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;
using VideoOS.Utilities.Validation;

namespace VideoOS.UI.Common.WPF.CustomControls;

public class OkCancelControlViewModel : ViewModelBase, IOkCancelControlViewModel
{
	private readonly DelegateCommand _okAction;

	private readonly DelegateCommand _cancelAction;

	private bool _okEnabled;

	private bool _cancelEnabled;

	public DelegateCommand OkAction => _okAction;

	public DelegateCommand CancelAction => _cancelAction;

	public bool OkEnabled
	{
		get
		{
			return _okEnabled;
		}
		set
		{
			SetProperty(ref _okEnabled, value, () => OkEnabled, "OkEnabled");
		}
	}

	public bool CancelEnabled
	{
		get
		{
			return _cancelEnabled;
		}
		set
		{
			SetProperty(ref _cancelEnabled, value, () => CancelEnabled, "CancelEnabled");
		}
	}

	public OkCancelControlViewModel(DelegateCommand okAction, DelegateCommand cancelAction, bool cancelEnabled = true, bool okEnabled = true)
	{
		okAction.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\SC_CommonFiles\\VideoOS.UI.Common.WPF\\CustomControls\\OkCancelControl\\OkCancelControlViewModel.cs", 20);
		cancelAction.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\SC_CommonFiles\\VideoOS.UI.Common.WPF\\CustomControls\\OkCancelControl\\OkCancelControlViewModel.cs", 21);
		_cancelAction = cancelAction;
		_okAction = okAction;
		_cancelEnabled = cancelEnabled;
		_okEnabled = okEnabled;
	}

	public OkCancelControlViewModel(DelegateCommand okAction)
		: this(okAction, new DelegateCommand(delegate(object o)
		{
			Cancel((Window)o);
		}))
	{
	}

	private static void Cancel(Window window)
	{
		window.Close();
	}
}
