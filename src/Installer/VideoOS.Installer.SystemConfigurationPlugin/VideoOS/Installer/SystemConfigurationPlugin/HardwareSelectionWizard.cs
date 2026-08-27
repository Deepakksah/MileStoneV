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

public class HardwareSelectionWizard : WizardPageBase
{
	private ElementHost _elementHost;

	private HardwareSelectionAndConfigurationPageControl _wpfControl = new HardwareSelectionAndConfigurationPageControl();

	private HardwareSelectionViewModel _viewModel;

	private IContainer components;

	public HardwareSelectionWizard()
	{
		InitializeComponent();
		PageName = "HardwareSelectionPage";
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_wpfControl = new HardwareSelectionAndConfigurationPageControl();
		_viewModel = new HardwareSelectionViewModel();
		_wpfControl.DataContext = _viewModel;
		_elementHost.Child = _wpfControl;
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
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		HardwareConfigurations hardwareConfigurations = new HardwareConfigurations();
		foreach (HardwareData item in _viewModel.Hardware)
		{
			if (item.Enabled)
			{
				hardwareConfigurations.Add(item.HardwareConfiguration);
			}
		}
		dictionary.Add(ServerParameters.SelectedHardware.ToString(), HardwareConfigurations.GetAsXml(hardwareConfigurations));
		return dictionary;
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		_viewModel.Hardware.Clear();
		if (!parameters.TryGetValue(ServerParameters.ScannedHardware.ToString(), out var value))
		{
			return;
		}
		try
		{
			HardwareConfigurations fromXml = HardwareConfigurations.GetFromXml(value);
			if (fromXml.Count <= 0)
			{
				SetMessageOnError(parameters);
			}
			else
			{
				_viewModel.SetHardware(fromXml);
			}
		}
		catch
		{
			LogManager.Write("HardwareSelectionWizard: Failed to extract data", TraceEventType.Information);
		}
	}

	private void SetMessageOnError(Dictionary<string, string> parameters)
	{
		if (parameters.TryGetValue(ServerParameters.HardwareScanErrorCode.ToString(), out var value) && !string.IsNullOrEmpty(value))
		{
			ScanErrorCode scanErrorCode = (ScanErrorCode)Enum.Parse(typeof(ScanErrorCode), value);
			_viewModel.NoCameraMessage = ScanResults.GetErrorMessageFromCode(scanErrorCode);
			if (scanErrorCode == ScanErrorCode.Timeout)
			{
				Wizard.HidePreviousButton();
			}
		}
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.CancelButton.Visible = true;
		Wizard.NextButton.Visible = true;
		Wizard.NextButton.Enabled = true;
		Wizard.NavigationPanel.Visible = true;
		Wizard.ShowPreviousButton();
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
		base.WizardPage.FlowId = new System.Guid("2808ACEB-88B6-471C-B1F8-435EDD6F84D7");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "HardwareSelectionWizard";
		this.PageName = "HardwareSelectionPage";
		base.ResumeLayout(false);
	}
}
