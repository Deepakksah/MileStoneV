using System.ComponentModel;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.UI.Common.WPF.CustomControls;

public interface ICloseControlViewModel
{
	bool CloseEnabled { get; set; }

	DelegateCommand CloseAction { get; }

	event PropertyChangedEventHandler PropertyChanged;
}
