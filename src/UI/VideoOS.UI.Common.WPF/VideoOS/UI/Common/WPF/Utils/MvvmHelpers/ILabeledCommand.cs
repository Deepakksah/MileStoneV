using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public interface ILabeledCommand : ICommand
{
	string Label { get; }
}
