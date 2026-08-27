using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class PrivacySettingsWizard : WizardPageBase
{
	public const string PageNameConstant = "PrivacySettingsPage";

	private ElementHost _elementHost;

	private PrivacySettingsViewModel _viewModel;

	private IContainer components;

	[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "It is disposed. Trust me, I'm a programmer")]
	public PrivacySettingsWizard()
	{
		InitializeComponent();
		PageName = "PrivacySettingsPage";
		_elementHost = new ElementHost
		{
			Dock = DockStyle.Fill
		};
		WizardPage.Controls.Add(_elementHost);
		_viewModel = new PrivacySettingsViewModel();
		PrivacySettingsControl child = new PrivacySettingsControl
		{
			DataContext = _viewModel
		};
		_elementHost.Child = child;
	}

	public override bool OnBeforeActivate()
	{
		_viewModel.ShareUsageData = true;
		return base.OnBeforeActivate();
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
	}

	public override bool OnBeforeContinue()
	{
		if (_viewModel.ShareUsageData && new ConfirmationDialog().ShowDialog() != true)
		{
			return false;
		}
		return base.OnBeforeContinue();
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return new Dictionary<string, string> { ["USAGEDATA"] = _viewModel.ShareUsageData.ToString() };
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_elementHost != null)
			{
				if (_elementHost.Child != null)
				{
					_elementHost.Child = null;
				}
				_elementHost.Dispose();
				_elementHost = null;
			}
			if (components != null)
			{
				components.Dispose();
				components = null;
			}
		}
		base.Dispose(disposing);
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		_viewModel.ReloadResources();
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.ServerPagesSetupPlugin.PrivacySettingsWizard));
		base.WizardPage = new VideoOS.Installer.UI.Framework.WizardPage();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("5931FCDE-CC9C-4E4A-A4E8-0F51DE4D671E");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "PrivacySettingsWizard";
		base.ResumeLayout(false);
	}
}
