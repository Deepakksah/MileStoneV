using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.UI;

public class WizardPageWarning : WizardPageSubscription<WarningPageContent, WarningPageViewModel>
{
	private IContainer components;

	protected override IEnumerable<IWizardCommonPageSubscription<WarningPageContent>> Subscriptions => InstallerManager.Instance.SubscriptionPlugins.WarningSubscriptions;

	public WizardPageWarning()
		: base("WarningCommonPage", (System.Windows.Controls.UserControl)new WarningPageControl())
	{
		InitializeComponent();
	}

	protected override WarningPageViewModel CreateViewModel(IEnumerable<WarningPageContent> content)
	{
		WarningPageViewModel warningPageViewModel = new WarningPageViewModel(from c in content
			orderby c.Rank
			select new WarningPageEntryViewModel(c));
		warningPageViewModel.PropertyChanged += delegate
		{
			TriggerValidation();
		};
		return warningPageViewModel;
	}

	protected override bool ValidateViewModel(WarningPageViewModel viewModel)
	{
		return viewModel?.CheckBoxChecked ?? true;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	}
}
