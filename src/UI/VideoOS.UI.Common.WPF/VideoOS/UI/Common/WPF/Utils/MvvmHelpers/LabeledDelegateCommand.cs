using System;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class LabeledDelegateCommand : DelegateCommand, ILabeledCommand, ICommand
{
	public string Label { get; private set; }

	public LabeledDelegateCommand(string label, Action<object> execute, Func<object, bool> canExecute)
		: base(execute, canExecute)
	{
		Label = label;
	}

	public LabeledDelegateCommand(string label, Action<object> execute)
		: this(label, execute, null)
	{
	}
}
