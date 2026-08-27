using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ScanHardwareWizard : WizardPageBase
{
	private ElementHost _elementHost;

	private ScanHardwareControl _wpfControl = new ScanHardwareControl();

	private ScanHardwareViewModel _viewModel;

	private BackgroundWorker _scanThread;

	private IContainer components;

	public ScanHardwareWizard()
	{
		InitializeComponent();
		PageName = "ScanHardwarePage";
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_wpfControl = new ScanHardwareControl();
		_viewModel = new ScanHardwareViewModel();
		_wpfControl.DataContext = _viewModel;
		_elementHost.Child = _wpfControl;
		LoadResources();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		Resources.Culture = culture;
		base.ReloadResources(culture);
		LoadResources();
	}

	private void LoadResources()
	{
		_wpfControl.labelHeadLine.Text = Resources.HardwareScanHeader;
		_viewModel.Header = Resources.RecorderOnlineVerfying;
		_viewModel.SetProgress(0);
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.CancelButton.Visible = false;
		Wizard.HideNavigationButtons();
		_scanThread = new BackgroundWorker();
		_scanThread.DoWork += _viewModel.InitStartHardwareScan;
		_scanThread.RunWorkerCompleted += ScanThread_RunWorkerCompleted;
		_scanThread.WorkerReportsProgress = true;
		_scanThread.RunWorkerAsync();
	}

	private void ScanThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		Wizard.MoveToNextPage();
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return new Dictionary<string, string>
		{
			{
				ServerParameters.ScannedHardware.ToString(),
				_viewModel.FoundCamerasXml.ToString()
			},
			{
				ServerParameters.HardwareScanErrorCode.ToString(),
				_viewModel.ScanErrorNumber.ToString()
			}
		};
	}

	public override bool OnBeforeActivateOnPrevious()
	{
		return false;
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
			if (_scanThread != null)
			{
				_scanThread.Dispose();
				_scanThread = null;
			}
			if (components != null)
			{
				components.Dispose();
				components = null;
			}
			if (_viewModel != null)
			{
				_viewModel.Dispose();
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
		base.Name = "ScanHardwareWizard";
		this.PageName = "ScanHardwareWizardPage";
		base.ResumeLayout(false);
	}
}
