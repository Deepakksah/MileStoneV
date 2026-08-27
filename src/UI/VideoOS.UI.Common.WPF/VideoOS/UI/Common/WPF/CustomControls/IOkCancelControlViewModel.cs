using System.ComponentModel;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.UI.Common.WPF.CustomControls;

public interface IOkCancelControlViewModel
{
	DelegateCommand OkAction { get; }

	DelegateCommand CancelAction { get; }

	bool OkEnabled { get; set; }

	bool CancelEnabled { get; set; }

	event PropertyChangedEventHandler PropertyChanged;
}
