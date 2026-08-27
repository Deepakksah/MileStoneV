using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ConfigureHardwareProgressWizard : WizardPageBase
{
	private ElementHost _elementHost;

	private ScanHardwareControl _wpfControl = new ScanHardwareControl();

	private ConfigureHardwareViewModel _viewModel;

	private BackgroundWorker _configureThread;

	private IContainer components;

	public ConfigureHardwareProgressWizard()
	{
		InitializeComponent();
		PageName = "ConfigureHardwareProgressPage";
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_wpfControl = new ScanHardwareControl();
		_viewModel = new ConfigureHardwareViewModel();
		_wpfControl.DataContext = _viewModel;
		_elementHost.Child = _wpfControl;
		LoadResources();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	private void LoadResources()
	{
		_wpfControl.labelHeadLine.Text = Resources.HardwareConfigurationProgressHeader;
		_viewModel.Header = Resources.HardwareConfigurationProgressInfo;
		_viewModel.SetProgress(0);
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (parameters.TryGetValue(ServerParameters.ConfiguredHardware.ToString(), out var value))
		{
			try
			{
				HardwareConfigurations fromXml = HardwareConfigurations.GetFromXml(value);
				_viewModel.SetConfiguredHardware(fromXml);
			}
			catch
			{
				LogManager.Write("CameraUserNamePassword: Failed to extract data", TraceEventType.Information);
			}
		}
		StartConfigure();
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.CancelButton.Visible = false;
		Wizard.HideNavigationButtons();
	}

	private void StartConfigure()
	{
		_configureThread = new BackgroundWorker();
		_configureThread.DoWork += _viewModel.InitStartConfiguringHardware;
		_configureThread.RunWorkerCompleted += ConfigureThread_RunWorkerCompleted;
		_configureThread.WorkerReportsProgress = true;
		_configureThread.RunWorkerAsync();
	}

	private void ConfigureThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		Wizard.MoveToNextPage();
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
			if (_configureThread != null)
			{
				_configureThread.Dispose();
				_configureThread = null;
			}
			if (components != null)
			{
				components.Dispose();
				components = null;
			}
			if (_viewModel != null)
			{
				_viewModel = null;
			}
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("45984772-2c05-466c-adb8-fbca0067e62e");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "ConfigureHardwareProgressWizard";
		this.PageName = "ConfigureHardwareProgressWizardPage";
		base.ResumeLayout(false);
	}
}
