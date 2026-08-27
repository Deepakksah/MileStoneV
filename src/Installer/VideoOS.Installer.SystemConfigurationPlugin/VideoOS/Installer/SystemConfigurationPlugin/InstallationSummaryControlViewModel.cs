using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class InstallationSummaryControlViewModel : ViewModelBase
{
	public ObservableCollection<string> Components { get; } = new ObservableCollection<string>();

	public InstallationSummaryControlViewModel()
	{
	}

	public InstallationSummaryControlViewModel(IEnumerable<string> components)
	{
		AddComponents(components);
	}

	public void AddComponents(IEnumerable<string> components)
	{
		if (components == null)
		{
			throw new ArgumentNullException("components");
		}
		foreach (string component in components)
		{
			Components.Add(component);
		}
	}
}
