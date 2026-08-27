using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ConfigurationSummaryControlViewModel : ViewModelBase
{
	public ObservableCollection<SingleSummaryControlViewModel> SummaryViewModels { get; } = new ObservableCollection<SingleSummaryControlViewModel>();

	public ConfigurationSummaryControlViewModel()
	{
	}

	public ConfigurationSummaryControlViewModel(IEnumerable<SingleSummaryControlViewModel> viewModels)
	{
		if (viewModels == null)
		{
			throw new ArgumentNullException("viewModels");
		}
		foreach (SingleSummaryControlViewModel viewModel in viewModels)
		{
			SummaryViewModels.Add(viewModel);
		}
	}

	public void AddSummary(SingleSummaryControlViewModel summary)
	{
		SummaryViewModels.Add(summary);
	}
}
