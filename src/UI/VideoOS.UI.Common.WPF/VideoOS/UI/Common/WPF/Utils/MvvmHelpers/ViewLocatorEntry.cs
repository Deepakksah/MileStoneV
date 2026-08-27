using System;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class ViewLocatorEntry
{
	public Type ViewModelType { get; set; }

	public Type ViewType { get; set; }

	public string Context { get; set; }
}
