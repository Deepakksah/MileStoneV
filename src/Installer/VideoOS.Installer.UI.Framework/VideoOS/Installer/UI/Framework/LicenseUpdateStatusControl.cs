using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework;

public partial class LicenseUpdateStatusControl : UserControl, IComponentConnector
{
	public LicenseUpdateStatusControl(bool isSuccess, bool isWarning, bool isProgress)
	{
		InitializeComponent();
		ViewModel viewModel = (ViewModel)(base.DataContext = new ViewModel());
		viewModel.WarningMessageText = string.Empty;
		viewModel.IsProgressIndicatorVisible = isProgress;
		viewModel.IsSuccessIndicatorVisible = isSuccess;
		viewModel.IsWarningIndicatorVisible = isWarning;
		viewModel.IsProcessSuccessIndicatorVisible = !isProgress;
		viewModel.UpgradeLicenseMessage = VideoOS.Installer.UI.Framework.Properties.Resources.Update_RetrieveNewLic;
		viewModel.AvailableLicenseMessage = VideoOS.Installer.UI.Framework.Properties.Resources.LicUpdate_Headline;
	}

	public void ShowWarning(string message)
	{
		if (base.DataContext is ViewModel viewModel)
		{
			viewModel.IsProgressIndicatorVisible = false;
			viewModel.IsSuccessIndicatorVisible = true;
			viewModel.IsWarningIndicatorVisible = true;
			viewModel.IsProcessSuccessIndicatorVisible = false;
			viewModel.WarningMessageText = message;
		}
	}

	public void ShowSuccess(string message)
	{
		if (base.DataContext is ViewModel viewModel)
		{
			viewModel.IsProgressIndicatorVisible = false;
			viewModel.IsSuccessIndicatorVisible = true;
			viewModel.IsWarningIndicatorVisible = false;
			viewModel.IsProcessSuccessIndicatorVisible = true;
			viewModel.UpgradeLicenseMessage = message;
		}
	}

	public void ShowProcess()
	{
		if (base.DataContext is ViewModel viewModel)
		{
			viewModel.IsProgressIndicatorVisible = true;
			viewModel.IsSuccessIndicatorVisible = true;
			viewModel.IsWarningIndicatorVisible = false;
			viewModel.IsProcessSuccessIndicatorVisible = false;
			viewModel.WarningMessageText = string.Empty;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
