using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;
using VideoOS.Utilities.Validation;

namespace VideoOS.UI.Common.WPF.CustomControls;

public class CloseControlViewModel : ViewModelBase, ICloseControlViewModel
{
	private readonly DelegateCommand _closeAction;

	private bool _closeEnabled;

	public bool CloseEnabled
	{
		get
		{
			return _closeEnabled;
		}
		set
		{
			SetProperty(ref _closeEnabled, value, () => CloseEnabled, "CloseEnabled");
		}
	}

	public DelegateCommand CloseAction => _closeAction;

	public CloseControlViewModel(DelegateCommand closeAction, bool closeEnabled = true)
	{
		closeAction.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\SC_CommonFiles\\VideoOS.UI.Common.WPF\\CustomControls\\CloseControl\\CloseControlViewModel.cs", 17);
		_closeAction = closeAction;
		_closeEnabled = closeEnabled;
	}
}
