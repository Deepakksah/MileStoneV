using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class HardwareConfigurationWizard : WizardPageBase
{
	private ElementHost _elementHost;

	private HardwareSelectionAndConfigurationPageControl _wpfControl = new HardwareSelectionAndConfigurationPageControl();

	private HardwareConfigurationViewModel _viewModel;

	private IContainer components;

	public HardwareConfigurationWizard()
	{
		InitializeComponent();
		PageName = "HardwareConfigurationPage";
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_wpfControl = new HardwareSelectionAndConfigurationPageControl();
		_elementHost.Child = _wpfControl;
		_viewModel = new HardwareConfigurationViewModel();
		_wpfControl.DataContext = _viewModel;
		SetResources();
	}

	protected override void Dispose(bool disposing)
	{
		if (components != null)
		{
			components.Dispose();
			components = null;
		}
		base.Dispose(disposing);
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return new Dictionary<string, string> { 
		{
			ServerParameters.ConfiguredHardware.ToString(),
			HardwareConfigurations.GetAsXml(_viewModel.HardwareConfiguration)
		} };
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (parameters.TryGetValue(ServerParameters.AddedHardware.ToString(), out var value))
		{
			try
			{
				HardwareConfigurations fromXml = HardwareConfigurations.GetFromXml(value);
				_viewModel.SetHardware(fromXml);
			}
			catch
			{
				LogManager.Write("HardwareConfigurationWizard: Failed to extract added hardware", TraceEventType.Information);
			}
		}
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.CancelButton.Visible = true;
		Wizard.NextButton.Visible = true;
		Wizard.NextButton.Enabled = true;
		Wizard.NavigationPanel.Visible = true;
		Wizard.HidePreviousButton();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		SetResources();
	}

	private void SetResources()
	{
		_viewModel.LoadResources();
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("466FCDDE-8758-4961-BF0D-84FDC2635DB7");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "HardwareConfigurationWizard";
		this.PageName = "HardwareConfigurationPage";
		base.ResumeLayout(false);
	}
}
